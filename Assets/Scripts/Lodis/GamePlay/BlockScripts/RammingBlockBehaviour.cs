using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis.GamePlay.BlockScripts
{
    public class RammingBlockBehaviour : MonoBehaviour,IUpgradable {
        [SerializeField]
        private BlockBehaviour _blockScript;
        [SerializeField]
        private Rigidbody _blockRigidbody;
        private Vector3 _ramForce;
        [SerializeField]
        private float _ramForceScale;
        [SerializeField] public int DamageVal;
        [SerializeField] public int UpgradeVal;
        private Quaternion playerRotation;
        public bool isRamming;
        private BulletBehaviour _projectileScript;
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
        private void Start()
        {
            _blockScript.specialActions += Ram;
            _blockRigidbody.isKinematic = true;
            _projectileScript = GetComponent<BulletBehaviour>();
        }
        public void InitializeProjectileScript()
        {
            _projectileScript.enabled = true;
            _projectileScript.DamageVal = DamageVal;
            _projectileScript.Owner = _blockScript.owner.name;
            _projectileScript.lifetime = 5;
        }
        public void UpgradeBlock(GameObject otherBlock)
        {
            BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
                    component.specialFeature.GetComponent<RammingBlockBehaviour>().DamageVal += UpgradeVal;
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        public void Ram(object[] args)
        {
            isRamming = true;
            InitializeProjectileScript();
            transform.rotation = _blockScript.owner.transform.rotation;
            transform.tag = "Projectile";
            _blockRigidbody.isKinematic = false;
            _blockScript.currentPanel.GetComponent<GridScripts.PanelBehaviour>().Occupied = false;
            _blockScript.currentPanel.GetComponent<GridScripts.PanelBehaviour>().blockCounter = 0;
            OtherScripts.ScreenShakeBehaviour shakeScript = _blockScript.GetComponent<OtherScripts.ScreenShakeBehaviour>();
            if(shakeScript != null)
            {
                shakeScript.enabled = false;
            }
            _ramForce = transform.forward * _ramForceScale;
            _blockRigidbody.AddForce(_ramForce, ForceMode.Impulse);
        }
        public void TransferOwner(GameObject otherBlock)
        {
            _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            _blockScript.componentList.Add(this);
            _blockRigidbody = otherBlock.GetComponent<Rigidbody>();
            _blockRigidbody.isKinematic = true;
            _blockScript.specialActions += Ram;
            transform.SetParent(otherBlock.transform,false);
            GetComponent<GameEventListener>().intendedSender = otherBlock;
            GetComponent<BulletBehaviour>().Laser = otherBlock;
        }
        public void RemoveOtherSpecialActions()
        {
            block.specialActions = null;
            block.specialActions += Ram;
        }
        void IUpgradable.ResolveCollision(GameObject collision)
        {
            if (_blockScript.canUpgrade)
            {
                return;
            }
            _projectileScript.ResolveCollision(collision.gameObject);
        }
        private void OnDestroy()
        {
            GameObject temp = block.gameObject;
            Destroy(temp);
        }
    }
}

