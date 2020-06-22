using System;
using Lodis.GamePlay.GridScripts;
using UnityEngine;
using VariableScripts;
using System.Collections.Generic;
using System.Collections;
namespace Lodis.GamePlay.AIFolder
{
	public class AIControllerBehaviour : MonoBehaviour
	{
		private BinaryTreeBehaviour _decisionTree;
		private AIMovementBehaviour _moveScript;
        private AISpawnBehaviour _spawnScript;
        private Condition CoreDefenseCheckDelegate;
        private Condition OpponentGridCheck;
        [SerializeField]
        private float _coreScore;
        [SerializeField]
        private float _supportScore;
        [SerializeField]
        private float _supportBlockScore;
        [SerializeField]
        private float _supportDefenseScore;
        [SerializeField]
        private float _blockHealthScore;
        [SerializeField]
        private float _blockAmountScore;
        [SerializeField]
        private float _totalScore;
        private List<int> yCoords;
        private bool isAttacking;
        private List<string> _priorityTypes;
        private RaycastHit _interactionRay;
        private PlayerAttackBehaviour _attackScript;
        [SerializeField]
        private float _shootDelay;
        [SerializeField]
        private HealthBehaviour healthScript;
        [SerializeField]
        private float supportScorePass;
        [SerializeField]
        private float coreScorePass;
        [SerializeField]
        private float supportDefScorePass;
        [SerializeField]
        private float gridScorePass;
        private void Start()
		{
			_decisionTree = GetComponent<BinaryTreeBehaviour>();
			_moveScript = GetComponent<AIMovementBehaviour>();
            _spawnScript = GetComponent<AISpawnBehaviour>();
            _attackScript = GetComponent<PlayerAttackBehaviour>();
            CoreDefenseCheckDelegate += GetPanelsOnBackRow;
            yCoords = new List<int>() { 0, 1, 2, 3 };
            StartCoroutine(Shoot());
        }
        public void InitializeLists()
        {
            //I can use this to initialize all list values instead of checking for which player in each func
        }
        private float ResourceStatusCheck()
        {
            //Initiialize values
            List<BlockBehaviour> characterBlockList = new List<BlockBehaviour>();
            List<Vector2> defensePositions = new List<Vector2>();
            PanelList characterPanelList = new PanelList();
            float totalDefenseBlocks = 0;
            float totalSupportBlocks = 0;
            //Check player tag
            if (name == "Player1")
            {
                characterBlockList = BlackBoard.p1Blocks;
                characterPanelList = BlackBoard.p1PanelList;
                defensePositions.Add(new Vector2(1,0));
                defensePositions.Add(new Vector2(2,0));
                defensePositions.Add(new Vector2(1,1));
                defensePositions.Add(new Vector2(2,1));
            }
            else
            {
                characterBlockList = BlackBoard.p2Blocks;
                characterPanelList = BlackBoard.p2PanelList;
                defensePositions.Add(new Vector2(8, 0));
                defensePositions.Add(new Vector2(7, 0));
                defensePositions.Add(new Vector2(7, 1));
                defensePositions.Add(new Vector2(8, 1));
            }
            //Check amount of support blocks
            foreach (BlockBehaviour block in characterBlockList)
            {
                foreach(string type in block.types)
                {
                    if (type == "Support")
                    {
                        totalSupportBlocks++;
                    }
                }
                
            }
            //check if there are defense blocks in front of support blocks
            foreach(var panelLocation in defensePositions)
            {
                PanelBehaviour currentPanel;
                if(characterPanelList == null)
                {
                    break;
                }
                if(characterPanelList.FindPanel(panelLocation,out currentPanel))
                {
                    if(currentPanel.CurrentBlock != null)
                    {
                        if(currentPanel.CurrentBlock.Type == "Defense")
                        {
                            totalDefenseBlocks++;
                        }
                    }
                }
            }
            _supportDefenseScore = (totalDefenseBlocks / 2f) * 100f;
            _supportBlockScore = (totalSupportBlocks / 2f) * 100f;
            if(_supportBlockScore == 0)
            {
                return 0;
            }
            return (_supportDefenseScore + _supportBlockScore) / 2f;
        }
        private bool GetPanelsOnBackRow(object[] args)
        {
            GameObject panelObject = (GameObject)args[0];
            PanelBehaviour panelScript = panelObject.GetComponent<PanelBehaviour>();
            if(name == "Player1")
            {
                if(panelScript.Position.x == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(name == "Player2")
            {
                if (panelScript.Position.x == 9)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        private bool GetPriorityPanels(object[] args)
        {
            GameObject panelObject = (GameObject)args[0];
            PanelBehaviour panelScript = panelObject.GetComponent<PanelBehaviour>();
            foreach(string type in _priorityTypes)
            {
                if (panelScript.CurrentBlock.types.Contains(type))
                {
                    return true;
                }
            }
            return false;
        }
        private float CoreDefenseCheck()
        {
            List<PanelBehaviour> panelsOnBack = new List<PanelBehaviour>();
            PanelList characterPanels = null;
            float CoreDefenseVal = 0f;
            if (name == "Player1")
            {
                characterPanels = BlackBoard.p1PanelList;
            }
            else
            {
                characterPanels = BlackBoard.p2PanelList;
            }
            if (characterPanels != null)
            {
                characterPanels.GetPanels(CoreDefenseCheckDelegate, out panelsOnBack);
            }

            foreach (var panel in panelsOnBack)
            {
                if (panel.Occupied)
                {
                    CoreDefenseVal++;
                }
            }
            return (CoreDefenseVal / 4f) * 100;
        }
        private void BuildCoreDefense()
        {
            List<PanelBehaviour> panelsOnBack = new List<PanelBehaviour>();
            PanelList characterPanels = null;
            if(name == "Player1")
            {
                characterPanels = BlackBoard.p1PanelList;
            }
            else
            {
                characterPanels = BlackBoard.p2PanelList;
            }
            if(characterPanels != null)
            {
                characterPanels.GetPanels(CoreDefenseCheckDelegate, out panelsOnBack);
            }
            
            foreach(var panel in panelsOnBack)
            {
                if(!panel.Occupied)
                {
                    _spawnScript.Build(1, panel);
                }
            }
        }
        private void BuildSupportDefense()
        {
            List<Vector2> defensePositions = new List<Vector2>();
            PanelList characterPanelList = new PanelList();
            if (name == "Player1")
            {
                characterPanelList = BlackBoard.p1PanelList;
                defensePositions.Add(new Vector2(2, 0));
                defensePositions.Add(new Vector2(2, 1));
            }
            else
            {
                characterPanelList = BlackBoard.p2PanelList;
                defensePositions.Add(new Vector2(7, 0));
                defensePositions.Add(new Vector2(7, 1));
            }
            PanelBehaviour buildPanel;
            int buildCounter =0;
            foreach(var position in defensePositions)
            {
                if (characterPanelList != null)
                {
                    if (characterPanelList.FindPanel(position, out buildPanel) && buildCounter < 2)
                    {
                        if (_spawnScript.Build(1, buildPanel))
                        {
                            buildCounter++;
                        }
                    }
                }
            }
        }
        private void BuildSupport()
        {
            List<Vector2> defensePositions = new List<Vector2>();
            PanelList characterPanelList = new PanelList();
            if (name == "Player1")
            {
                characterPanelList = BlackBoard.p1PanelList;
                defensePositions.Add(new Vector2(0, 1));
                defensePositions.Add(new Vector2(0, 0));
            }
            else
            {
                characterPanelList = BlackBoard.p2PanelList;
                defensePositions.Add(new Vector2(9, 0));
                defensePositions.Add(new Vector2(9, 1));
            }
            PanelBehaviour buildPanel;
            foreach (var position in defensePositions)
            {
                if (characterPanelList != null)
                {
                    if (characterPanelList.FindPanel(position, out buildPanel))
                    {
                        _spawnScript.Build(2, buildPanel);
                    }
                }
            }
        }
        private PanelBehaviour FindLocationOfWeakBlock(string Name,out string blocktype)
        {
            PanelBehaviour desiredLocation = new PanelBehaviour();
            BlockBehaviour temp = new BlockBehaviour();
            blocktype = "";
            if (Name == "Player1")
            {
                temp = BlackBoard.p1Blocks[0];
                desiredLocation = temp.Panel;
                blocktype = BlackBoard.p1Blocks[0].Type;
                foreach (BlockBehaviour block in BlackBoard.p1Blocks)
                {
                    if (block.types.Contains("Support"))
                    {
                        continue;
                    }
                    if ( block.HealthScript.health.Val < temp.HealthScript.health.Val)
                    {
                        temp = block;
                        desiredLocation = temp.Panel;
                        blocktype = block.Type;
                    }
                }
            }
            else
            {
                temp = BlackBoard.p2Blocks[0];
                desiredLocation = temp.Panel;
                blocktype = BlackBoard.p2Blocks[0].Type;
                foreach (BlockBehaviour block in BlackBoard.p2Blocks)
                {
                    if (block.HealthScript.health.Val < temp.HealthScript.health.Val && block.types.Contains("Support") == false)
                    {
                        temp = block;
                        desiredLocation = temp.Panel;
                        blocktype = block.Type;
                    }
                }
            }
            return desiredLocation;
        }
        private IEnumerator Shoot()
        {
            yield return new WaitForSecondsRealtime(3);
            while (gameObject != null)
            {
                int layerMask = 1 << 9;
                if (Physics.Raycast(transform.position, transform.forward, out _interactionRay, 50,layerMask))
                {
                    if(_interactionRay.collider.gameObject.CompareTag("Barrier"))
                    {
                        IUpgradable barrier = _interactionRay.collider.gameObject.GetComponent<IUpgradable>();
                        if(barrier.block.owner.name != name)
                        {
                            _attackScript.FireGun();
                        }
                    }
                    else
                    {
                        BlockBehaviour blockScript = _interactionRay.collider.gameObject.GetComponent<BlockBehaviour>();
                        if (blockScript != null)
                        {
                            if (blockScript.owner.name != name)
                            {
                                _attackScript.FireGun();
                            }
                        }
                        else
                        {
                            _attackScript.FireGun();
                        }
                    }
                    
                }
                yield return new WaitForSeconds(_shootDelay);
            }
        }
        private void UpgradeBlock()
        {
            string blockType;
            PanelBehaviour desiredLocation = FindLocationOfWeakBlock(name,out blockType);
            switch (blockType)
            {
                case("Attack"):
                    {
                        if (_spawnScript.Build(0, desiredLocation) == false && desiredLocation.CurrentBlock.types.Contains("Support") == false)
                        {
                            _spawnScript.Delete(desiredLocation);
                        }
                        break;
                    }
                case ("Defense"):
                    {
                        if (_spawnScript.Build(1, desiredLocation) ==false && desiredLocation.CurrentBlock.types.Contains("Support") == false)
                        {
                            _spawnScript.Delete(desiredLocation);
                        }
                        break;
                    }
                case ("Support"):
                    {
                        if (_spawnScript.Build(2, desiredLocation) == false)
                        {
                            _spawnScript.Delete(desiredLocation);
                        }
                        break;
                    }
            }
        }
        private void AttackOpponentOpening()
        {
            if(isAttacking)
            {
                return;
            }
            isAttacking = true;
            PanelList characterPanelList = new PanelList();
            if (name == "Player1")
            {
                characterPanelList = BlackBoard.p1PanelList;
            }
            else
            {
                characterPanelList = BlackBoard.p2PanelList;
            }
            _spawnScript.Build(0, Aim());
            isAttacking = false;
        }
        private void AttackOpponentWeakSpot()
        {
            if(isAttacking)
            {
                return;
            }
            isAttacking = true;
            PanelList characterPanelList = new PanelList();
            int weakYCoord;
            string blockType;
            if (name == "Player1")
            {
                characterPanelList = BlackBoard.p1PanelList;
                weakYCoord = (int)FindLocationOfWeakBlock("Player2", out blockType).Position.y;
            }
            else
            {
                characterPanelList = BlackBoard.p2PanelList;
                weakYCoord = (int)FindLocationOfWeakBlock("Player1", out blockType).Position.y;
            }
            if(yCoords.Count == 0)
            {
                yCoords.Add(weakYCoord);
            }
            else
            {
                yCoords[0] = weakYCoord;
            }
            _spawnScript.Build(0, Aim());
            isAttacking = false;
        }
        private float GradeFriendlyGrid()
        {
            List<BlockBehaviour> characterBlockList =new List<BlockBehaviour>();
            PanelList characterPanelList= new PanelList();
            HealthBehaviour characterCoreHealth = new HealthBehaviour();
            if(name == "Player1")
            {
                characterBlockList = BlackBoard.p1Blocks;
                characterPanelList = BlackBoard.p1PanelList;
                characterCoreHealth = BlackBoard.p1Core.GetComponent<HealthBehaviour>();
            }
            else
            {
                characterBlockList = BlackBoard.p2Blocks;
                characterPanelList = BlackBoard.p2PanelList;
                characterCoreHealth = BlackBoard.p2Core.GetComponent<HealthBehaviour>();
            }
            if(characterPanelList != null)
            {
                _blockAmountScore = ((float)characterBlockList.Count / (float)characterPanelList.Count) * 100f;
            }
            float totalBlockHealth = 0;
            float currentBlockHealth = 0;
            foreach(BlockBehaviour block in characterBlockList)
            {
                if(block.types.Contains("Support"))
                {
                    continue;
                }
                totalBlockHealth += block.HealthScript.HealthRef.Val;
            }
            foreach (BlockBehaviour block in characterBlockList)
            {
                if (block.types.Contains("Support"))
                {
                    continue;
                }
                currentBlockHealth += block.HealthScript.health.Val;
            }
            _blockHealthScore = (currentBlockHealth / totalBlockHealth) * 100f;
            _coreScore = ((((float)characterCoreHealth.health.Val / 200f) * 100f) + CoreDefenseCheck())/2;
            _supportScore= ResourceStatusCheck();
            _totalScore = (_blockAmountScore + _blockHealthScore+_coreScore+_supportScore) / 4f;
            return _totalScore;
        }
        private PanelBehaviour Aim()
        {
            List<BlockBehaviour> characterBlockList = new List<BlockBehaviour>();
            PanelList characterPanelList = new PanelList();
            PanelBehaviour targetPanel = new PanelBehaviour();
            Vector2 location = new Vector2();
            
            if (name == "Player1")
            {
                characterBlockList = BlackBoard.p1Blocks;
                characterPanelList = BlackBoard.p1PanelList;
                location.x = 4;
            }
            else
            {
                characterBlockList = BlackBoard.p2Blocks;
                characterPanelList = BlackBoard.p2PanelList;
                location.x = 5;
            }
            foreach (BlockBehaviour block in characterBlockList)
            {
                if (yCoords[0] == block.Panel.Position.y)
                {
                    if (name == "Player1")
                    {
                        location.x = block.Panel.Position.x + 1;
                    }
                    else
                    {
                        if (block.Panel.Position.x - 1 < 5)
                        {
                            location.x = block.Panel.Position.x;
                        }
                        else
                        {
                            location.x = block.Panel.Position.x - 1;
                        }

                    }

                }
            }
            location.y = yCoords[0];
            characterPanelList.FindPanel(location, out targetPanel);
            return targetPanel;
        }
        private bool OpeningCheck()
        {
            
            List<BlockBehaviour> characterBlockList = new List<BlockBehaviour>();
            PanelList characterPanelList = new PanelList();
            if (name == "Player1")
            {
                characterBlockList = BlackBoard.p2Blocks;
                characterPanelList = BlackBoard.p2PanelList;
            }
            else
            {
                characterBlockList = BlackBoard.p1Blocks;
                characterPanelList = BlackBoard.p1PanelList;
            }
            foreach (BlockBehaviour block in characterBlockList)
            {
                if(yCoords.Contains((int)block.Panel.Position.y))
                {
                    yCoords.Remove((int)block.Panel.Position.y);
                }
            }
            if(yCoords.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
		private void Update()
		{
            if (healthScript.health.Val < healthScript.HealthRef.Val * .50)
            {
                SendMessage("StopMaterialLoss");
                if(_spawnScript.PlayerSpawnScript.overdriveEnabled)
                {
                    _decisionTree.Decisions.SetCondition("GridStatusCheck", GradeFriendlyGrid() == 80);
                    _decisionTree.Decisions.SetCondition("CoreStatusCheck", _coreScore > 60);
                    _decisionTree.Decisions.SetCondition("SupportStatusCheck", _supportScore == 100);
                    _decisionTree.Decisions.SetCondition("SupportDefenseCheck", _supportDefenseScore == 100);
                    _decisionTree.Decisions.SetCondition("OpponentOpeningCheck", OpeningCheck());
                    return;
                }
            }
            _decisionTree.Decisions.SetCondition("GridStatusCheck", GradeFriendlyGrid() > gridScorePass);
            _decisionTree.Decisions.SetCondition("CoreStatusCheck", _coreScore>coreScorePass);
            _decisionTree.Decisions.SetCondition("SupportStatusCheck", _supportScore == supportScorePass);
            _decisionTree.Decisions.SetCondition("SupportDefenseCheck", _supportDefenseScore == supportDefScorePass);
            _decisionTree.Decisions.SetCondition("OpponentOpeningCheck", OpeningCheck());
        }
	}
}
