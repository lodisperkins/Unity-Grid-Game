using System.Collections.Generic;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{

    /// <summary>
    /// rework
    /// </summary>
    public class KineticBlockBehaviour : MonoBehaviour,IUpgradable {
        private List<Rigidbody> _rigidbodies;
        private List<BulletBehaviour> _bullets;
        private List<Vector3> velocityVals;
        [SerializeField] private HealthBehaviour _blockHealth;
        [SerializeField]
        private BlockBehaviour _blockScript;
        [SerializeField]
        private GameEventListener _eventListener;
        public int bulletCapacity;

        [SerializeField] private int _bulletCapUpgradeVal;
        public BlockBehaviour block
        {
            get
            {
                return _blockScript;
            }
            set
            {
                _blockScript = value;
            }
        }
        public GameObject specialFeature
        {
            get
            {
                return gameObject;
            }
        }

        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        // Use this for initialization
        void Start() 
        {
            _rigidbodies = new List<Rigidbody>();
            _bullets = new List<BulletBehaviour>();
            velocityVals = new List<Vector3>();
            _eventListener.intendedSender = _blockScript.owner;
            _blockHealth.health.Val = bulletCapacity;
            _blockScript.specialActions += DetonateBlock;
        }
        //Destroys block and fires all absorbed projectiles
        public void DetonateBlock(object[]arg)
        {
            _blockScript.DestroyBlock(0);
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i] != null)
                {
                    _bullets[i].Owner = _blockScript.owner.name;
                    //_bullets[i].DamageVal *= 2;
                }
            }
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i] != null)
                {
                    _rigidbodies[i].GetComponent<Transform>().parent = null;
                    _rigidbodies[i].GetComponent<Collider>().enabled = true;
                    _rigidbodies[i].isKinematic = false;
                    _rigidbodies[i].AddForce(-(velocityVals[i]) *2, ForceMode.Impulse); 
                }
            }
        }
        /// <summary>
        /// Upgrades:
        /// Bullet Capacity Increased
        /// </summary>
        /// <param name="otherBlock"></param>
        public void UpgradeBlock(GameObject otherBlock)
        {
            BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
                    component.specialFeature.GetComponent<KineticBlockBehaviour>().bulletCapacity+= _bulletCapUpgradeVal;
                    component.specialFeature.GetComponent<KineticBlockBehaviour>()._blockHealth.health.Val+=_bulletCapUpgradeVal;
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        public void TransferOwner(GameObject otherBlock)
        {
            _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            _blockScript.componentList.Add(this);
            _blockScript.specialActions += DetonateBlock;
            _blockHealth = otherBlock.GetComponent<HealthBehaviour>();
            transform.SetParent(otherBlock.transform,false);
        }
        // Update is called once per frame
        void Update() {
        }
        //This needs to be done here. Resolve collision func is called twice
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                BulletBehaviour bulletscript = other.GetComponent<BulletBehaviour>();
                /// <summary>
                /// Since a ramming block has its barrier as a separate collider,
                /// there must be a separate condition to get its parent added to 
                /// the list instead of it. The other condition should work for
                /// every other projectile.
                /// </summary>
                /// <param name="other"></param>
                if (other.name == "Ramming Barrier")
                {
                    Rigidbody parentRigidbody = other.GetComponentInParent<Rigidbody>();
                    Collider parentCollider = parentRigidbody.GetComponentInParent<Collider>();
                    if (parentRigidbody != null)
                    {
                        //disables ramming barrier
                        other.GetComponent<Collider>().enabled= false;
                        //disables parent
                        parentCollider.enabled = false;
                        parentCollider.transform.SetParent(transform, false);
                        parentCollider.transform.position = transform.position;
                        parentCollider.transform.localScale /= 2;
                        //Adds parent to list
                        velocityVals.Add(parentRigidbody.velocity);
                        _rigidbodies.Add(parentRigidbody);
                        parentRigidbody.velocity = Vector3.zero;
                        parentRigidbody.isKinematic = true;
                        
                    }
                    _bullets.Add(bulletscript);
                    _blockHealth.health.Val -= bulletscript.DamageVal;
                    return;
                }
                //Condition for normal projectiles
                if (bulletscript != null)
                {
                    _bullets.Add(bulletscript);
                    bulletscript.hitTrail.SetActive(false);
                    other.GetComponent<Collider>().enabled = false;
                    other.transform.localScale /= 2;
                    other.transform.SetParent(transform, false);
                    other.transform.position = transform.position;
                    _blockHealth.health.Val-= bulletscript.DamageVal;
                }
                
                Rigidbody temp = other.GetComponent<Rigidbody>();
                if (temp != null)
                {
                    velocityVals.Add(temp.velocity);
                    _rigidbodies.Add(temp);
                    temp.velocity = Vector3.zero;
                    temp.isKinematic = true;
                }

            }
        }
        public void ResolveCollision(GameObject other)
        {
            return;
        }

        public void ActivateDisplayMode()
        {
            return;
        }
    }
}
