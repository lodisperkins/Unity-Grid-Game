using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class BlockDisplayBehaviour : MonoBehaviour {
        [SerializeField]
        private GameObjectList _blocks;
        private GameObject currentBlock;
        [SerializeField]
        private float _blockScaleSize;
	    // Use this for initialization
	    public void DisplayBlock(int index)
        {
            if(currentBlock != null)
            {
                GameObject temp = currentBlock;
                Destroy(temp);
            }
            currentBlock = Instantiate(_blocks[index]);
            currentBlock.transform.position = transform.position;
            currentBlock.GetComponent<BlockBehaviour>().ActivateDisplayMode();
            currentBlock.transform.localScale *= _blockScaleSize;
        }
    }
}


