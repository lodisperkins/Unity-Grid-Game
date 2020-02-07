using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class AttackUpgradeBehaviour : MonoBehaviour {

		[SerializeField] private GunBehaviour turretScript;
		[SerializeField] private BlockBehaviour _blockScript;
		[SerializeField] private int _damageUpgradeVal;
		[SerializeField] private int _bulletForceUpgradeVal;
		[SerializeField] private int _ammoUpgradeVal;
		[SerializeField] private HealthBehaviour _blockHealth;
		// Use this for initialization
		void Start ()
		{
			turretScript = GetComponent<GunBehaviour>();
            turretScript.OutOfAmmo.AddListener(_blockScript.DestroyBlock);
            turretScript.owner = _blockScript.owner.name;
            _blockHealth.health.Val = turretScript.CurrentAmmo;
		}

		private void Awake()
		{
			
		}

		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
			for (int i = 0; i < _blockScript.componentList.Count; i++)
			{
				if (_blockScript.componentList[i].name == gameObject.name)
				{
					turretScript = _blockScript.componentList[i].GetComponent<GunBehaviour>();
					turretScript.damageVal += _damageUpgradeVal;
					turretScript.bulletForceScale += _bulletForceUpgradeVal;
					turretScript.bulletCount += _ammoUpgradeVal;
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
			turretScript.OutOfAmmo.AddListener(blockScript.DestroyBlock);
			blockScript.componentList.Add(gameObject);
			transform.SetParent(otherBlock.transform,false);
		}
		// Update is called once per frame
		void Update ()
		{
			
		}
	}
}
