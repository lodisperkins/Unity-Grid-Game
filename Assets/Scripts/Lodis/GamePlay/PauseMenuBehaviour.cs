using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

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
    	private void Update()
    	{
	        if (isPaused)
	        {
		        if (Input.GetButtonDown("Delete1") || Input.GetButtonDown("Delete2"))
		        {
			        Application.Quit();
		        }
	        }
	        
    		if (Input.GetButtonDown("Pause"))
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
    	}
    }

}

