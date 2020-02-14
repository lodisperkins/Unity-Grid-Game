using System.Collections.Generic;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{


    public class KineticBlockBehaviour : MonoBehaviour {
        private List<Rigidbody> _rigidbodies;
        private List<BulletBehaviour> _bullets;
        private List<Vector3> velocityVals;
        [SerializeField] private HealthBehaviour _blockHealth = new HealthBehaviour();
        [SerializeField]
        private BlockBehaviour block;
        [SerializeField]
        private GameEventListener _eventListener;
        public int bulletCapacity;

        [SerializeField] private int _bulletCapUpgradeVal;
	    // Use this for initialization
	    void Start() 
        {
            _rigidbodies = new List<Rigidbody>();
            _bullets = new List<BulletBehaviour>();
            velocityVals = new List<Vector3>();
            _eventListener.intendedSender = block.owner;
            _blockHealth.health.Val = bulletCapacity;
            block.specialActions += DetonateBlock;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
               
                BulletBehaviour bulletscript = other.GetComponent<BulletBehaviour>();
                if(bulletscript != null)
                {
                    _bullets.Add(bulletscript);
                    other.gameObject.SetActive(false);
                }
                
                Rigidbody temp = other.GetComponent<Rigidbody>();
                
                velocityVals.Add(temp.velocity);
                _rigidbodies.Add(temp);
                temp.velocity = Vector3.zero;
                temp.isKinematic = true;
            }
        }
        public void DetonateBlock(object[]arg)
        {
            block.DestroyBlock(0);
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i] != null)
                {
                    _bullets[i].Owner = block.owner.name;
                    _bullets[i].gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i] != null)
                {
                    _rigidbodies[i].isKinematic = false;
                    _rigidbodies[i].gameObject.SetActive(true);
                    _rigidbodies[i].AddForce(-(velocityVals[i]) *2, ForceMode.Impulse); 
                }
            }
        }
        public void UpgradeBlock(GameObject otherBlock)
        {
            BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            for (int i = 0; i < _blockScript.componentList.Count; i++)
            {
                if (_blockScript.componentList[i].name == gameObject.name)
                {
                    _blockScript.componentList[i].GetComponent<KineticBlockBehaviour>().bulletCapacity+= _bulletCapUpgradeVal;
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        public void TransferOwner(GameObject otherBlock)
        {
            block = otherBlock.GetComponent<BlockBehaviour>();
            block.componentList.Add(gameObject);
            block.specialActions += DetonateBlock;
            transform.SetParent(otherBlock.transform,false);
        }
        // Update is called once per frame
        void Update() {
            if (_rigidbodies.Count >= bulletCapacity)
            {
                block.DestroyBlock(1);
                foreach (BulletBehaviour bullet in _bullets)
                {
                    bullet.Destroy();
                }
            }
            if(_blockHealth != null)
            {
              _blockHealth.health.Val = bulletCapacity-_rigidbodies.Count; 
            }
            
        }
    }
}
