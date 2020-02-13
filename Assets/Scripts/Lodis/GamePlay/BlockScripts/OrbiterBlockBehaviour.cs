using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class OrbiterBlockBehaviour : MonoBehaviour {
		
		[SerializeField]
		private BlockBehaviour block;
		public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
			for (int i = 0; i < _blockScript.componentList.Count; i++)
			{
				if (_blockScript.componentList[i].name == gameObject.name)
				{
					_blockScript.componentList[i].GetComponent<KineticBlockBehaviour>().bulletCapacity+= _bulletCapUpgradeVal;
					return;
				}
			}
			TransferOwner(otherBlock);
		}
		public void TransferOwner(GameObject otherBlock)
		{
			block = otherBlock.GetComponent<BlockBehaviour>();
			block.componentList.Add(gameObject);
			transform.SetParent(otherBlock.transform,false);
		}
	}
}
