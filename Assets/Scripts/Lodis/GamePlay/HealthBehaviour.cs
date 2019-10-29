using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lodis
{
    public class HealthBehaviour : MonoBehaviour
    {
        [SerializeField]
        private IntVariable Health_Ref;
        public IntVariable Health;
        [SerializeField]
        private bool IsAlive;
        private GameObject TempGameObject;
        [SerializeField]
        UnityEvent OnDeath;

        [SerializeField] private ParticleSystem ps;
        // Use this for initialization
        public void Start()
        {
            Health.Val = Health_Ref.Val;
            TempGameObject = gameObject;
            IsAlive = true;
        }
        public void takeDamage(int damageVal)
        {
            Health.Val -= damageVal;
            if (Health.Val <= 0)
            {
                IsAlive = false;
            }
        }
        public void DestroyBlock(float time)
        {
            GameObject tempGameObject = gameObject;
            Destroy(tempGameObject,time);
        }
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
