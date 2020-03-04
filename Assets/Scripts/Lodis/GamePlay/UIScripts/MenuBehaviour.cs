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
    public class MenuBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _controlsPanel;
        [SerializeField] private Event onSelected;
        [SerializeField] private List<Image> _displayOptions;
        private int _currentIndex;
        private bool _canPressButton;
        private bool _controlWindowUp;
        public bool gameWon;
        private void Start()
        {
            _controlWindowUp = false;
        }

        public void GoToNextOption()
        {
            _displayOptions[_currentIndex].color = Color.yellow;
            _currentIndex++;
            if (_currentIndex > _displayOptions.Count - 1)
            {
                _currentIndex = 0;
            }
            _displayOptions[_currentIndex].color = Color.white;
        }
        public void GoToPreviousOption()
        {
            _displayOptions[_currentIndex].color = Color.yellow;
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _displayOptions.Count - 1;
            }
            _displayOptions[_currentIndex].color = Color.white;
        }

        public void ToggleControlsMenu()
        {
                _controlsPanel.SetActive(!_controlsPanel.activeSelf);
                _controlWindowUp = !_controlWindowUp;
                _canPressButton = false;
        }
        public void Quit()
        {
                Application.Quit();
        }

        public void DoCurrentAction()
        {
             onSelected.Raise(_displayOptions[_currentIndex].gameObject);
        }

        private void Update()
        {

            if ((Input.GetAxis("Vertical1") < .5 && Input.GetAxis("Vertical1") > -.5) && Input.GetAxis("Vertical2") > -.5 && (Input.GetAxis("Vertical2") < .5))
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
            if ((Input.GetAxis("Vertical1") < .5 && Input.GetAxis("Vertical1") > -.5) && Input.GetAxis("Vertical2") > -.5 && (Input.GetAxis("Vertical2") < .5))
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

