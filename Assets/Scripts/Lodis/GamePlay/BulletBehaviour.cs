using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class BulletBehaviour : MonoBehaviour
    {
        public string Owner;
        public int DamageVal;
        private GameObject TempObject;
        [SerializeField]
        private GameObject _laser;
        
        [SerializeField] private ParticleSystem ps;
        [SerializeField] private Event OnBulletSpawn;
        private void Start()
        {
            
            TempObject = gameObject;
            ChangeColor();
        }
        
        private void ChangeColor()
        {
            if (Owner == "Player1")
            {
                _laser.GetComponent<Light>().color = Color.red;
            }
            else
            {
                _laser.GetComponent<Light>().color = Color.blue;
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
