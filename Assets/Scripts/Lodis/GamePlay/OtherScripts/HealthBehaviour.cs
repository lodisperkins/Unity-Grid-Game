using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Lodis
{
    public class HealthBehaviour : MonoBehaviour
    {
        //the reference of what the objects health should start as
        [FormerlySerializedAs("Health_Ref")] [SerializeField]
        private IntVariable healthRef;

        //the current health of the object
        [FormerlySerializedAs("Health")] public IntVariable health;
        //whether or not the object is alive
        [FormerlySerializedAs("IsAlive")] [SerializeField]
        private bool isAlive;

        [SerializeField] private HealthBehaviour _secondaryHealthSource;

        public int conversionRate;
        //unity event raised when the object dies
        [SerializeField]
        UnityEvent OnDeath;

        private bool hasRaised;
        [SerializeField] private Event OnObjectDeath;
        [SerializeField] private Event OnHit;
        //the particles that should play on the objects death
        [SerializeField] private ParticleSystem ps;
        // Use this for initialization
        public void Start()
        {
            health = IntVariable.CreateInstance(healthRef.Val);
            isAlive = true;
            hasRaised = false;
        }
        //decrements the objects health by the damge amount given
        public void takeDamage(int damageVal)
        {
            health.Val -= damageVal;
            OnHit.Raise();
            if (health.Val <= 0)
            {
                isAlive = false;
            }
        }
        //destroys the block within a given time
        public void DestroyBlock(float time)
        {
            GameObject tempGameObject = gameObject;
            Destroy(tempGameObject,time);
        }
        //plays particles after the object has died 
        public void playDeathParticleSystems(float duration)
        {
            var tempPs = Instantiate(ps,transform.position,transform.rotation);
            tempPs.Play();
            Destroy(tempPs, duration);
        }
        //(not used) Meant for the player to heal from some secondary health source like the core
        public void Heal()
        {
            if (_secondaryHealthSource == null || health.Val == healthRef.Val)
            {
                return;
            }
            _secondaryHealthSource.health.Val -= conversionRate;
            health.Val += 1;
        }
        // Update is called once per frame
        void Update()
        {
            if (isAlive == false)
            {
                OnDeath.Invoke();
                if (OnObjectDeath != null && hasRaised == false)
                {
                    hasRaised = true;
                    OnObjectDeath.Raise(gameObject);
                }
            }
        }
    }
}
