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
        [SerializeField] private int playerUseAmount;
        private PlayerAttackBehaviour playerAttackScript;
        [SerializeField]
        private TeleportBeamBehaviour teleportBeam;
        [SerializeField] private Color _displayColor;
        [SerializeField] private GunBehaviour bulletEmitter;
        [SerializeField] private bool _canBeHeld;
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
                return _displayColor;
            }

            set
            {
                _displayColor = value;
            }
        }

        public bool CanBeHeld
        {
            get
            {
                return _canBeHeld;
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
            playerAttackScript = player;
            playerAttackScript.weaponUseAmount = playerUseAmount;
            GetComponent<RoutineBehaviour>().shouldStop = true;
            GetComponent<RoutineBehaviour>().StopAllCoroutines();
            transform.SetParent(player.transform, false);
            teleportBeam.transform.parent = null;
            teleportBeam.Teleport(player.transform.position);
            bulletEmitter.owner = player.name;
            player.SetSecondaryWeapon(this, playerUseAmount);
        }

        public void ActivatePowerUp()
        {
            bulletEmitter.FireBullet();
        }

        public void DetachFromPlayer()
        {
            GameObject temp = gameObject;
            Destroy(temp);
        }

        public void DeactivatePowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
