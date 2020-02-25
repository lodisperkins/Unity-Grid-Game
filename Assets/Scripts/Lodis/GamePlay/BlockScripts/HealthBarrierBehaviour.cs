using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class HealthBarrierBehaviour : MonoBehaviour,IUpgradable
	{
		[SerializeField] private HealthBehaviour _healthScript;
		[SerializeField] private BlockBehaviour _blockScript;
		[SerializeField] private int _upgradeVal;
		[SerializeField] private int _healVal;
		[SerializeField] private int _bulletsToHeal;
        [SerializeField]
        private GameEventListener _deleteEventListener;
        private float _healTimer;
        [SerializeField]
        private float _timeUntilNextHeal;
		private int _currentBulletsToHeal;
        private bool _canHeal;

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
            _canHeal = true;
            _healthScript = block.gameObject.GetComponent<HealthBehaviour>();
            _healTimer = Time.time + _timeUntilNextHeal;
        }
        public void DestroyBarrier()
        {
            GameObject temp = gameObject;
            Destroy(temp);
        }
		private void OnTriggerStay(Collider other)
		{
            if(!other.CompareTag("Block"))
            {
                return;
            } 
            
			if (other.name != "Repair Block(Clone)" && other.name != "Orbiter Block(Clone)")
			{
                _healthScript = other.GetComponent<HealthBehaviour>();
				TryHeal();
			}
		}
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                _healthScript.takeDamage(other.GetComponent<BulletBehaviour>().DamageVal);
                other.GetComponent<BulletBehaviour>().Destroy();
            }
            if ( other.name != _blockScript.owner.name && other.CompareTag("Block") && other.name != "Orbiter Block(Clone)")
            {
                _healthScript = other.GetComponent<HealthBehaviour>();
                _healthScript.health.Val += _healVal * 2;
            }
        }
        private void TryHeal()
		{
            if(_canHeal && !_healthScript.healthFull)
            {
                _healthScript.health.Val +=_healVal;
                _healTimer = Time.time + _timeUntilNextHeal;
            }
		}
		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
                    component.specialFeature.GetComponent<HealthBarrierBehaviour>()._healthScript.health.Val += _upgradeVal;
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
			blockScript.componentList.Add(this);
            transform.parent = null;
            transform.position = otherBlock.transform.position;
            _deleteEventListener.intendedSender = otherBlock;
        }
		// Update is called once per frame
		void Update () {
            _canHeal = Time.time >= _healTimer;
		}

        public void ResolveCollision(GameObject collision)
        {
            return; 
        }
    }
}
