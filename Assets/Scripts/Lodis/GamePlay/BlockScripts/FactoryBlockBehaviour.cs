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

            NeighboorCheck += CheckIfNeighboor;

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
                SpawnBlock();
                return true;
            }
            Debug.Log("Couldn't find neighboors");
            return false;
        }
        public void SpawnBlock()
        {
            _playerSpawnScript.BuyItem(_currentBlock.Cost);
            var position = new Vector3(panelsInRange[0].gameObject.transform.position.x, _currentBlock.Block.transform.position.y, panelsInRange[0].gameObject.transform.position.z);
            GameObject BlockCopy = Instantiate(_currentBlock.Block, position, _playerSpawnScript.Block_rotation);
            BlockCopy.GetComponent<BlockBehaviour>().currentPanel = panelsInRange[0].gameObject;
            BlockCopy.GetComponentInChildren<BlockBehaviour>().owner = _blockScript.owner;
            panelsInRange[0].Occupied = true;
            panelsInRange[0].Selected = false;
            BlockCopy.GetComponent<Collider>().isTrigger = true;
        }
        // Update is called once per frame
        void Update () {
		
	    }

        public void UpgradeBlock(GameObject otherBlock)
        {
            throw new System.NotImplementedException();
        }

        public void TransferOwner(GameObject otherBlock)
        {
            throw new System.NotImplementedException();
        }

        public void ResolveCollision(GameObject collision)
        {
            return;
        }
    }
}
    
