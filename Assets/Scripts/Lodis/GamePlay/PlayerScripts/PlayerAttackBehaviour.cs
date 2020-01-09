using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{
	[SerializeField]
	private GunBehaviour _gun;
	private float _time;
	[SerializeField]
	private float _hitboxActiveTime;
	private bool _meleeHitboxActive;
	[SerializeField]
	private GameObject _weaponHitbox;

	private PlayerMovementBehaviour player;
	// Use this for initialization
	void Start ()
	{
		player = GetComponent<PlayerMovementBehaviour>();
	}

	public void MeleeAttack()
	{
		if (player.canMove)
		{
			_meleeHitboxActive = true;
			_time = Time.time + _hitboxActiveTime;
		}
		
	}
	
	public void FireGun()
	{
		if (player.canMove)
		{
			_gun.FireBullet();
		}
		
	}
	// Update is called once per frame
	void Update () {
		if (_meleeHitboxActive)
		{
			_weaponHitbox.SetActive(true);
			if (Time.time >=_time )
			{
				_weaponHitbox.SetActive(false);
				_meleeHitboxActive = false;
			}
		}
	}
}
