using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;
using UnityEngine.Serialization;

public class OverdriveBehaviour : MonoBehaviour
{

	[SerializeField] private PlayerSpawnBehaviour _player;

	[SerializeField] private HealthBehaviour _health;
	private float _time;
	[SerializeField]
	private float _overdriveLength;
	[SerializeField] private ParticleSystem ps;
	[SerializeField] private GameObject _particles;

	[FormerlySerializedAs("_used")] public bool used;
	// Use this for initialization
	void Start ()
	{
		used = false;
	}
	public void PlayParticleSystems(float duration)
	{
		var tempPs = Instantiate(ps,transform.position,transform.rotation);
		tempPs.playbackSpeed = 2.5f;
		tempPs.Play();
		Destroy(tempPs, duration);
	}
	public void StopMaterialLoss()
	{
		if ( used)
		{
			return;
		}
		_player.overdriveEnabled = true;
		_player.AddMaterials(60);
		
		if (_health.health.Val >= 75)
		{
			_overdriveLength = 10;
		}
		else if(_health.health.Val >= 25 && _health.health.Val < 75)
		{
			_overdriveLength = 20;
		}
		else
		{
			_overdriveLength = 30;
		}
		_particles.SetActive(true);
		_time = Time.time + _overdriveLength;
	}
	
	// Update is called once per frame
	void Update () {
		if (_player.overdriveEnabled)
		{
			used = true;
			if (Time.time >=_time )
			{
				_player.overdriveEnabled = false;
				_particles.SetActive(false);
				
			}
		}
	}
}
