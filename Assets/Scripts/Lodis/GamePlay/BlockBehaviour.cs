using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class BlockBehaviour : MonoBehaviour
    {
        public GameObject CurrentPanel;
        public GameObject Owner;
        public int cost;
        GunBehaviour Gun;
        HealthBehaviour Armor;
        MaterialBlockBehaviour MaterialMine;
        PanelBehaviour panel;
        public bool canUpgrade;

        [SerializeField] private Event OnUpgrade;
        [SerializeField] private Event OnBlockSpawn;
        // Use this for initialization
        void Start()
        {
            Gun = GetComponentInChildren<GunBehaviour>();
            Armor = GetComponent<HealthBehaviour>();
            MaterialMine = GetComponent<MaterialBlockBehaviour>();
            panel = CurrentPanel.GetComponent<PanelBehaviour>();
            canUpgrade = false;
            if (Gun != null)
            {
                Gun.Owner = Owner.name;
            }
        }

        private void Awake()
        {
            OnBlockSpawn.Raise();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!canUpgrade)
            {
                return;
            }
            if (other.name == "Attack Block(Clone)")
            {
                UpgradeAttack();
                OnUpgrade.Raise();
            }
            else if (other.name == "Defense Block(Clone)")
            {
                UpgradeDefense();
                OnUpgrade.Raise();
            }
            else if (other.name == "Material Block(Clone)")
            {
                UpgradeMaterial();
                OnUpgrade.Raise();
            }
            BlockBehaviour block;
            block = other.GetComponent<BlockBehaviour>();
            var destroyblock = other.GetComponent<DeletionBlockBehaviour>();
            if (block != null && destroyblock == null)
            {
                block.DestroyBlock();
                CurrentPanel.GetComponent<PanelBehaviour>().Occupied = true;
            }

        }
        public void enableUpgrades()
        {
            canUpgrade = true;
        }
        public void DestroyBlock()
        {
            panel.Occupied = false;
            GameObject tempGameObject = gameObject;
            Destroy(tempGameObject);
        }
        public void DestroyBlock(float time)
        {
            panel.Occupied = false;
            GameObject TempGameObject = gameObject;
            Destroy(TempGameObject,time);
        }
        public void UpgradeAttack()
        {
            Gun.enabled = true;
            Gun.DamageVal += 1;
            Gun.Bullet_Count += 15;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, .5f, 1);
        }
        public void UpgradeDefense()
        {
            if (Armor != null)
            {
                Armor.enabled = true;
                Armor.Health.Val += 5;
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
        public void UpgradeMaterial()
        {
            if (MaterialMine != null)
            {
                MaterialMine.enabled = true;
                MaterialMine.MaterialAmount += 10;
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, .2f, 0f);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}