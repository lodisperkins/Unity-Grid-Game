using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Lodis
{
    public class GunBehaviour : MonoBehaviour
    {
        //the bullet the gun will be firing
        [FormerlySerializedAs("Bullet")] [SerializeField]
        private GameObject bullet;
        //the player the owns this gun
        [FormerlySerializedAs("Owner")] public string owner;
        //temporary gameobject used to modify the bullets transform
        private GameObject _tempBullet;
        //the amount the bullet force is scale up by
        [FormerlySerializedAs("Bullet_Force_Scale")] [SerializeField]
        public int bulletForceScale;
        //the total amount of force used to move the bullet
        public Vector3 _bulletForce;
        //the amount of time between firing bullets
        [FormerlySerializedAs("Bullet_Delay")] [SerializeField]
        private int bulletDelay;
        //the amount of bullet to fire
        [FormerlySerializedAs("Bullet_Count")] public int bulletCount;
        //unity event raised when the gun is out of ammo
        [FormerlySerializedAs("OutOfAmmo")] [SerializeField]
        private UnityEvent outOfAmmo;
        //temporary transform for the bullet
        private Transform _tempTransform;
        //temporary rigidbody for the bullet
        private Rigidbody _tempRigidBody;
        //If this gun is turret it fires automatically, otherwise it waits for player input tp fire
        [SerializeField] private bool _isTurret;
        [SerializeField] private Event _onShotFired;
        // the amount of damge the bullet should deal
        [FormerlySerializedAs("DamageVal")] public int damageVal;
        [SerializeField]
        public BlockBehaviour block;
        // Use this for initialization
        void Start()
        {
            if (_isTurret)
            {
                StartCoroutine(Fire());
            }
        }
        //fires a bullet with a specified interval of time
        private IEnumerator Fire()
        {
            for (int i = 0; i < bulletCount; i++)
            {
                FireBullet();
                yield return new WaitForSeconds(bulletDelay);
            }
            outOfAmmo.Invoke();
        }

        public void FireBullet(Vector3 position)
        {
            _tempBullet = Instantiate(bullet, position, transform.rotation);
            _tempBullet.GetComponent<BulletBehaviour>().Owner = owner;
            _tempBullet.GetComponent<BulletBehaviour>().DamageVal = damageVal;
            _tempBullet.transform.Rotate(new Vector3(90, 0));
            _tempBullet.GetComponent<BulletBehaviour>().block = block;
            _tempRigidBody = _tempBullet.GetComponent<Rigidbody>();
            _bulletForce = transform.forward * bulletForceScale;
            _tempBullet.GetComponent<BulletBehaviour>().bulletForce = _bulletForce;
            _tempRigidBody.AddForce(_bulletForce);
            _onShotFired.Raise(gameObject);
        }
        public void FireBullet()
        {
            _tempBullet = Instantiate(bullet, transform.position, transform.rotation);
            _tempBullet.GetComponent<BulletBehaviour>().Owner = owner;
            _tempBullet.GetComponent<BulletBehaviour>().DamageVal = damageVal;
            _tempBullet.transform.Rotate(new Vector3(90, 0));
            _tempRigidBody = _tempBullet.GetComponent<Rigidbody>();
            _bulletForce = transform.forward * bulletForceScale;
            _tempRigidBody.AddForce(_bulletForce);
            _onShotFired.Raise(gameObject);
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
