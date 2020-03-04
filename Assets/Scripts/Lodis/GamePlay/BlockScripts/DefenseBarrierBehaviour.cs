using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class DefenseBarrierBehaviour : MonoBehaviour,IUpgradable
	{
		[SerializeField] private HealthBehaviour healthScript;
		private float _decayVal;
		[SerializeField] private int _upgradeVal;
		private string colorName;
		private Material _attachedMaterial;
        [SerializeField]
        private BlockBehaviour _blockScript;
		[SerializeField] private RoutineBehaviour shieldTimer;

		private float lerpNum;
		// Use this for initialization
		void Start ()
		{
			_decayVal = (float)1/shieldTimer.actionLimit;
			lerpNum = (float)1/shieldTimer.actionLimit;
			colorName = "Color_262603E3";
			_attachedMaterial = GetComponent<MeshRenderer>().material;
		}
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
        private void OnTriggerEnter(Collider other)
        {
            ResolveCollision(other.gameObject);
        }
        public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
                    component.specialFeature.GetComponent<DefenseBarrierBehaviour>().Upgrade();
					return;
				}
			}
			TransferOwner(otherBlock);
		}

		public void Upgrade()
		{
			gameObject.SetActive(true);
			healthScript.health.Val += _upgradeVal;
			shieldTimer.actionLimit += 10;
			shieldTimer.ResetActions();
			_decayVal = (float)1/shieldTimer.actionLimit;
			lerpNum = (float)1/shieldTimer.actionLimit;
			_attachedMaterial.SetColor(colorName,Color.Lerp(Color.green, Color.red,lerpNum));
		}
		public void DestroyBarrier()
		{
			healthScript.health.Val = 5;
			gameObject.SetActive(false);
		}
		public void DecaySphere()
		{
			_attachedMaterial.SetColor(colorName,Color.Lerp(Color.green, Color.red,lerpNum));
			lerpNum += _decayVal;
		}
		public void TransferOwner(GameObject otherBlock)
		{
			BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
			healthScript = otherBlock.GetComponent<HealthBehaviour>();
			healthScript.health.Val += _upgradeVal;
			blockScript.componentList.Add(this);
			transform.SetParent(otherBlock.transform,false);
		}
		// Update is called once per frame
		void Update () {
			
		}

        public void ResolveCollision(GameObject collision)
        {
            if (collision.CompareTag("Projectile"))
            {
                block.Health.takeDamage(collision.GetComponent<BulletBehaviour>().DamageVal);
                collision.GetComponent<BulletBehaviour>().Destroy();
            }
        }
    }
}
