using System;
using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;

namespace Lodis.GamePlay
{
	public class RobotSpecialBehaviour : MonoBehaviour
    {
    	private Rigidbody RobotBody;
        public Event OnSpecialAbilityActivated;
        public Event OnSpecialAbilityDeactivated;
    	public float tackleForce;
    	// Use this for initialization
    	void Start ()
    	{
    		RobotBody = GetComponent<Rigidbody>();
    	}
        
    	public void RollAttack()
    	{
	        RobotBody.AddForce(0,0,tackleForce);
	        SendMessage("EnableMoveAnimation");
	        OnSpecialAbilityActivated.Raise(gameObject);
    	}
    
    	public void RollRecoil()
    	{
    		RobotBody.AddForce(0,0,-tackleForce/2);
    	}
    
    	private void OnTriggerEnter(Collider other)
    	{
    		var health = other.GetComponent<HealthBehaviour>();
    		if (other.CompareTag("Projectile"))
    		{
    			health.DestroyBlock(2);
    		}
    		else if (other.CompareTag("Player"))
    		{
    			health.takeDamage(20);
                OnSpecialAbilityDeactivated.Raise(gameObject);
    		}
            
    	}
    
    	private void OnCollisionEnter(Collision other)
    	{
    		if (other.gameObject.CompareTag("Player"))
    		{
    			SendMessage("resetPositionToCurrentPanel");
                SendMessage("DisableMoveAnimation");
    		}
    		
    	}
    	// Update is called once per frame
    	
    	void Update () {
    		
    	}
    }

}

