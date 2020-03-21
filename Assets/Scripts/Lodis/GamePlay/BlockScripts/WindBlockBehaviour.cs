using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis.GamePlay
{
    public class WindBlockBehaviour : MonoBehaviour,IUpgradable {
        [SerializeField]
        private float windForce;
        private Vector3 velocity;
        private GridScripts.PanelBehaviour currentPanel;
        public List<GridScripts.PanelBehaviour> panelsInRange;
        [SerializeField]
        private BlockBehaviour _block;
        private void Start()
        {
            panelsInRange = new List<GridScripts.PanelBehaviour>();
            currentPanel = block.currentPanel.GetComponent<GridScripts.PanelBehaviour>();
        }
        public BlockBehaviour block
        {
            get
            {
                return _block;
            }

            set
            {
                _block = value;
            }
        }

        GameObject IUpgradable.specialFeature
        {
            get
            {
                return specialFeature;
            }
        }

        public string Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public GameObject specialFeature;

        public void ResolveCollision(GameObject other)
        {
            
        }

        public void TransferOwner(GameObject otherBlock)
        {
            return;
        }
        public Vector3 FindSafePanel(Vector3 OtherPosition)
        {
            GridScripts.PanelBehaviour safePanel = new GridScripts.PanelBehaviour();
            float lastDisplacementVal =0;
            foreach(var panel in panelsInRange)
            {
                if(panel.Occupied)
                {
                    continue;
                }
                float currentDisplacementVal = Mathf.Abs(panel.Position.x) - currentPanel.Position.x;
                if (currentDisplacementVal >= lastDisplacementVal )
                {
                    lastDisplacementVal = currentDisplacementVal;
                    safePanel = panel;
                }
            }
            Vector3 newPosition = new Vector3(safePanel.XAbsolute, OtherPosition.y, safePanel.ZAbsolute);
            return newPosition;
        }
        public void UpgradeBlock(GameObject otherBlock)
        {
            return;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Panel"))
            {
                panelsInRange.Add(other.GetComponent<GridScripts.PanelBehaviour>());
            }
            if (other.CompareTag("Block") && other != gameObject)
            {

                other.gameObject.AddComponent<SeekBehaviour>();
                other.gameObject.GetComponent<SeekBehaviour>().Init(FindSafePanel(other.transform.position), velocity, (int)windForce, 0.5f, true);
            }
        }
        public void ActivateDisplayMode()
        {
            return;
        }

    }
}

