using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis.GamePlay.BlockScripts
{
    public class RammingBlockBehaviour : MonoBehaviour {
        [SerializeField]
        private BlockBehaviour _block;
        [SerializeField]
        private Rigidbody _blockRigidbody;
        private Vector3 _ramForce;
        [SerializeField]
        private float _ramForceScale;
        [SerializeField] public int DamageVal;
        [SerializeField] public int UpgradeVal;
        private Quaternion playerRotation;
        public bool isRamming;
        private void Start()
        {
            _block.specialActions += Ram;
            _blockRigidbody.isKinematic = true;
           
        }
        private void OnCollisionEnter(Collision collision)
        {
            ResolveCollision(collision.gameObject);
        }

        public void ResolveCollision(GameObject other)
        {
            switch (other.tag)
            {
                case "Block":
                {
                    var health = other.GetComponent<HealthBehaviour>();
                        var block = other.GetComponent<BlockBehaviour>();
                    if (health != null && isRamming && block.canUpgrade)
                    {
                        block.GiveMoneyForKill(_block.owner.name, DamageVal);
                        health.takeDamage(DamageVal);
                        _block.DestroyBlock();
                    }
                    break;
                }
                default:
                {
                    var health = other.GetComponent<HealthBehaviour>();
                    if (health != null && other.name != _block.owner.name)
                    {
                        health.takeDamage(DamageVal);
                        _block.DestroyBlock();
                    }
                    break;
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
                    _blockScript.componentList[i].GetComponent<RammingBlockBehaviour>().DamageVal += UpgradeVal;
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        public void Ram(object[] args)
        {
            isRamming = true;
            transform.rotation = _block.owner.transform.rotation;
            transform.parent.tag = "Projectile";
            _blockRigidbody.isKinematic = false;
            _block.currentPanel.GetComponent<GridScripts.PanelBehaviour>().Occupied = false;
            _block.currentPanel.GetComponent<GridScripts.PanelBehaviour>().blockCounter = 0;
            _ramForce = transform.forward * _ramForceScale;
            _blockRigidbody.AddForce(_ramForce, ForceMode.Impulse);
        }
        public void TransferOwner(GameObject otherBlock)
        {
            _block = otherBlock.GetComponent<BlockBehaviour>();
            _block.componentList.Add(gameObject);
            _blockRigidbody = otherBlock.GetComponent<Rigidbody>();
            _blockRigidbody.isKinematic = true;
            _block.specialActions += Ram;
            transform.SetParent(otherBlock.transform,false);
        }
        // Update is called once per frame
        void Update () {
            _block.canUpgrade = !isRamming; 
	    }
    }
}

