using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lodis
{
    /// <summary>
    /// These are the items that each block has that can be passsed on to another
    /// block to upgrade it. These are usually the defining feature of a block.
    /// For example, the IUpgradable item for an Attack block would be its AttackBlockBehaviour script
    /// that allows it to fire.
    /// </summary>
    public interface IUpgradable
    {
        //The block this item is attached to
        BlockBehaviour block
        {
            get;set;
        }
        //The defining characteristic for this item. This is its ability Ex: Attack Block - Bullet Emitter
        GameObject specialFeature
        {
            get;
        }
        //The name of this item
        string Name
        {
            get; 
        }
        //Upgrades whatever block this IUpgradable item has touched
        void UpgradeBlock(GameObject otherBlock);
        //Transfers ownership of this item to the block that this item is upgrading
        void TransferOwner(GameObject otherBlock);
        //Does whatever this item needs to do upon colliding with any other object
        void ResolveCollision(GameObject collision);
        //Disables whatever component this item has so that it may be displayed using whatever ability it has
        void ActivateDisplayMode();
    }
}

