﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lodis.GamePlay
{
    public class GridBehaviour : MonoBehaviour
    {
       //The reference to player 1's panels
        [FormerlySerializedAs("P1Panels")] [SerializeField] private  PanelList p1Panels;
        //The reference to player 2's panels
        [FormerlySerializedAs("P2Panels")] [SerializeField] private PanelList p2Panels;
        // player 1's current position on the grid
        [FormerlySerializedAs("P1Position")] [SerializeField]
        private Vector2Variable p1Position;
        // player 2's current position on the grid
        [FormerlySerializedAs("P2Position")] [SerializeField]
        private Vector2Variable p2Position;
        [FormerlySerializedAs("OnPanelsSwapped")] [SerializeField]
        private Lodis.Event onPanelsSwapped;
        //The amount of materials the players curretly have
        [FormerlySerializedAs("P1Materials")] [SerializeField]
        private IntVariable p1Materials;
        [FormerlySerializedAs("P2Materials")] [SerializeField]
        private IntVariable p2Materials;
        //the direction both players are facing
        [FormerlySerializedAs("P1Direction")] [SerializeField]
        private Vector2Variable p1Direction;
        [FormerlySerializedAs("P2Direction")] [SerializeField]
        private Vector2Variable p2Direction;
//The amount of materials the players have
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
        //Sets player1 panels to the appropriate material and sets their owner to be player 1
        public void P1AssignLists()
        {
            AssignPanelMaterials();
            p1Panels.updateOwners();
        }
        //Sets player2 panels to the appropriate material and sets their owner to be player 2
        public void P2AssignLists()
        {
            AssignPanelMaterials();
            p2Panels.updateOwners();
        }
        //Removes an entire row from player1 and gives it to player2
        public void surrenderRowP1()
        {
            p1Panels.SurrenderRow(p2Panels);
            p1Panels.updateOwners();
            p2Panels.updateOwners();
        }
        //Removes an entire row from player2 and gives it to player1
        public void surrenderRowP2()
        {
            p2Panels.SurrenderRow(p1Panels);
            p1Panels.updateOwners();
            p2Panels.updateOwners();
        }
        //Sets both players materials to either their red or blue variants
        public void AssignPanelMaterials()
        {
            foreach (GameObject panel in p1Panels)
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
            foreach (GameObject panel in p2Panels)
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
        //Takes one panel from player two and gives it player one
        public void StealPanelP1()
        {
            Vector2 panelPosition = new Vector2(p1Position.Val.x + p1Direction.X, p1Position.Val.y+ p1Direction.Y);
            int index = 0;
            if (p2Panels.FindIndex(panelPosition, out index))
            {
                if (p1Materials.Val < 60)
                {
                    onPanelsSwapped.Raise();
                    UnHighlightPanelsP1();
                    return;
                }
                p1Materials.Val -= 60;
                p2Panels.TransferPanel(p1Panels, index);
                UnHighlightPanelsP1();
                p1Panels.updateOwners();
                p2Panels.updateOwners();
                onPanelsSwapped.Raise();
                
            }
        }
        //takes one panel from player 1 and gives it to player two
        public void StealPanelP2()
        {
            Vector2 panelPosition = new Vector2(p2Position.Val.x + p2Direction.X, p2Position.Val.y + p2Direction.Y);
            int index = 0;
            if (p1Panels.FindIndex(panelPosition, out index))
            {
                if (p2Materials.Val < 60)
                {
                    onPanelsSwapped.Raise();
                    UnHighlightPanelsP2();
                    return;
                }
                p2Materials.Val -= 60;
                p1Panels.TransferPanel(p2Panels, index);
                UnHighlightPanelsP2();
                p2Panels.updateOwners();
                p1Panels.updateOwners();
                onPanelsSwapped.Raise();
            }
        }
        //finds and highlights all nearby panels in player 2's list for player1
        public void FindNeighborsP1()
        {
            //Used to find the position the block can be placed
            Vector2 DisplacementX = new Vector2(1, 0);
            Vector2 DisplacementY = new Vector2(0, 1);
            p1Panels.tempPanels = new List<GameObject>();
            //Loops through all panels to find those whose position is the
            //player current position combined with x or y displacement
            foreach (GameObject panel in p2Panels)
            {
                var coordinate = panel.GetComponent<PanelBehaviour>().Position;
                if ((p1Position.Val + DisplacementX) == coordinate)
                {
                    p1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((p1Position.Val - DisplacementX) == coordinate)
                {
                    p1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((p1Position.Val + DisplacementY) == coordinate)
                {
                    p1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((p1Position.Val - DisplacementY) == coordinate)
                {
                    p1Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
            }
        }
        //unhighlights all previously highlighted panels for player1
        public void UnHighlightPanelsP1()
        {
            foreach (GameObject panel in p1Panels.tempPanels)
            {
                panel.GetComponent<PanelBehaviour>().SelectionColor = Color.green;
                panel.GetComponent<PanelBehaviour>().Selected = false;
            }
        }
        //unhighlights all previously highlighted panels for player1
        public void UnHighlightPanelsP2()
        {
            foreach (GameObject panel in p2Panels.tempPanels)
            {
                panel.GetComponent<PanelBehaviour>().SelectionColor = Color.green;
                panel.GetComponent<PanelBehaviour>().Selected = false;
            }
        }
        //finds and highlights all nearby panels in player 2's list for player1
        public void FindNeighborsP2()
        {
            //Used to find the position the block can be placed
            Vector2 DisplacementX = new Vector2(1, 0);
            Vector2 DisplacementY = new Vector2(0, 1);
            p2Panels.tempPanels = new List<GameObject>();
            //Loops through all panels to find those whose position is the
            //player current position combined with x or y displacement
            foreach (GameObject panel in p1Panels)
            {
                var coordinate = panel.GetComponent<PanelBehaviour>().Position;
                if ((p2Position.Val + DisplacementX) == coordinate)
                {
                    p2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((p2Position.Val - DisplacementX) == coordinate)
                {
                    p2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((p2Position.Val + DisplacementY) == coordinate)
                {
                    p2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
                    panel.GetComponent<PanelBehaviour>().Selected = true;
                }
                else if ((p2Position.Val - DisplacementY) == coordinate)
                {
                    p2Panels.tempPanels.Add(panel);
                    panel.GetComponent<PanelBehaviour>().SelectionColor = Color.magenta;
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
