using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

namespace Lodis
{
    public class BulletBehaviour : MonoBehaviour
    {
        //The player that shot this bullet
        public string Owner;
        //the amount of damage this bullet does 
        public int DamageVal;
        //Temporary gameobject used to delete the bullet without deleting the prefab
        private GameObject TempObject;
        //the particle system to be played when a bullet hits an obstacle
        [SerializeField] private ParticleSystem ps;
        //Event used to play the sound of a bullet being shot
        [SerializeField] private Event OnBulletSpawn;
        //The laser model attached to this bullet
        [SerializeField] private VolumetricLineBehavior _laser;
        private void Start()
        {
            TempObject = gameObject;
            
            ChangeColor();
        }
        //(not working) meant to change the bullets color based on the owner
        private void ChangeColor()
        {
            if (Owner == "Player1")
            {
                _laser.LineColor = Color.red;
            }
            else
            {
                _laser.LineColor = Color.blue;
            }
        }
        
        private void Awake()
        {
            ChangeColor();
            OnBulletSpawn.Raise();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name != Owner && other.CompareTag("Panel") == false)
            {
                playDeathParticleSystems(1);
                ps.transform.position = other.transform.position;
                var health = other.GetComponent<HealthBehaviour>();
                if (health != null)
                {
                    health.takeDamage(DamageVal);
                }
                Destroy(TempObject);
            }
        }
        //plays the particle system after a bullet hits an object
        public void playDeathParticleSystems(float duration)
        {
            var tempPs = Instantiate(ps,transform.position,transform.rotation);
            tempPs.playbackSpeed = 2.5f;
            tempPs.Play();
            Destroy(tempPs, duration);
        }
        // Update is called once per frame
        void Update()
        {
            Destroy(TempObject, 1);
        }
    }
}
