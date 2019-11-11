using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace Lodis.Gameplay
{
	public class TeleportBeamBehaviour : MonoBehaviour {
		[FormerlySerializedAs("Target")] [SerializeField]
		public string target;

		[SerializeField] private GameObject player; 
		public Transform playerTransform;
		private Vector3 _beamdestination;
		[SerializeField] private float _beamHeight;
		[SerializeField]
		private Vector3 velocity;
		[SerializeField]
		private int max_speed;
		[SerializeField]
		private Event _onAscending;
		[SerializeField]
		private Event _onLanding ;
		public bool ascending;
		private Rigidbody _body;
		// Use this for initialization
		void Start()
		{
			_body = GetComponent<Rigidbody>();
			playerTransform = playerTransform.transform;
			ascending = false;
		}
		void MoveBeamUpwards()
		{
			_onAscending.Raise();
			_beamdestination = new Vector3(playerTransform.position.x,_beamHeight,playerTransform.position.z);
			Vector3 seekforce = _beamdestination - transform.position;
			seekforce = (seekforce.normalized*max_speed) - velocity;
			if (velocity.magnitude > max_speed)
			{
				velocity=velocity.normalized*max_speed;
			}
			transform.position += seekforce*Time.deltaTime;
			if (transform.position.y >= _beamdestination.y)
			{
				player.SendMessage("resetPositionToCurrentPanel");
				@ascending = false;
			}
			
		}
		void MoveBeamDownwards()
		{
			Vector3 seekforce = playerTransform.position- transform.position;
			seekforce = (seekforce.normalized*max_speed) - velocity;
			if (velocity.magnitude > max_speed)
			{
				velocity=velocity.normalized*max_speed;
			}
			transform.position += seekforce*Time.deltaTime;
			if (transform.position.y < playerTransform.position.y && @ascending == false) 
			{
				_onLanding.Raise();
			}
			
		}

		public void EnableAscending()
		{
			@ascending = true;
		}
		
		// Update is called once per frame
		
		void Update()
		{
			if (ascending)
			{
				MoveBeamUpwards();
			}
			else
			{
				MoveBeamDownwards();
				
			}
		}
	}
}
