using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lodis
{
    public class BlockBehaviour : MonoBehaviour
    {
        //The current panel the block is on
        [FormerlySerializedAs("CurrentPanel")] public GameObject currentPanel;
        //The player that owns the block
        [FormerlySerializedAs("Owner")] public GameObject owner;
        //the cost of materials to build this block
        public int cost;
        //The gun script attached to the bullet emitter
        GunBehaviour _gun;
        //he health behaviour script attached to this block
        HealthBehaviour _armor;
        //The material block behaviour script attached to this block
        EnergyBlockBehaviour _energyMine;
        //The script of the panel this block is currently on
        PanelBehaviour _panel;
        [SerializeField] private Text _level;
        private int _currentLevel;
        [SerializeField] private Material _sleepingMateral;
        [SerializeField]
        private Material _defaultMaterial;
        //The weight of a block represents how much of it can be placed on a panel. Panels havea limit of 3;
        public int BlockWeightVal;
        private bool _awake;
        private Material _currentMaterial;
        //If true, the player may upgrade this block, otherwise they must wait until it is
        public bool canUpgrade;
        public bool sleeping;
        [FormerlySerializedAs("OnUpgrade")] [SerializeField] private Event onUpgrade;
        [FormerlySerializedAs("OnBlockSpawn")] [SerializeField] private Event onBlockSpawn;
        // Use this for initialization
        void Start()
        {
            //initialzes local variables to be gameobjects components
            _gun = GetComponentInChildren<GunBehaviour>();
            _armor = GetComponent<HealthBehaviour>();
            _energyMine = GetComponent<EnergyBlockBehaviour>();
            _panel = currentPanel.GetComponent<PanelBehaviour>();
            _panel.blockCounter += BlockWeightVal;
            _currentMaterial = GetComponent<Renderer>().material;
            GetComponent<BlockBehaviour>().enabled = true;
            canUpgrade = false;
            _awake = true;
            _currentLevel = 1;
            if (_gun != null)
            {
                _gun.owner = owner.name;
            }
        }

        private void Awake()
        {
            //raises the event signaling the block has been spawned
            onBlockSpawn.Raise();
            _awake = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            //If the player cannot upgrade yet do nothing 
            if (!canUpgrade)
            {
                return;
            }
            //otherwise check the name of the block and upgrade
            if (other.name == "Attack Block(Clone)")
            {
                UpgradeAttack();
                _currentLevel++;
                onUpgrade.Raise();
            }
            else if (other.name == "Defense Block(Clone)")
            {
                UpgradeDefense();
                _currentLevel++;
                onUpgrade.Raise();
            }
            else if (other.name == "Material Block(Clone)")
            {
                UpgradeMaterial();
                _currentLevel++;
                onUpgrade.Raise();
            }
            //Destroys the block placed on top after the upgrade to free up space
            BlockBehaviour block;
            block = other.GetComponent<BlockBehaviour>();
            var destroyblock = other.GetComponent<DeletionBlockBehaviour>();
            if (block != null && destroyblock == null)
            {
                block.DestroyBlock();
                currentPanel.GetComponent<PanelBehaviour>().Occupied = true;
            }

        }
        public void enableUpgrades()
        {
            canUpgrade = true;
        }
        //Destroys this block instantly
        public void DestroyBlock()
        {
            _panel.Occupied = false;
            GameObject tempGameObject = gameObject;
            Destroy(tempGameObject);
        }
        //Not in game yet
        public void MakeBlockSleep(Vector3 parentPosition)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Renderer>().material = _sleepingMateral;
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            sleeping = true;
            transform.localScale-= new Vector3(.5f,.5f,.5f);
            transform.position = parentPosition+Vector3.up;
            if (name == "Attack Block(Clone)")
            {
                GetComponentInChildren<GunBehaviour>().enabled = false;
            }
            
            foreach (MonoBehaviour component in components)
            {
                if (component is BlockBehaviour)
                {
                    continue;
                }
                component.enabled = false;
            }

            _awake = false;
        }
        //not in game yet
        public void WakeBlock()
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            sleeping = false;
            transform.localScale+= new Vector3(.5f,.5f,.5f);
            if (name == "Attack Block(Clone)")
            {
                GetComponentInChildren<GunBehaviour>().enabled = true;
            }
            
            foreach (MonoBehaviour component in components)
            {
                if (component is BlockBehaviour)
                {
                    continue;
                }
                component.enabled = true;
            }

            _awake = true;
        }
        
        //destroys this block after a specified time
        public void DestroyBlock(float time)
        {
            _panel.Occupied = false;
            _panel.blockCounter = 0;
            GameObject TempGameObject = gameObject;
            Destroy(TempGameObject,time);
        }
        //increases attack power and bullet count
        public void UpgradeAttack()
        {
            _gun.enabled = true;
            _gun.bulletCount += 5;
            _gun.damageVal += 1;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, .5f, 1);
        }
        //Increases health value 
        public void UpgradeDefense()
        {
            if (_armor != null)
            {
                _armor.enabled = true;
                _armor.health.Val += 20;
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
        //increases the materials gained 
        public void UpgradeMaterial()
        {
            if (_energyMine != null)
            {
                _energyMine.enabled = true;
                GetComponent<RoutineBehaviour>().actionDelay -= .5f;
                _energyMine.MaterialAmount +=3;
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, .2f, 0f);
            }
        }


        private void Update()
        {
            if (_level != null)
            {
                _level.text = "lvl. "+_currentLevel;
            }
            
        }
    }
}