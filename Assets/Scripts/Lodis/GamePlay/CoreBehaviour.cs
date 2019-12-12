using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBehaviour : MonoBehaviour
{

	[SerializeField] private Material _coreMaterial;
	private float _time;
	private Color _materialColor;

	private bool _isFlashing;
	// Use this for initialization
	void Start ()
	{
		_materialColor = Color.white;
	}

	private void OnTriggerStay(Collider other)
	{
		StartCoroutine(Flash());
	}
	private IEnumerator Flash()
	{
		_isFlashing = false;
		for (var i = 0; i < 5; i++)
        {
            _coreMaterial.color =Color.yellow;
            yield return new WaitForSeconds(.1f);
            _coreMaterial.color =_materialColor;
            yield return new WaitForSeconds(.1f);
        }

		
		
		
	}
	// Update is called once per frame
	void Update ()
	{
	}
}
