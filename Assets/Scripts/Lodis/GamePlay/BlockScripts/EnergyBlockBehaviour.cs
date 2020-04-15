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

        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        public Color displayColor
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
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
        /// <summary>
        /// Upgrades:
        /// Increases the amount of energy given
        /// Decreases energy boost cooldown
        /// </summary>
        /// <param name="otherBlock"></param>
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
        /// <summary>
        /// Transfers the energy boost component
        /// </summary>
        /// <param name="otherBlock"></param>
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
        //Turns off particle effect
        public void ActivateDisplayMode()
        {
            gameObject.SetActive(false);
            return;
        }

        public void UpgradePlayer(PlayerAttackBehaviour player)
        {
            throw new System.NotImplementedException();
        }

        public void PlayerAttack()
        {
            throw new System.NotImplementedException();
        }

        public void DetachFromPlayer()
        {
            throw new System.NotImplementedException();
        }
    }
}
