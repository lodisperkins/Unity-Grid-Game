using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class MaterialBlockBehaviour : MonoBehaviour
    {
        private GameObject Player;
        private PlayerSpawnBehaviour PlayerSpawner;
        private IntVariable PlayerMaterials;
        public int MaterialAmount;
        // Use this for initialization
        void Start()
        {
            Player = GetComponent<BlockBehaviour>().Owner;
            PlayerSpawner = Player.GetComponent<PlayerSpawnBehaviour>();
        }

        public void AddMaterials()
        {
            PlayerSpawner.AddMaterials(MaterialAmount);
        }
    }
}
