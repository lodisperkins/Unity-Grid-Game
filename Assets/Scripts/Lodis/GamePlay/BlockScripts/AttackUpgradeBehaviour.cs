using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class AttackUpgradeBehaviour : MonoBehaviour,IUpgradable {

		[SerializeField] private GunBehaviour turretScript;
		[SerializeField] private BlockBehaviour _blockScript;
		[SerializeField] private int _damageUpgradeVal;
		[SerializeField] private int _bulletForceUpgradeVal;
		[SerializeField] private int _ammoUpgradeVal;
		[SerializeField] private HealthBehaviour _blockHealth;
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
		// Use this for initialization
		void Start ()
		{
			turretScript = GetComponent<GunBehaviour>();
            turretScript.OutOfAmmo.AddListener(_blockScript.DestroyBlock);
            turretScript.owner = _blockScript.owner.name;
            _blockHealth.health.Val = turretScript.CurrentAmmo;
		}

		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
					turretScript = component.specialFeature.GetComponent<GunBehaviour>();
					turretScript.damageVal += _damageUpgradeVal;
					turretScript.bulletForceScale += _bulletForceUpgradeVal;
					turretScript.bulletCount += _ammoUpgradeVal;
                    component.specialFeature.GetComponent<AttackUpgradeBehaviour>()._blockHealth.health.Val = turretScript.bulletCount;
                    turretScript.bulletDelay -= .2f;
					return;
				}
			}
			TransferOwner(otherBlock);
		}
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
            _blockHealth = otherBlock.GetComponent<HealthBehaviour>();
            _blockHealth.health.Val = turretScript.bulletCount;
			turretScript.OutOfAmmo.AddListener(blockScript.DestroyBlock);
			blockScript.componentList.Add(this);
			transform.SetParent(otherBlock.transform,false);
		}

		public void DecreaseHealth()
		{
			_blockHealth.health.Val--;
		}

        public void ResolveCollision(GameObject collision)
        {
            return;
        }
    }
}
