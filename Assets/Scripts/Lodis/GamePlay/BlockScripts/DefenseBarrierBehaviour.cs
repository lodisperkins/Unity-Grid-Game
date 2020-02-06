using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class DefenseBarrierBehaviour : MonoBehaviour
	{
		[SerializeField] private HealthBehaviour healthScript;

		[SerializeField] private int _upgradeVal;
		// Use this for initialization
		void Start () {
		
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Projectile"))
			{
				healthScript.takeDamage(other.GetComponent<BulletBehaviour>().DamageVal);
			}
		}

		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
			for (int i = 0; i < _blockScript.componentList.Count; i++)
			{
				if (_blockScript.componentList[i].name == gameObject.name)
				{
					_blockScript.componentList[i].GetComponent<DefenseBarrierBehaviour>().healthScript.health.Val += _upgradeVal;
					return;
				}
			}
			TransferOwner(otherBlock);
		}
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
			healthScript = otherBlock.GetComponent<HealthBehaviour>();
			healthScript.health.Val += _upgradeVal;
			blockScript.componentList.Add(gameObject);
			transform.SetParent(otherBlock.transform,false);
		}
		// Update is called once per frame
		void Update () {
		
		}
	}
}
