using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis.GamePlay
{
    public class GridBehaviour : MonoBehaviour
    {
        [SerializeField] private PlayerMovementBehaviour _p1PanelsRef;

        [SerializeField]
        private PlayerMovementBehaviour  _p2PanelsRef;
        [SerializeField] private  PanelList P1Panels;
        [SerializeField] private PanelList P2Panels;
        [SerializeField]
        private Vector2Variable P1Position;
        [SerializeField]
        private Vector2Variable P2Position;
        [SerializeField]
        private Lodis.Event OnPanelsSwapped;
        [SerializeField]
        private IntVariable P1Materials;
        [SerializeField]
        private IntVariable P2Materials;
        [SerializeField]
        private Vector2Variable P1Direction;
        [SerializeField]
        private Vector2Variable P2Direction;

        [SerializeField] private Material _p1Material;
        [SerializeField] private Material _p2Material;
        // Use this for initialization
        void Start()
        {
            P1AssignLists();
            P2AssignLists();
        }

        private void Awake()
        {
            
        }

        public void P1AssignLists()
        {
            AssignPanelMaterials();
            P1Panels.updateOwners();
        }
        public void P2AssignLists()
        {
            AssignPanelMaterials();
            P2Panels.updateOwners();
        }
    
        public void surrenderRowP1()
        {
            P1Panels.SurrenderRow(P2Panels);
            P1Panels.updateOwners();
            P2Panels.updateOwners();
        }
        public void surrenderRowP2()
        {
            P2Panels.SurrenderRow(P1Panels);
            P1Panels.updateOwners();
            P2Panels.updateOwners();
        }

        public void AssignPanelMaterials()
        {
            foreach (GameObject panel in P1Panels)
            {
                int counter = 0;
                if (panel == null)
                {
                    Debug.Log("The panel at the index of " + counter + "is null");
                    counter++;
                    continue;
                }
                panel.GetComponent<PanelBehaviour>().Init(_p1Material,_p2Material);
                counter++;
            }
            foreach (GameObject panel in P2Panels)
            {
                int counter = 0;
                if (panel == null)
                {
                    Debug.Log("The panel at the index of " + counter + "is null");
                    counter++;
                    continue;
                }
                panel.GetComponent<PanelBehaviour>().Init(_p1Material,_p2Material);
                counter++;
            }
        }
        
        public void StealPanelP1()
        {
            Vector2 panelPosition = new Vector2(P1Position.Val.x + P1Direction.X, P1Position.Val.y+ P1Direction.Y);
            int index = 0;
            if (P2Panels.FindIndex(panelPosition, out index))
            {
                if (P1Materials.Val < 100)
                {
                    OnPanelsSwapped.Raise();
                    UnHighlightPanelsP1();
                    return;
                }
                P1Materials.Val -= 100;
                P2Panels.TransferPanel(P1Panels, index);
                UnHighlightPanelsP1();
                P1Panels.updateOwners();
                P2Panels.updateOwners();
                OnPanelsSwapped.Raise();
                
            }
        }
        public void StealPanelP2()
        {
            Vector2 panelPosition = new Vector2(P2Position.Val.x + P2Direction.X, P2Position.Val.y + P2Direction.Y);
            int index = 0;
            if (P1Panels.FindIndex(panelPosition, out index))
            {
                if (P2Materials.Val < 100)
                {
                    OnPanelsSwapped.Raise();
                    UnHighlightPanelsP2();
                    return;
                }
                P2Materials.Val -= 100;
                P1Panels.TransferPanel(P2Panels, index);
                UnHighlightPanelsP2();
                P2Panels.updateOwners();
                P1Panels.updateOwners();
                OnPanelsSwapped.Raise();
            }
        }
        public void FindNeighborsP1()
        {
            //Used to find the position the block can be placed
            Vector2 DisplacementX = new Vector2(1, 0);
            Vector2 DisplacementY = new Vector2(0, 1);
            P1Panels.tempPanels = new List<GameObject>();
            //Loops through all panels to find those whose position is the
            //player current position combined with x or y displacement
            foreach (GameObject panel in P2Panels)
            {
                var coordinate = panel.GetComponent<PanelBehaviour>().Position;
                if ((P1Position.Val + DisplacementX) == coordinate)
                {
                    P1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((P1Position.Val - DisplacementX) == coordinate)
                {
                    P1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((P1Position.Val + DisplacementY) == coordinate)
                {
                    P1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((P1Position.Val - DisplacementY) == coordinate)
                {
                    P1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
            }
        }
        public void UnHighlightPanelsP1()
        {
            foreach (GameObject panel in P1Panels.tempPanels)
            {
                panel.GetComponent<PanelBehaviour>().SelectionColor = Color.green;
                panel.GetComponent<PanelBehaviour>().Selected = false;
            }
        }
        public void UnHighlightPanelsP2()
        {
            foreach (GameObject panel in P2Panels.tempPanels)
            {
                panel.GetComponent<PanelBehaviour>().SelectionColor = Color.green;
                panel.GetComponent<PanelBehaviour>().Selected = false;
            }
        }
        public void FindNeighborsP2()
        {
            //Used to find the position the block can be placed
            Vector2 DisplacementX = new Vector2(1, 0);
            Vector2 DisplacementY = new Vector2(0, 1);
            P2Panels.tempPanels = new List<GameObject>();
            //Loops through all panels to find those whose position is the
            //player current position combined with x or y displacement
            foreach (GameObject panel in P1Panels)
            {
                var coordinate = panel.GetComponent<PanelBehaviour>().Position;
                if ((P2Position.Val + DisplacementX) == coordinate)
                {
                    P2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((P2Position.Val - DisplacementX) == coordinate)
                {
                    P2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((P2Position.Val + DisplacementY) == coordinate)
                {
                    P2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((P2Position.Val - DisplacementY) == coordinate)
                {
                    P2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.yellow;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
        }
    }
}
