using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
	public class OrbiterBlockBehaviour : MonoBehaviour {
		
		[SerializeField]
		private BlockBehaviour _block;
        [SerializeField]
        private GameObject _orb1;
        [SerializeField]
        private GameObject _orb2;
        [SerializeField]
        private GameObject _orb3;
        [SerializeField]
        private int _orbUpgradeVal;
        [SerializeField]
        private GameEventListener _upgradeEventListener;
        [SerializeField]
        private GameEventListener _deleteEventListener;
        private void Start()
        {
            transform.parent = null;
        }
        public void UpgradeBlock(GameObject otherBlock)
		{
			BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
			for (int i = 0; i < _blockScript.componentList.Count; i++)
			{
				if (_blockScript.componentList[i].name == gameObject.name)
				{
					_blockScript.componentList[i].GetComponent<OrbiterBlockBehaviour>().UpgradeOrbs();
                    return;
				}
			}
			TransferOwner(otherBlock);
		}
        public void DestroyOrbs()
        {
            GameObject temp = _orb1;
            GameObject temp2 = _orb2;
            GameObject temp3 = _orb3;
            Destroy(temp);
            Destroy(temp2);
            Destroy(temp3);
        }
        public void UpgradeOrbs()
        {
            if(_orb2.activeInHierarchy)
            {
                _orb3.SetActive(true);
            }
            _orb2.SetActive(true);
        }
        public void ChangeOrbProperties(GameObject otherBlock)
        {
            if(otherBlock == null)
            {
                return;
            }
            BlockBehaviour _blockScript = otherBlock.GetComponent<BlockBehaviour>();
            for (int i = 0; i < _blockScript.componentList.Count; i++)
            {
                if (_blockScript.componentList[i].CompareTag("Barrier") || _blockScript.componentList[i].CompareTag("Gun"))
                {
                    _blockScript.componentList[i].transform.SetParent(_orb1.transform,false);
                    _blockScript.componentList[i].transform.localScale = _orb1.transform.localScale;
                    Instantiate(_blockScript.componentList[i], _orb2.transform, false);
                    return;
                }
            }
        }
		public void TransferOwner(GameObject otherBlock)
		{
			_block = otherBlock.GetComponent<BlockBehaviour>();
			_block.componentList.Add(gameObject);
            transform.position = otherBlock.transform.position;
            _upgradeEventListener.intendedSender = otherBlock;
            _deleteEventListener.intendedSender = otherBlock;
            ChangeOrbProperties(otherBlock);
		}
	}
}
