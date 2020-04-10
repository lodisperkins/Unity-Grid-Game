using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay;
using Lodis.GamePlay.BlockScripts;
using Lodis.GamePlay.GridScripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Events;
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
        private PanelBehaviour _panel;
        [SerializeField] private Text _level;
        private int _currentLevel;
        //The weight of a block represents how much of it can be placed on a panel. Panels have a limit of 3;
        public int BlockWeightVal;
        public IUpgradable actionComponent;
        //If true, the player may upgrade this block, otherwise they must wait until it is
        public bool canUpgrade;
        public bool deleting;
        [FormerlySerializedAs("OnUpgrade")] [SerializeField] private Event onUpgrade;
        [FormerlySerializedAs("OnBlockSpawn")] [SerializeField] private Event onBlockSpawn;
        [SerializeField] private Event onBlockDelete;
        private HealthBehaviour _health;
        public List<IUpgradable> componentList;
        private Color _currentMaterialColor;
        public BlockAction specialActions;
        public MonoBehaviour specialFeature;
        //The energy for both players used to give a boost when this block is destroyed
        [SerializeField] private IntVariable player1Materials;
        [SerializeField] private IntVariable player2Materials;
        [SerializeField] private Canvas _blockUI;
        public GamePlay.OtherScripts.ScreenShakeBehaviour shakeScript;
        [SerializeField] private UnityEvent displayModeActions;
        [SerializeField] private string _type;
        public List<string> types;
        public HealthBehaviour HealthScript
        {
            get
            {
                return _health;
            }
        }

        public PanelBehaviour Panel
        {
            get
            {
                return currentPanel.GetComponent<PanelBehaviour>();
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }
        private void Start()
        {
            InitializeBlock();
        }
        // Use this for initialization
        void Awake()
        {
            //raises the event signaling the block has been spawned
            onBlockSpawn.Raise();
        }
        //sets all values to default
        public void InitializeBlock()
        {
            if(owner.name =="Player1")
            {
                BlackBoard.p1Blocks.Add(this);
            }
            else
            {
                BlackBoard.p2Blocks.Add(this);
            }
            types.Add(_type);
            actionComponent = specialFeature as IUpgradable;
            _panel = currentPanel.GetComponent<PanelBehaviour>();
            _panel.blockCounter += BlockWeightVal;
            componentList = new List<IUpgradable>();
            componentList.Add(actionComponent);
            _health = GetComponent<HealthBehaviour>();
            shakeScript = GetComponent<GamePlay.OtherScripts.ScreenShakeBehaviour>();
            canUpgrade = true;
            _currentLevel = 1;
        }
        //Turns off UI and disablkes any special components attached
        public void ActivateDisplayMode()
        {
            enabled = false;
            displayModeActions.Invoke();
            _blockUI = GetComponentInChildren<Canvas>();
            _blockUI.enabled = false;
            if (actionComponent != null)
            {
                actionComponent.ActivateDisplayMode();
            }
            return;
        }
        private void OnTriggerEnter(Collider other)
        {
            //Check if the player teleported onto a block
            if (other.name == "TeleportationBeam")
            {
                DestroyBlock();
                return;
            }
            //Upgrade check
            else if (other.CompareTag("Block"))
            {
                Upgrade(other.GetComponent<BlockBehaviour>());
            }
            else if (other.CompareTag("Panel"))
            {
                int oldWeight = Panel.blockCounter;
                currentPanel = other.gameObject;
                if(oldWeight != 0)
                {
                    Panel.blockCounter = oldWeight;
                }
            }
            //Tells all components of the block that collision has occured
            foreach (IUpgradable component in componentList)
            {
                if(component != null)
                {
                    component.ResolveCollision(other.gameObject);
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            //Tells all components of the block that collision has occured
            foreach (IUpgradable component in componentList)
            {
                if (component != null)
                {
                    component.ResolveCollision(collision.gameObject);
                }
            }
        }
        //Upgrades the block that this block was placed upon
        public void Upgrade(BlockBehaviour block)
        {
            //If the block cannot upgrade other blocks do nothing 
            if (!block.canUpgrade || block.actionComponent == null)
            {
                return;
            }
            block.actionComponent.UpgradeBlock(gameObject);
            _currentLevel++;
            onUpgrade.Raise(gameObject);
            types.Add(block._type);
            //Destroys the block placed on top after the upgrade to free up space
            block.GetComponent<BlockBehaviour>();
            var destroyblock = block.GetComponent<DeletionBlockBehaviour>();
            if (block != null && destroyblock == null)
            {
                block.DestroyBlock();
                block.canUpgrade = false;
                currentPanel.GetComponent<PanelBehaviour>().Occupied = true;
            }
        }
        public void DisableUpgrades()
        {
            canUpgrade = false;
        }
        //Destroys this block instantly
        public void DestroyBlock()
        {

            if (canUpgrade == false && HealthScript != null)
            {
                HealthScript.playDeathParticleSystems(2);
            }
            else if (!canUpgrade && HealthScript != null)
            {
                HealthScript.hasRaised = true;
            }
            canUpgrade = false;
            GameObject TempGameObject = gameObject;
            Destroy(TempGameObject);
        }

        private void OnDestroy()
        {
            if (onBlockDelete != null)
            {
                 onBlockDelete.Raise(gameObject);
            }
            BlackBoard.p1Blocks.Remove(this);
            BlackBoard.p2Blocks.Remove(this);
        }

        public void ActivateSpecialAction()
        {
            specialActions.Invoke();
        }
        
        //destroys this block after a specified time
        public void DestroyBlock(float time)
        {
            if(Panel != null)
            {
                Panel.blockCounter = 0;
            }
            
            if (canUpgrade == false && HealthScript != null)
            {
                HealthScript.playDeathParticleSystems(2);
            }
            else if(!canUpgrade&& HealthScript != null)
            {
                HealthScript.hasRaised = true;
            }
            canUpgrade = false;
            GameObject TempGameObject = gameObject;
            Destroy(TempGameObject,time);
        }
        //Gives the player a slight energy boost for destroying this block
        public void GiveMoneyForKill(string shooterName,int damageVal)
        {
            if (HealthScript.health.Val - damageVal <= 0)
            {
                if (shooterName == "Player1" && shooterName != owner.name)
                {
                    player1Materials.Val += cost / 2;
                }
                else if (shooterName == "Player2" && shooterName != owner.name)
                {
                    player2Materials.Val += cost / 2;
                }
            }
            
        }

        private void Update()
        {
            //Updates the ui to reflect the blocks current level
            if (_level != null)
            {
                _level.text = "lvl. "+_currentLevel;
            }
            if (_currentLevel == 3)
            {
                _level.text = "MAX";
            }
            else if (_currentLevel > 3)
            {
                _level.text = "OverLVL";
            }
        }
    }
}