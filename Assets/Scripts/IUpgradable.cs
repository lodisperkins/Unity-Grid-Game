using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lodis
{
    public interface IUpgradable
    {
        BlockBehaviour block
        {
            get;set;
        }
        GameObject specialFeature
        {
            get;
        }
        void UpgradeBlock(GameObject otherBlock);
        void TransferOwner(GameObject otherBlock);
        void ResolveCollision(GameObject collision);
        void ActivateDisplayMode();
    }
}

