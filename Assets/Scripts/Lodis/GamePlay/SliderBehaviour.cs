using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lodis
{
	public class SliderBehaviour : MonoBehaviour
    {
    
    	[SerializeField] private Slider _bar;
    
    	[SerializeField]
    	private HealthBehaviour hp;

        [SerializeField] private IntVariable value;

        public bool isHealthBar;
    	// Use this for initialization
    	void Start () {
    		
    	}
    	
    	// Update is called once per frame
    	void Update ()
    	{
	        if (isHealthBar)
	        {
		        _bar.value = hp.Health.Val;
	        }
	        else
	        {
		        _bar.value = value.Val;
	        }
    		
    	}
    }

}

