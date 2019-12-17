using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lodis.GamePlay
{
	public class PauseMenuBehaviour : MonoBehaviour
	{
		//raised when the game is paused
		[SerializeField]
		private Event OnPause;
		//raised when the game is unpaused
		[SerializeField]
		private Event OnUnPause;
		//true if the game is poused
		private bool isPaused;
		[SerializeField] private GameObject _controlsPanel;
		[SerializeField] private List<Event> _actions;
		[SerializeField] private List<Text> _displayOptions;
		private int _currentIndex;
		private bool _canPressButton;
		private bool _controlWindowUp;

		private void Start()
		{
			_controlWindowUp = false;
		}

		public void GoToNextOption()
		{
			Debug.Log("got here");
			if (isPaused)
			{
				
				_displayOptions[_currentIndex].color = Color.white;
				_currentIndex++;
				if (_currentIndex > 3)
				{
					_currentIndex = 0;
				}
				_displayOptions[_currentIndex].color = Color.blue;
			}
		}
		public void GoToPreviousOption()
		{
			if (isPaused)
			{
				
				_displayOptions[_currentIndex].color = Color.white;
				_currentIndex--;
				if (_currentIndex < 0)
				{
					_currentIndex = 3;
				}

				_displayOptions[_currentIndex].color = Color.blue;
			}
		}

		public void PauseGame()
		{
			if (Time.timeScale == 1)
			{
				
				Time.timeScale = 0;
				isPaused = true;
				OnPause.Raise();
			}
			else
			{
				Time.timeScale = 1;
				isPaused = false;
				OnUnPause.Raise();
			}
		}

		public void ToggleControlsMenu()
		{
			if (isPaused)
			{
				_controlsPanel.SetActive(!_controlsPanel.activeSelf);
				_controlWindowUp = !_controlWindowUp;
				_canPressButton = false;
			}
		}
		public void Restart()
		{
			if (isPaused)
			{
				PauseGame();
				SceneManager.LoadScene(0);
			}
		}
		public void Quit()
		{
			if (isPaused)
			{
				Application.Quit();
			}
		}

		public void DoCurrentAction()
		{
			if (isPaused)
			{
				_actions[_currentIndex].Raise();
			}
		}

		private void Update()
		{
			if (Input.GetButtonDown("Pause"))
			{
				PauseGame();
			}

			if ((Input.GetAxis("Vertical1") < .5 && Input.GetAxis("Vertical1") > -.5) && Input.GetAxis("Vertical1") > -.5&&(Input.GetAxis("Vertical1") < .5 && (Input.GetAxis("Vertical1") > -.5) && Input.GetAxis("Vertical1") > -.5))
			{
				if (_controlWindowUp == false) 
				{
					_canPressButton = true;
				}
			}
			
			if (Input.GetAxis("Vertical1") >= .8 && _canPressButton)
			{
				_canPressButton = false;
				GoToPreviousOption();
			}
			else if (Input.GetAxisRaw("Vertical1") <= -.8 && _canPressButton)
			{
				_canPressButton = false;
				GoToNextOption();
			}
			if (Input.GetAxis("Vertical2") >= .8 && _canPressButton)
			{
				_canPressButton = false;
				GoToPreviousOption();
			}
			else if (Input.GetAxisRaw("Vertical2") <= -.8 && _canPressButton)
			{
				_canPressButton = false;
				GoToNextOption();
			}
			if (Input.GetButtonDown("Submit"))
			{
				DoCurrentAction();
			}
		}
	}

}

