using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lodis
{
    public class GunBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject Bullet;
        public string Owner;
        private GameObject Temp_Bullet;
        [SerializeField]
        private int Bullet_Force_Scale;
        private Vector3 Bullet_Force;
        [SerializeField]
        private int Bullet_Delay;
        public int Bullet_Count;
        [SerializeField]
        private UnityEvent OutOfAmmo;
        private Transform Temp_Transform;
        private Rigidbody Temp_RigidBody;
        public int DamageVal;
        // Use this for initialization
        void Start()
        {
            StartCoroutine(Fire());
            
        }
        private IEnumerator Fire()
        {
            for (int i = 0; i < Bullet_Count; i++)
            {
                Temp_Bullet = Instantiate(Bullet, transform.position, transform.rotation);
                Temp_Bullet.GetComponent<BulletBehaviour>().Owner = Owner;
                Temp_Bullet.GetComponent<BulletBehaviour>().DamageVal = DamageVal;
                Temp_Bullet.transform.Rotate(new Vector3(90, 0));
                Temp_RigidBody = Temp_Bullet.GetComponent<Rigidbody>();
                Bullet_Force = transform.forward * Bullet_Force_Scale;
                Temp_RigidBody.AddForce(Bullet_Force);
                yield return new WaitForSeconds(Bullet_Delay);
            }
            OutOfAmmo.Invoke();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
