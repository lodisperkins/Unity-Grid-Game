using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverdriveToggleBehaviour : MonoBehaviour
{

	[SerializeField] private Image _toggleOnImage;
	[SerializeField] private Image _toggleOffImage;

	[SerializeField] private OverdriveBehaviour _enabled;
	// Use this for initialization
	void Start ()
	{
		_toggleOnImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_enabled.used)
		{
			_toggleOnImage.enabled = false;
			_toggleOffImage.enabled = true;
		}
	}
}
