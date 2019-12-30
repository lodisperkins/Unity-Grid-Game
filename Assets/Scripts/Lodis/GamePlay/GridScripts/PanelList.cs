﻿using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;

namespace Lodis.GamePlay
{
    [CreateAssetMenu(menuName = "PanelList")]
    public class PanelList : ScriptableObject
    {
        //raised when the a panel is somehow null
        [SerializeField] private Event OnPanelAssignmentFailed;
        //the list of panels tthis object holds
        [SerializeField]
        private List<GameObject> panels;
        public List<GameObject> Panels
        {
            get
            {
                return panels;
            }
        }
        //a list of panels meant to be in the players possesion only temporarily
        public List<GameObject> tempPanels;
        //the owner of the list of panels
        public string Owner;
        //initializes this panel list to have a populated list and an owner
        public void Init(List<GameObject> startPanels, string playername)
        {
            panels = startPanels;
            Owner = playername;
            updateOwners();
        }
        //creates an instance of this scriptable object
        public static PanelList CreateInstance(List<GameObject> startpanels, string playername)
        {
            var data = ScriptableObject.CreateInstance<PanelList>();
            data.Init(startpanels, playername);
            data.updateOwners();
            return data;
        }
        //Adds a panel to the list
        public void Add(GameObject panel)
        {
            panels.Add(panel);
        }
        //changes the owner of each panel in the list
        public void updateOwners()
        {
            int counter = 0;
            foreach (var panel in panels)
            {
                if (panel == null)
                {
                    Debug.Log("The panel at the index of " + counter + "is null");
                    OnPanelAssignmentFailed.Raise();
                    counter++;
                    continue;
                }
                panel.GetComponent<PanelBehaviour>().UpdateOwner(Owner);
                counter++;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return panels.GetEnumerator();
        }
        /// <summary>
        /// removes the desired panel from the 
        /// lit of panels.
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public bool RemovePanel(GameObject panel)
        {
            if (panels.Remove(panel))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Transfers a single panel from one player list of
        /// panels to another
        /// </summary>
        /// <param name="opponent_Panel_List"></param>
        /// <param name="panel_Index"></param>
        public void TransferPanel(PanelList opponent_Panel_List, int panel_Index)
        {
            GameObject temp  = panels[panel_Index];
            if (RemovePanel(panels[panel_Index]))
            {
                opponent_Panel_List.panels.Insert(0,temp);
            }
        }
        /// <summary>
        /// Transfers the last row of panels from one player to another
        /// </summary>
        /// <param name="opponent_Panel_List"></param>
        public void SurrenderRow(PanelList opponent_Panel_List)
        {
            for(int i =0; i < 4 ; i ++)
            {
                TransferPanel(opponent_Panel_List, 0);
            }
        }
        //Returns the amount of panels currently in the list
        public int Count
        {
            get
            {
                return panels.Count;
            }
        }
        //Returns a panel at the given index
        public GameObject this[int index]
        {
            get
            {
                return panels[index];
            }
        }
        //returns the index of a panel at the given coordinates
        public bool FindIndex(Vector2 position, out int index)
        {
            for(int i = 0; i < panels.Count; i++)
            {
                var panel = panels[i].GetComponent<PanelBehaviour>();
                if(panel.Position == position)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
    }


}