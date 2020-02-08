using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShakeBehaviour : MonoBehaviour {
public float shakeVal;

	public bool isShaking;
	private Vector3 _startPosition;
    [SerializeField]
    private float shakeRange;

    [SerializeField] private int shakeLength;
	private void Start()
	{
		_startPosition = transform.position;
	}

	IEnumerator Shake()
	{
		if (shakeLength == 0)
		{
			shakeLength = 5;
		}
		isShaking = false;
		for(int i = 0; i< shakeLength; i++)
		{
			var newPosition = new Vector3( Random.Range(-shakeRange, shakeRange),Random.Range(-shakeRange, shakeRange),0);
			transform.position += newPosition;
			yield return new WaitForSeconds(shakeVal);
		}

		transform.localPosition = _startPosition;
	}

	public void StartShaking()
	{
		isShaking = true;
	}
	public void StartShaking(int length)
	{
		shakeLength = length;
		isShaking = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isShaking)
		{
			StartCoroutine(Shake());
		}
	}
}
