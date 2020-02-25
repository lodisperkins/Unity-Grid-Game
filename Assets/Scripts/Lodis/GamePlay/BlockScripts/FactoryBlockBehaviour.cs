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
        GridScripts.PanelBehaviour _currentPanel;
        private GridScripts.Condition NeighboorCheck;
        List<GridScripts.PanelBehaviour> panelsInRange;
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
            _playerBlocks = _blockScript.owner.GetComponent<PlayerSpawnBehaviour>().Blocks;
            _currentPanel = _blockScript.currentPanel.GetComponent<GridScripts.PanelBehaviour>();
            _blockIndex = 0;
            _currentBlock = new BlockVariable();
            _currentBlock.Block = _playerBlocks[_blockIndex];
            block.specialActions += SwitchBlocks;
            NeighboorCheck += CheckIfNeighboor;

            List<GridScripts.PanelBehaviour> panelsInRange = new List<GridScripts.PanelBehaviour>();
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
            FindNeighbors();
            _currentBlock.Block = _playerBlocks[_blockIndex];
            int panelIndex = Random.Range(0, panelsInRange.Count - 1);
            if (panelsInRange[panelIndex].blockCounter < 3 && _playerSpawnScript.CheckMaterial(_currentBlock.Cost))
            {
                _playerSpawnScript.BuyItem(_currentBlock.Cost);
               
                var position = new Vector3(panelsInRange[panelIndex].gameObject.transform.position.x, _currentBlock.Block.transform.position.y, panelsInRange[panelIndex].gameObject.transform.position.z);
                GameObject BlockCopy = Instantiate(_currentBlock.Block, position, _playerSpawnScript.Block_rotation);
                BlockBehaviour copyScript = BlockCopy.GetComponent<BlockBehaviour>();
                copyScript.currentPanel = panelsInRange[panelIndex].gameObject;
                copyScript.owner = _blockScript.owner;
                copyScript.specialFeature = block.specialFeature;
                Instantiate(specialFeature.gameObject, BlockCopy.transform);
                panelsInRange[panelIndex].Occupied = true;
                panelsInRange[panelIndex].Selected = false;
                BlockCopy.GetComponent<Collider>().isTrigger = true;
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
        public void Upgrade()
        {
            var routineScript = GetComponent<RoutineBehaviour>();
            routineScript.actionLimit += 3;
            routineScript.actionDelay -= 1;
        }
        public void TransferOwner(GameObject otherBlock)
        {
            BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
            blockScript.componentList.Add(this);
            transform.SetParent(otherBlock.transform, false);
        }

        public void ResolveCollision(GameObject collision)
        {
            return;
        }
    }
}
    
