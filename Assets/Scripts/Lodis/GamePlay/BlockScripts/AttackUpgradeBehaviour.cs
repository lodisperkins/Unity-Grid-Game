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
        private string _nameOfItem;
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
        public string Name
        {
            get
            {
                return _nameOfItem;
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
            _nameOfItem = specialFeature.name;
			turretScript = GetComponent<GunBehaviour>();
            turretScript.OutOfAmmo.AddListener(_blockScript.DestroyBlock);
            turretScript.owner = _blockScript.owner.name;
            _blockHealth.health.Val = turretScript.CurrentAmmo;
		}
        /// <summary>
        /// Upgrades:
        /// Bullet damage increased
        /// Bullet speed increased
        /// Ammo capacity increased
        /// Fire rate increased
        /// </summary>
        /// <param name="otherBlock"></param>
		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _otherBlockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _otherBlockScript.componentList)
            {
                if (component.Name == Name)
                {
                    turretScript = component.specialFeature.GetComponent<GunBehaviour>();
					turretScript.damageVal += _damageUpgradeVal;
					turretScript.bulletForceScale += _bulletForceUpgradeVal;
					turretScript.bulletCount += _ammoUpgradeVal;
                    turretScript.bulletDelay -= .2f;
                    _otherBlockScript.HealthScript.health.Val = turretScript.bulletCount;
					return;
				}
			}
			TransferOwner(otherBlock);
		}
        /// <summary>
        /// - Sets health of other block to be current
        /// ammunition count
        /// 
        /// -  Adds the detroy block func for the
        /// other block as a listener for the out of anmmo event
        /// </summary>
        /// <param name="otherBlock"></param>
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
            _blockHealth = otherBlock.GetComponent<HealthBehaviour>();
            _blockHealth.health.Val = turretScript.bulletCount;
			turretScript.OutOfAmmo.AddListener(blockScript.DestroyBlock);
			blockScript.componentList.Add(this);
			transform.SetParent(otherBlock.transform,false);
		}
        //Used to decrease health everytime a shot is fired
		public void DecreaseHealth()
		{
			_blockHealth.health.Val--;
		}

        public void ResolveCollision(GameObject collision)
        {
            return;
        }
        //Turns off turret 
        public void ActivateDisplayMode()
        {
            gameObject.SetActive(false);
            turretScript.StopAllCoroutines();
            turretScript.enabled = false;
        }
    }
}
