using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
    public class EnergyBlockBehaviour : MonoBehaviour
    {
        //the player the material block is responsible for
        [SerializeField] private BlockBehaviour _blockScript;
        private GameObject Player;
        [SerializeField]
        //the players spawning script that hold its current materials
        private PlayerSpawnBehaviour PlayerSpawner;
        //the amount of materials the block increases the players by
        public int MaterialAmount;

        public int materialUpgradeVal;

        public float regenTimeUpgradeVal;
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
            for (int i = 0; i < _blockScript.componentList.Count; i++)
            {
                if (_blockScript.componentList[i].name == gameObject.name)
                {
                    _blockScript.componentList[i].GetComponent<EnergyBlockBehaviour>().MaterialAmount += materialUpgradeVal;
                    _blockScript.componentList[i].GetComponent<RoutineBehaviour>().actionDelay -= regenTimeUpgradeVal;
                    return;
                }
            }
            TransferOwner(otherBlock);
        }
        public void TransferOwner(GameObject otherBlock)
        {
            BlockBehaviour blockScript = otherBlock.GetComponent<BlockBehaviour>();
            blockScript.componentList.Add(gameObject);
            transform.SetParent(otherBlock.transform,false);
        }
    }
}
