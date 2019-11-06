using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lodis
{
    public class HealthBehaviour : MonoBehaviour
    {
        //the reference of what the objects health should start as
        [SerializeField]
        private IntVariable Health_Ref;

        public int DisplayHealth;
        //the current health of the object
        public IntVariable Health;
        //whether or not the object is alive
        [SerializeField]
        private bool IsAlive;
        //unity event raised when the object dies
        [SerializeField]
        UnityEvent OnDeath;
        //the particles that should play on the objects death
        [SerializeField] private ParticleSystem ps;
        // Use this for initialization
        public void Start()
        {
            Health = IntVariable.CreateInstance(Health_Ref.Val);
            IsAlive = true;
        }
        //decrements the objects health by the damge amount given
        public void takeDamage(int damageVal)
        {
            Health.Val -= damageVal;
            if (Health.Val <= 0)
            {
                IsAlive = false;
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
        
        // Update is called once per frame
        void Update()
        {
            if (IsAlive == false)
            {
                OnDeath.Invoke();
            }
        }
    }
}
