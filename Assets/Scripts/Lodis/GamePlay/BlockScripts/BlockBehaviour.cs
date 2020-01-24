using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay;
using Lodis.GamePlay.GridScripts;
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
        public bool deleting;
        public bool sleeping;
        [FormerlySerializedAs("OnUpgrade")] [SerializeField] private Event onUpgrade;
        [FormerlySerializedAs("OnBlockSpawn")] [SerializeField] private Event onBlockSpawn;
        private HealthBehaviour _health;
        private Color _currentMaterialColor;
        [SerializeField]
        private GameObject shield;
        [SerializeField]
        private SphereCollider shieldCollider;
        [SerializeField]
        private GameObject overdriveParticles;
        private string pastName;
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
            _currentMaterial.SetColor("_EmissionColor",Color.black);
            _health = GetComponent<HealthBehaviour>();
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
            if (other.CompareTag("Player"))
            {
                DestroyBlock();
                return;
            }
            //otherwise check the name of the block and upgrade
            Upgrade(other.gameObject);
        }

        public void Upgrade(GameObject block)
        {
            //If the player cannot upgrade yet do nothing 
            if (!canUpgrade)
            {
                return;
            }
            switch (block.name)
            {
                case "Attack Block(Clone)":
                {
                    UpgradeAttack();
                    _currentLevel++;
                    onUpgrade.Raise();
                    break;
                }
                case "Defense Block(Clone)":
                {
                    UpgradeDefense();
                    _currentLevel++;
                    onUpgrade.Raise();
                    break;
                }
                case "Material Block(Clone)":
                {
                    UpgradeMaterial();
                    _currentLevel++;
                    onUpgrade.Raise();
                    break;
                }
                default:
                    return;
            }
            //Destroys the block placed on top after the upgrade to free up space
            BlockBehaviour blockScript;
            blockScript = block.GetComponent<BlockBehaviour>();
            var destroyblock = block.GetComponent<DeletionBlockBehaviour>();
            if (blockScript != null && destroyblock == null)
            {
                blockScript.DestroyBlock();
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
            if (canUpgrade && _health != null)
            {
                _panel.Occupied = false;
                _health.playDeathParticleSystems(2);
            }
            else if(!canUpgrade&& _health != null)
            {
                _health.hasRaised = true;
            }
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
        public bool CheckForSameUpgradeBlock(string name)
        {
            if(pastName == null || pastName != name)
            {
                Debug.Log("past is " + pastName);
                Debug.Log("current is " + name);
                pastName = name;
                return false;
            }
            return true;
        }
        private IEnumerator Flash()
        {
            for (var i = 0; i < 5; i++)
            {
                _currentMaterial.SetColor("_EmissionColor",new Color(.7f,.5f,0.1f,1));
                yield return new WaitForSeconds(.1f);
                _currentMaterial.SetColor("_EmissionColor",Color.black); 
                yield return new WaitForSeconds(.1f);
            }
        }
        //destroys this block after a specified time
        public void DestroyBlock(float time)
        {
            _panel.blockCounter = 0;
            if (canUpgrade && _health != null)
            {
                _panel.Occupied = false;
                _health.playDeathParticleSystems(2);
            }
            else if(!canUpgrade&& _health != null)
            {
                _health.hasRaised = true;
            }
            canUpgrade = false;
            GameObject TempGameObject = gameObject;
            
            
            StartCoroutine(Flash());
            Destroy(TempGameObject,time);
        }
        //increases attack power and bullet count
        public void UpgradeAttack()
        {
            _gun.enabled = true;
            _gun.bulletCount += 5;
            _gun.damageVal += 1;
        }
        //Increases health value 
        public void UpgradeDefense()
        {
            if (_armor != null)
            {
                shieldCollider.enabled = true;
                shield.SetActive(true);
                _armor.enabled = true;
                _armor.health.Val += 10;
            }
        }

        //increases the materials gained 
        public void UpgradeMaterial()
        {
            if (_energyMine != null)
            {
                overdriveParticles.SetActive(true);
                _energyMine.enabled = true;
                GetComponent<RoutineBehaviour>().actionDelay -= .5f;
                _energyMine.MaterialAmount +=3;
            }
        }


        private void Update()
        {
            if (_level != null)
            {
                _level.text = "lvl. "+_currentLevel;
            }
            if (_currentLevel >= 3)
            {
                _level.text = "MAX";
            }
        }
    }
}