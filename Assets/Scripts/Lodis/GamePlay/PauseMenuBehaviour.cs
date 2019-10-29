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
		[SerializeField]
		private Event OnPause;
		[SerializeField]
		private Event OnUnPause;

		private bool isPaused;
    	private void Update()
    	{
	        if (isPaused)
	        {
		        if (Input.GetButtonDown("Delete1") || Input.GetButtonDown("Delete1"))
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

