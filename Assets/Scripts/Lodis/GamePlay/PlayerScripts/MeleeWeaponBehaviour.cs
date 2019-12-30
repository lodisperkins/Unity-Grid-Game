using System;
using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour {

	[SerializeField] private ParticleSystem ps;
	[SerializeField] private int _damageVal;
	private void OnTriggerEnter(Collider other)
	{
		var health = other.GetComponent<HealthBehaviour>();
		if (health != null)
		{
			PlayParticleSystems(1);
			health.takeDamage(_damageVal);
		}
		
	}
	//Plays particle effect when hit
	public void PlayParticleSystems(float duration)
	{
		var tempPs = Instantiate(ps,transform.position,transform.rotation);
		tempPs.playbackSpeed = 2.5f;
		tempPs.Play();
		Destroy(tempPs, duration);
	}
}
