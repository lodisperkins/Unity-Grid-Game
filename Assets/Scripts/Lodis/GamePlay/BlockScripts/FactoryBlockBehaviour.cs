using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis.GamePlay.BlockScripts
{
    public class FactoryBlockBehaviour : MonoBehaviour,IUpgradable {
        private Lodis.PlayerMovementBehaviour _playerMoveScript;
        private Lodis.PlayerSpawnBehaviour _playerSpawnScript;
        private List<GameObject> _playerBlocks;
        private BlockVariable _currentBlock;
        private int _blockIndex;
        [SerializeField]
        private BlockBehaviour _blockScript;
        [SerializeField]
        private RoutineBehaviour _spawnRoutine;
        GridScripts.PanelBehaviour _currentPanel;
        private GridScripts.Condition NeighboorCheck;
        List<GridScripts.PanelBehaviour> panelsInRange;
        private Color SelectionColor;
        private string colorName;
        [SerializeField]
        private GameObject box;
        private Material _attachedMaterial;
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
        void Start () {
            _playerMoveScript = _blockScript.owner.GetComponent<PlayerMovementBehaviour>();
            _playerSpawnScript = _blockScript.owner.GetComponent<PlayerSpawnBehaviour>();
            _playerBlocks = _playerSpawnScript.Blocks;
            _currentPanel = _blockScript.currentPanel.GetComponent<GridScripts.PanelBehaviour>();
            _blockIndex = 0;
            _currentBlock = new BlockVariable();
            _attachedMaterial = box.GetComponent<MeshRenderer>().material;
            _currentBlock.Block = _playerBlocks[_blockIndex];
            block.specialActions += SwitchBlocks;
            NeighboorCheck += CheckIfNeighboor;
            colorName = "Color_262603E3";
            _blockScript.Health.health.Val = _spawnRoutine.numberOfActionsLeft;
            List<GridScripts.PanelBehaviour> panelsInRange = new List<GridScripts.PanelBehaviour>();
            FindNeighbors();
        }
        public bool CheckIfNeighboor(object[] arg)
        {
            GameObject temp = (GameObject)arg[0];
            Vector2 position = temp.GetComponent<GridScripts.PanelBehaviour>().Position;
            Vector2 displacdementX = new Vector2(1, 0);
            Vector2 displacdementY = new Vector2(0, 1);
            if (position == _currentPanel.Position + displacdementX || position == _currentPanel.Position - displacdementX)
            {
                return true;
            }
            if (position == _currentPanel.Position + displacdementY || position == _currentPanel.Position - displacdementY)
            {
                return true;
            }
            return false;
        }
        public bool FindNeighbors()
        {
            if (_playerMoveScript.Panels.GetPanels(NeighboorCheck, out panelsInRange))
            {
                return true;
            }
            Debug.Log("Couldn't find neighboors");
            return false;
        }
        public void SpawnBlock()
        {
            _currentBlock.Block = _playerBlocks[_blockIndex];
            int panelIndex = Random.Range(0, panelsInRange.Count - 1);
            if (panelsInRange[panelIndex].blockCounter < 3 )
            {
               
                var position = new Vector3(panelsInRange[panelIndex].gameObject.transform.position.x, _currentBlock.Block.transform.position.y, panelsInRange[panelIndex].gameObject.transform.position.z);
                GameObject BlockCopy = Instantiate(_currentBlock.Block, position, _playerSpawnScript.Block_rotation);
                BlockBehaviour copyScript = BlockCopy.GetComponent<BlockBehaviour>();
                copyScript.currentPanel = panelsInRange[panelIndex].gameObject;
                copyScript.owner = _blockScript.owner;
                copyScript.InitializeBlock();
                TransferUpgrades(copyScript);
                panelsInRange[panelIndex].Occupied = true;
                panelsInRange[panelIndex].Selected = false;
                BlockCopy.GetComponent<Collider>().isTrigger = true;
                block.Health.takeDamage(1);
            }
        }
        public void SwitchBlocks(object[] args)
        {
            _blockIndex++;
            if(_blockIndex > 3)
            {
                _blockIndex = 0;
            }
        }
        private void UpdateColor()
        {
            switch(_blockIndex)
            {
                case (0):
                    {
                        SelectionColor = Color.red;
                        break;
                    }
                case (1):
                    {
                        SelectionColor = Color.green;
                        break;
                    }
                case (2):
                    {
                        SelectionColor = Color.yellow;
                        break;
                    }
                case (3):
                    {
                        SelectionColor = Color.white;
                        break;
                    }
            }
            //highlights panels
            //foreach(GridScripts.PanelBehaviour panel in panelsInRange)
            //{
            //    panel.SelectionColor = SelectionColor;
            //    panel.Selected = true;
            //}
            if(_attachedMaterial != null)
            {
                _attachedMaterial.SetColor(colorName, SelectionColor);
            }
        }
        public void UnHighlightPanels()
        {
            foreach (GridScripts.PanelBehaviour panel in panelsInRange)
            {
                panel.Selected = false;
            }
        }
        public void UpgradeBlock(GameObject otherBlock)
        {
            BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
                    component.specialFeature.GetComponent<FactoryBlockBehaviour>().Upgrade();
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        private void TransferUpgrades(BlockBehaviour spawnedBlock)
        {
            foreach(IUpgradable component in block.componentList)
            {
                if(component.specialFeature.name == gameObject.name)
                {
                    continue;
                }
                spawnedBlock.InitializeBlock();
                component.TransferOwner(spawnedBlock.gameObject);
                Instantiate(component.specialFeature.gameObject, spawnedBlock.transform);
            }
        }
        public void DestroyFactory()
        {
            _spawnRoutine.StopAllCoroutines();
            block.DestroyBlock();
        }
        public void Upgrade()
        {
            var routineScript = GetComponent<RoutineBehaviour>();
            routineScript.ResetActions();
            routineScript.actionLimit += 3;
            routineScript.actionDelay -= 1;
            _blockScript.Health.health.Val = _spawnRoutine.numberOfActionsLeft;
        }
        public void TransferOwner(GameObject otherBlock)
        {
            BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
            blockScript.componentList.Add(this);
            transform.SetParent(otherBlock.transform, false);
        }
        private void OnDestroy()
        {
            UnHighlightPanels();
            block.currentPanel.GetComponent<GridScripts.PanelBehaviour>().Occupied = false;
        }
        public void ResolveCollision(GameObject collision)
        {
            return;
        }
        private void Update()
        {
            UpdateColor();
        }
    }
}
    
