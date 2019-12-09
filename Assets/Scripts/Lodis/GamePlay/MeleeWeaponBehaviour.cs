using System;
using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	[SerializeField] private ParticleSystem ps;
	[SerializeField] private int _damageVal;
	private void OnTriggerEnter(Collider other)
	{
		var health = other.GetComponent<HealthBehaviour>();
		if (health != null)
		{
			playDeathParticleSystems(1);
			health.takeDamage(_damageVal);
		}
		
	}
	public void playDeathParticleSystems(float duration)
	{
		var tempPs = Instantiate(ps,transform.position,transform.rotation);
		tempPs.playbackSpeed = 2.5f;
		tempPs.Play();
		Destroy(tempPs, duration);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
