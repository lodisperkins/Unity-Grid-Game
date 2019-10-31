using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class MaterialBlockBehaviour : MonoBehaviour
    {
        //the player the material block is responsible for
        private GameObject Player;
        //the players spawning script that hold its current materials
        private PlayerSpawnBehaviour PlayerSpawner;
        //the reference to player materials
        private IntVariable PlayerMaterials;
        //the amount of materials the block increases the players by
        public int MaterialAmount;
        // Use this for initialization
        void Start()
        {
            Player = GetComponent<BlockBehaviour>().owner;
            PlayerSpawner = Player.GetComponent<PlayerSpawnBehaviour>();
        }
        //Adds materials to the players material pool
        public void AddMaterials()
        {
            PlayerSpawner.AddMaterials(MaterialAmount);
        }
    }
}
