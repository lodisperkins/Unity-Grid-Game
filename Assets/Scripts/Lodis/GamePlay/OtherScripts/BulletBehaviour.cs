using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay;
using Lodis.GamePlay.GridScripts;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Lodis
{
    public class BulletBehaviour : MonoBehaviour
    {
        
        //The player that shot this bullet
        public string Owner;
        //the amount of damage this bullet does 
        public int DamageVal;
        //Temporary gameobject used to delete the bullet without deleting the prefab
        protected GameObject TempObject;
        //the particle system to be played when a bullet hits an obstacle
        [SerializeField] protected GameObject ps;
        //Event used to play the sound of a bullet being shot
        [SerializeField] protected Event OnBulletSpawn;
        //The laser model attached to this bullet
        [SerializeField] protected Material _laserMatP1;
        [SerializeField] protected Material _laserMatP2;
        [SerializeField] protected GameObject _laser;
        [SerializeField] protected GameObject laserLight;
        public BlockBehaviour block;
        public Vector3 bulletForce;
        public bool reflected;
        public int lifetime;
        [SerializeField]
        protected Event onReflect;
        [SerializeField]
        protected Event onPanelSet;
        [SerializeField] protected GameObjectList _bulletListP1;
        [SerializeField] protected GameObjectList _bulletListP2;
        protected bool panelSetCalled;
        public bool active;
        public bool noColor;
        public bool destroyOnHit = true;
        protected PanelBehaviour _currentPanel;
        public GameObject hitTrail;
        public PanelBehaviour currentPanel
        {
            get { return _currentPanel; }
        }

        public GameObject Laser
        {
            get
            {
                return _laser;
            }

            set
            {
                _laser = value;
            }
        }

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
            if(noColor)
            {
                return;
            }
            if (Owner == "Player1")
            {
                Laser.GetComponent<MeshRenderer>().sharedMaterial = _laserMatP1;
                laserLight.GetComponent<MeshRenderer>().material.color = Color.red;
                _bulletListP1.Add(gameObject);
            }
            else
            {
                Laser.GetComponent<MeshRenderer>().sharedMaterial = _laserMatP2;
                laserLight.GetComponent<MeshRenderer>().material.color = Color.blue;
                _bulletListP2.Add(gameObject);
            }
        }
        
        private void Awake()
        {
            ChangeColor();
            OnBulletSpawn.Raise();
            panelSetCalled = false;
        }

        public void Reflect(int damageIncrease = 2, float speedScale = 1.5f)
        {
            GetComponent<Rigidbody>().velocity = -(GetComponent<Rigidbody>().velocity *= speedScale);
            ReverseOwner();
            reflected = true;
            lifetime = 2;
            DamageVal *= damageIncrease;
            onReflect.Raise();
        }
        public virtual void ResolveCollision(GameObject other)
        {
            switch (other.tag)
            {
                case "Player":
                {
                    if (other.name != Owner || reflected)
                    {
                        playDeathParticleSystems(1);
                        ps.transform.position = other.transform.position;
                        var health = other.GetComponent<HealthBehaviour>();
                        if (health != null)
                        {
                            health.takeDamage(DamageVal);
                        }
                            if (destroyOnHit)
                            {
                                Destroy(TempObject);
                            }
                        }
                        
                        break;
                }
                case "Core":
                {
                    playDeathParticleSystems(1);
                    ps.transform.position = other.transform.position;
                    var health = other.GetComponent<HealthBehaviour>();
                    if (health != null)
                    {
                        health.takeDamage(DamageVal);
                    }
                        if (destroyOnHit)
                        {
                            Destroy(TempObject);
                        }
                        break;
                }
                case "Panel":
                    {
                        _currentPanel = other.GetComponent<PanelBehaviour>();
                        if (panelSetCalled == false)
                        {
                            onPanelSet.Raise();
                            panelSetCalled = true;
                        }

                        break;
                    }
                case "Block":
                {
                    if(other.name == "DeletionBlock(Clone)")
                    {
                        return;
                    }
                    playDeathParticleSystems(1);
                    ps.transform.position = other.transform.position;
                    var health = other.GetComponent<HealthBehaviour>();
                    if (health != null)
                    {
                        other.GetComponent<BlockBehaviour>().GiveMoneyForKill(Owner,DamageVal);
                        health.takeDamage(DamageVal);
                        
                    }
                        if (destroyOnHit)
                        {
                            Destroy(TempObject);
                        }
                        break;
                }
                case "Projectile":
                    {
                        if(other.name == "Ramming Block(Clone)" && other.GetComponent<BlockBehaviour>().owner.name != Owner)
                        {
                            playDeathParticleSystems(1);
                            ps.transform.position = other.transform.position;
                            var health = other.GetComponent<HealthBehaviour>();
                            if (health != null)
                            {
                                other.GetComponent<BlockBehaviour>().GiveMoneyForKill(Owner, DamageVal);
                                health.takeDamage(DamageVal);
                            }

                            if (destroyOnHit)
                            {
                                Destroy(TempObject);
                            }
                        }
                        break;
                    }
                case "Barrier":
                    {
                        playDeathParticleSystems(1);
                        ps.transform.position = other.transform.position;
                        break;
                    }
                case "Wind":
                    {
                        break;
                    }
                default:
                    {
                        playDeathParticleSystems(1);
                        ps.transform.position = other.transform.position;
                        if (destroyOnHit)
                        {
                            Destroy(TempObject);
                        }
                        break;
                    }
            }
           
        }
        private void OnTriggerEnter(Collider other)
        {
            if (Owner == "")
            {
                Destroy();
                return;
            }
            if (DamageVal >= 5)
            {
                Vector3 direction = other.transform.position - transform.position;
                KnockBackBehaviour knockBackScript = other.gameObject.GetComponent<KnockBackBehaviour>();
                if (knockBackScript != null)
                {
                    knockBackScript.KnockBack(direction, 100, 1);
                }
                
            }
            ResolveCollision(other.gameObject);
            
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (Owner == "")
            {
                Destroy();
                return;
            }
            if (DamageVal >= 5)
            {
                Vector3 direction = collision.contacts[0].point - transform.position;
                collision.gameObject.GetComponent<KnockBackBehaviour>().KnockBack(direction, 100, 1);
            }
            ResolveCollision(collision.gameObject);
        }
        public void Destroy()
        {
            Destroy(TempObject);
        }
        //plays the particle system after a bullet hits an object
        public void playDeathParticleSystems(float duration)
        {
            var tempPs = Instantiate(ps,transform.position,transform.rotation);
            Destroy(tempPs,.5f);
        }
        // Update is called once per frame
        void Update()
        {
            if (Owner == "")
            {
                Destroy();
            }
        }

        private void OnDestroy()
        {
            _bulletListP1.RemoveItem(gameObject);
            _bulletListP2.RemoveItem(gameObject);
        }
    }
    
}
