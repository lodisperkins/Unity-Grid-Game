using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class HealthBarrierBehaviour : MonoBehaviour
	{
		[SerializeField] private HealthBehaviour _blockhealthScript;
		[SerializeField] private HealthBehaviour _playerhealthScript;
		[SerializeField] private BlockBehaviour _block;
		[SerializeField] private int _upgradeVal;
		[SerializeField] private int _healVal;
		[SerializeField] private int _bulletsToHeal;

		private int _currentBulletsToHeal;
		// Use this for initialization
		void Start ()
		{
			_playerhealthScript = _block.owner.GetComponent<HealthBehaviour>();
			_currentBulletsToHeal = _bulletsToHeal;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Projectile"))
			{
				TryHeal();
				_blockhealthScript.takeDamage(other.GetComponent<BulletBehaviour>().DamageVal);
			}
		}

		private void TryHeal()
		{
			_currentBulletsToHeal -= 1;
			if (_currentBulletsToHeal <= 0)
			{
				_currentBulletsToHeal = _bulletsToHeal;
				_playerhealthScript.health.Val +=_healVal;
			}
		}
		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
			for (int i = 0; i < _blockScript.componentList.Count; i++)
			{
				if (_blockScript.componentList[i].name == gameObject.name)
				{
					_blockScript.componentList[i].GetComponent<HealthBarrierBehaviour>()._blockhealthScript.health.Val += _upgradeVal;
					_blockScript.componentList[i].GetComponent<HealthBarrierBehaviour>()._bulletsToHeal -= 1;
					_blockScript.componentList[i].GetComponent<HealthBarrierBehaviour>()._healVal += 1;
					return;
				}
			}
			TransferOwner(otherBlock);
		}
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
			_blockhealthScript = otherBlock.GetComponent<HealthBehaviour>();
			_blockhealthScript.health.Val += _upgradeVal;
			blockScript.componentList.Add(gameObject);
			transform.SetParent(otherBlock.transform,false);
		}
		// Update is called once per frame
		void Update () {
		
		}
	}
}
