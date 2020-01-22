using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private Material _laserMatP1;
        [SerializeField] private Material _laserMatP2;
        [SerializeField] private GameObject laser;
        public BlockBehaviour block;
        public Vector3 bulletForce;
        public bool reflected;
        public int lifetime;
        [SerializeField]
        private Event onReflect;
        private void Start()
        {
            TempObject = gameObject;
            ChangeColor();
            lifetime = 1;
        }
        public void ReverseOwner()
        {
            if(Owner == "Player1")
            {
                Owner = "Player2";
            }
            else if (Owner == "Player2")
            {
                Owner = "Player1";
            }
        }
        //(not working) meant to change the bullets color based on the owner
        private void ChangeColor()
        {
            //if (Owner == "Player1")
            //{
            //    laser.GetComponent<MeshRenderer>().sharedMaterial = _laserMatP1;
            //}
            //else
            //{
            //    laser.GetComponent<MeshRenderer>().sharedMaterial = _laserMatP2;
            //}
        }
        
        private void Awake()
        {
            ChangeColor();
            OnBulletSpawn.Raise();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shield"))
            {
                GetComponent<Rigidbody>().velocity = -(GetComponent<Rigidbody>().velocity *= 1.5f);
                reflected = true;
                lifetime = 2;
                DamageVal += 1;
                onReflect.Raise();
                return;
            }
            else if (other.CompareTag("Projectile"))
            {
                if (Owner == other.GetComponent<BulletBehaviour>().Owner)
                {
                    return;
                }
            }
            else if(other.name == Owner && reflected)
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
            else if (other.name != Owner && other.CompareTag("Panel") == false)
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
            Destroy(TempObject, 8);
        }
    }
}
