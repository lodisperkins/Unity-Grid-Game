using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lodis.GamePlay
{
    public class BlockTransporterBehaviour : MonoBehaviour
    {
        public Vector3 moveDirection;
        public Transform ownerTransform;
        public PlayerSpawnBehaviour _playerSpawnScript;
        public string owner;
        public float flyForce;
        public SeekBehaviour seekScript;
        private bool _deployed;
        public Rigidbody rigidbody;
        private GridScripts.PanelBehaviour _currentPanel;
        private UnityAction _explodeOnImpact;
        private UnityAction _spawnBlockOnImpact;
        [SerializeField]
        private BlockDisplayBehaviour blockDisplay;
        private int _currentBlockIndex;
        private GameObject displayBlock;
        private bool _blockDropped;
        public bool Deployed
        {
            get
            {
                return _deployed;
            }
        }

        // Use this for initialization
        void Start()
        {
            seekScript.Init(ownerTransform.position, rigidbody.velocity, 5, 5);
            _explodeOnImpact += ExplodePanel;
            _explodeOnImpact += DestroyDisplayBlock;
            _spawnBlockOnImpact += SpawnBlock;
            _spawnBlockOnImpact += DestroyDisplayBlock;
            _currentBlockIndex = _playerSpawnScript.CurrentIndex;
            blockDisplay.DisplayBlock(_currentBlockIndex);
            displayBlock = blockDisplay.Blocks[_currentBlockIndex];
        }
        public void Idle()
        {
            seekScript.SeekEnabled = false;
        }
        public void ExplodePanel()
        {
            BlackBoard.grid.ExplodePanel(_currentPanel,true,5);
        }
        public void SpawnBlock()
        {
            var position = new Vector3(_currentPanel.gameObject.transform.position.x, .6f, _currentPanel.transform.position.z);
            GameObject BlockCopy = Instantiate(blockDisplay.Blocks[_currentBlockIndex], position, _playerSpawnScript.Block_rotation);
            BlockBehaviour copyScript = BlockCopy.GetComponent<BlockBehaviour>();
            copyScript.currentPanel = _currentPanel.gameObject;
            copyScript.owner = _playerSpawnScript.gameObject;
            copyScript.InitializeBlock();
            copyScript.currentPanel.GetComponent<GridScripts.PanelBehaviour>().blockCounter -= 1;
            _currentPanel.Occupied = true;
            BlockCopy.GetComponent<Collider>().isTrigger = true;
            Destroy(gameObject);
        }
        public void DestroyDisplayBlock()
        {
            Destroy(blockDisplay.CurrentBlock);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Panel"))
            {
                _currentPanel = other.GetComponent<GridScripts.PanelBehaviour>();
            }
        }
        public void Deploy()
        {
            seekScript.SeekEnabled = false;
            transform.position = ownerTransform.position;
            rigidbody.AddForce(moveDirection * flyForce, ForceMode.Acceleration);
            _deployed = true;
        }
        public void DropBlock()
        {
            if(!_currentPanel)
            {
                return;
            }
            blockDisplay.CurrentBlock.AddComponent<SeekBehaviour>();
            SeekBehaviour blockSeekScript = blockDisplay.CurrentBlock.GetComponent<SeekBehaviour>();
            if (_currentPanel.BlockCapacityReached || _currentPanel.IsBroken)
            {
                blockSeekScript.Init(BlackBoard.grid.GetPanelFromGlobalList(_currentPanel.Position).transform.position, blockDisplay.CurrentBlock.GetComponent<Rigidbody>().velocity, 10, 2, true);
                blockSeekScript.onTargetReached.AddListener(_explodeOnImpact);
                blockDisplay.GetComponent<Rigidbody>().isKinematic = false;

                GameObject temp = gameObject;
                Destroy(temp, .5f);
                _blockDropped = true;
            }
            else
            {
                _currentPanel.Occupied = true;
                PlaceBlock();
            }
            
        }

        public void PlaceBlock()
        {
            seekScript.SetTarget(_currentPanel.transform.position, 5);
            seekScript.enabled = true;
            seekScript.onTargetReached.AddListener(_spawnBlockOnImpact);
        }
        void CheckPosition()
        {
            if (transform.position.x >= 1 || transform.position.x <= -7 || transform.position.z >= 29 || transform.position.z <= 8)
            {
                DestroyDisplayBlock();
                GameObject temp = gameObject;
                Destroy(temp);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if(_currentBlockIndex != _playerSpawnScript.CurrentIndex && !Deployed)
            {
                _currentBlockIndex = _playerSpawnScript.CurrentIndex;
                blockDisplay.DisplayBlock(_currentBlockIndex);
                displayBlock = blockDisplay.Blocks[_currentBlockIndex];
            }
            if(blockDisplay.CurrentBlock && ! _blockDropped)
            {
                blockDisplay.CurrentBlock.transform.position = blockDisplay.transform.position;
            }
            CheckPosition();
            if(seekScript && ownerTransform && !Deployed)
            {
                seekScript.SetTarget(ownerTransform.position, 5);
            }
            
        }
    }
}
