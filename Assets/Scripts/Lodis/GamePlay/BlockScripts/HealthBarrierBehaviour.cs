using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class HealthBarrierBehaviour : MonoBehaviour
	{
		[SerializeField] private HealthBehaviour _healthScript;
		[SerializeField] private BlockBehaviour _block;
		[SerializeField] private int _upgradeVal;
		[SerializeField] private int _healVal;
		[SerializeField] private int _bulletsToHeal;
        private float _healTimer;
        [SerializeField]
        private float _timeUntilNextHeal;
		private int _currentBulletsToHeal;
        private bool _canHeal;
		// Use this for initialization
		void Start ()
		{
            _canHeal = true;
            _healTimer = Time.time + _timeUntilNextHeal;
        }

		private void OnTriggerStay(Collider other)
		{
            _healthScript = other.GetComponent<HealthBehaviour>();
			if (_healthScript != null)
			{
				TryHeal();
			}
		}

		private void TryHeal()
		{
            if(_canHeal)
            {
                _healthScript.health.Val +=_healVal;
                _healTimer = Time.time + _timeUntilNextHeal;
            }
		}
		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
			for (int i = 0; i < _blockScript.componentList.Count; i++)
			{
				if (_blockScript.componentList[i].name == gameObject.name)
				{
					_blockScript.componentList[i].GetComponent<HealthBarrierBehaviour>()._healthScript.health.Val += _upgradeVal;
					return;
				}
			}
			TransferOwner(otherBlock);
		}
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
			_healthScript = otherBlock.GetComponent<HealthBehaviour>();
			_healthScript.health.Val += _upgradeVal;
			blockScript.componentList.Add(gameObject);
            transform.parent = null;
            transform.position = otherBlock.transform.position;
		}
		// Update is called once per frame
		void Update () {
            _canHeal = Time.time >= Time.time + _timeUntilNextHeal;
		}
	}
}
