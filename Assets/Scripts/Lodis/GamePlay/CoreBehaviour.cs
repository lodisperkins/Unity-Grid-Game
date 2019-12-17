using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBehaviour : MonoBehaviour
{

	[SerializeField] private Material _coreMaterial;
	private Color _materialColor;

	// Use this for initialization
	void Start ()
	{
		_coreMaterial.color = Color.white;
		_materialColor = Color.white;
	}

	private void OnTriggerStay(Collider other)
	{
		StartCoroutine(Flash());
	}
	//This makes the core flash for a few seconds when hit
	private IEnumerator Flash()
	{
		for (var i = 0; i < 5; i++)
        {
            _coreMaterial.color =Color.yellow;
            yield return new WaitForSeconds(.1f);
            _coreMaterial.color =_materialColor;
            yield return new WaitForSeconds(.1f);
        }
	}
}
