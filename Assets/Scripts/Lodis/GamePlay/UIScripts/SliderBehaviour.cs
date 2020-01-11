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
        [SerializeField]
        private Color _overdriveColor;
        [SerializeField] private Color _defaultColor;
        private float lerpVal;
        [SerializeField] private IntVariable value;
        [SerializeField] private Image _healthbarImage;
        public bool isHealthBar;
    	// Use this for initialization
    	void Start ()
        {
	        lerpVal = 1;

        }

        public void LerpHealthColor()
        {
	        if (_healthbarImage != null)
	        {
		        lerpVal = _bar.value / 100;
		        _healthbarImage.color=Vector4.Lerp(Color.red,Color.yellow , lerpVal);
		        if (lerpVal == 1)
		        {
			        _healthbarImage.color = Color.green;
		        }
		        
	        }
				
        }

        public void ChangeToOverdriveColor()

        {
	        _healthbarImage.color = Color.yellow;
        }

        public void ChangeToDefaultColor()
        {
	        _healthbarImage.color = Color.cyan;
        }
        
    	void Update ()
    	{
	        if (isHealthBar)
	        {
		        _bar.value = hp.health.Val;
		        LerpHealthColor();
	        }
	        else
	        {
		        _bar.value = value.Val;
		        
	        }
    		
    	}
    }

}

