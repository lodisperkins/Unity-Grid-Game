using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay;
using Lodis.GamePlay.BlockScripts;
using Lodis.GamePlay.GridScripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lodis
{
    
    public class BlockBehaviour : MonoBehaviour
    {
        public delegate void BlockAction(object[] arg = null);
        //The current panel the block is on
        [FormerlySerializedAs("CurrentPanel")] public GameObject currentPanel;
        //The player that owns the block
        [FormerlySerializedAs("Owner")] public GameObject owner;
        //the cost of materials to build this block
        public int cost;
        //The script of the panel this block is currently on
        PanelBehaviour _panel;
        [SerializeField] private Text _level;
        private int _currentLevel;
        //The weight of a block represents how much of it can be placed on a panel. Panels havea limit of 3;
        public int BlockWeightVal;
        private bool _awake;
        private Material _currentMaterial;
        public GameObject actionComponent;
        //If true, the player may upgrade this block, otherwise they must wait until it is
        public bool canUpgrade;
        public bool deleting;
        [FormerlySerializedAs("OnUpgrade")] [SerializeField] private Event onUpgrade;
        [FormerlySerializedAs("OnBlockSpawn")] [SerializeField] private Event onBlockSpawn;
        [SerializeField] private Event onBlockDelete;
        private HealthBehaviour _health;
        public List<GameObject> componentList;
        private Color _currentMaterialColor;
        private string pastName;
        public BlockAction specialActions;
        [SerializeField] private IntVariable player1Materials;
        [SerializeField] private IntVariable player2Materials;
        // Use this for initialization
        void Start()
        {
            _panel = currentPanel.GetComponent<PanelBehaviour>();
            _panel.blockCounter += BlockWeightVal;
            _currentMaterial = GetComponent<Renderer>().material;
            componentList = new List<GameObject>();
            componentList.Add(actionComponent);
            GetComponent<BlockBehaviour>().enabled = true;
            _currentMaterial.SetColor("_EmissionColor",Color.black);
            _health = GetComponent<HealthBehaviour>();
            canUpgrade = false;
            _awake = true;
            _currentLevel = 1;
        }

        private void Awake()
        {
            //raises the event signaling the block has been spawned
            onBlockSpawn.Raise();
            _awake = true;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (canUpgrade == false)
            {
                if (gameObject.name == "Ramming Block(Clone)")
                {
                    if (gameObject.GetComponentInChildren<RammingBlockBehaviour>().isRamming)
                    {
                        CheckIfRammingBlock(collision.gameObject);
                        return;
                    }
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Player"))
            {
                DestroyBlock();
                return;
            }
            else if (other.CompareTag("Block"))
            {
                if (gameObject.name == "Ramming Block(Clone)")
                {
                    RammingBlockBehaviour ramScript = GetComponentInChildren<RammingBlockBehaviour>();
                    if(ramScript == null)
                    {
                        return;
                    }
                    if (ramScript.isRamming)
                    {
                        
                        CheckIfRammingBlock(other.gameObject);
                        return;
                    }
                }
                //otherwise check the name of the block and upgrade
                Upgrade(other.gameObject);
            }
            
        }
        public bool CheckIfRammingBlock(GameObject other)
        {
            foreach (GameObject component in componentList)
            {
                RammingBlockBehaviour ram = component.GetComponent<RammingBlockBehaviour>();
                if (ram != null)
                {
                    ram.ResolveCollision(other);
                    return true;
                }
            }
            return false;
        }
        public void Upgrade(GameObject block)
        {
            //If the player cannot upgrade yet do nothing 
            if (!canUpgrade)
            {
                return;
            }
            GameObject component = block.GetComponent<BlockBehaviour>().actionComponent;
            component.SendMessage("UpgradeBlock",gameObject);
            _currentLevel++;
            onUpgrade.Raise(gameObject);
            //Destroys the block placed on top after the upgrade to free up space
            BlockBehaviour blockScript;
            blockScript = block.GetComponent<BlockBehaviour>();
            var destroyblock = block.GetComponent<DeletionBlockBehaviour>();
            if (blockScript != null && destroyblock == null)
            {
                blockScript.DestroyBlock();
                currentPanel.GetComponent<PanelBehaviour>().Occupied = true;
            }
            canUpgrade = false;
            GetComponent<RoutineBehaviour>().ResetActions();
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

        private void OnDestroy()
        {
            if (onBlockDelete != null)
            {
                 onBlockDelete.Raise(gameObject);
            }
        }

        public void ActivateSpecialAction()
        {
            specialActions.Invoke();
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

        public void GiveMoneyForKill(string shooterName,int damageVal)
        {
            if (_health.health.Val - damageVal <= 0)
            {
                if (shooterName == "Player1")
                {
                    player1Materials.Val += cost / 2;
                }
                else if (shooterName == "Player2")
                {
                    player2Materials.Val += cost / 2;
                }
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