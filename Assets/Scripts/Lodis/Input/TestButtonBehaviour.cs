using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonBehaviour : MonoBehaviour {
   
	// Use this for initialization
	void Start () {
		
	}
	public void pause()
    {
        Time.timeScale = 0;
    }
    public void unpause()
    {
        Time.timeScale = 1;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
