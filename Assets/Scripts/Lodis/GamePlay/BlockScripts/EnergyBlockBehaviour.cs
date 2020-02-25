using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
    public class EnergyBlockBehaviour : MonoBehaviour,IUpgradable
    {
        //the player the material block is responsible for
        [SerializeField] private BlockBehaviour _blockScript;
        private GameObject Player;
        //the players spawning script that hold its current materials
        private PlayerSpawnBehaviour PlayerSpawner;
        //the amount of materials the block increases the players by
        public int MaterialAmount;

        public int materialUpgradeVal;

        public float regenTimeUpgradeVal;

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
        void Start()
        {
            Player = _blockScript.owner;
            PlayerSpawner = Player.GetComponent<PlayerSpawnBehaviour>();
        }
        //Adds materials to the players material pool
        public void AddMaterials()
        {
            PlayerSpawner.AddMaterials(MaterialAmount);
        }
        public void UpgradeBlock(GameObject otherBlock)
        {
            BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            foreach (IUpgradable component in _blockScript.componentList)
            {
                if (component.specialFeature.name == gameObject.name)
                {
                    component.specialFeature.GetComponent<EnergyBlockBehaviour>().MaterialAmount += materialUpgradeVal;
                    component.specialFeature.GetComponent<RoutineBehaviour>().actionDelay -= regenTimeUpgradeVal;
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        public void TransferOwner(GameObject otherBlock)
        {
            BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
            blockScript.componentList.Add(this);
            transform.SetParent(otherBlock.transform,false);
        }

        public void ResolveCollision(GameObject collision)
        {
            return;
        }
    }
}
