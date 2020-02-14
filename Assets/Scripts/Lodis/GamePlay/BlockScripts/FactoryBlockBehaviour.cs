using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlockBehaviour : MonoBehaviour {
    private Lodis.PlayerSpawnBehaviour _playerSpawnScript;
    private Dictionary<string, GameObject> panelsInRange;
	// Use this for initialization
	void Start () {
		
	}
    public void FindNeighbors()
    {
        //Creates a new dictionary to store the blocks in range
        panelsInRange = new Dictionary<string, GameObject>();
        //Used to find the position the block can be placed
        Vector2 DisplacementX = new Vector2(1, 0);
        Vector2 DisplacementY = new Vector2(0, 1);
        //Loops through all panels to find those whose position is the
        //player current position combined with x or y displacement
        //    foreach (GameObject panel in _playerSpawnScript.player.Panels)
        //    {
        //        _panel = panel.GetComponent<PanelBehaviour>();
        //        _currentBlock = blocks[current_index].GetComponent<BlockBehaviour>();
        //        var coordinate = _panel.Position;
        //        if ((player.Position + DisplacementX) == coordinate)
        //        {
        //            if (_panel.CheckPanelCapacity(_currentBlock) && buildStateEnabled && !DeleteEnabled)
        //            {
        //                _panel.Selected = false;
        //                continue;
        //            }
        //            panels_in_range.Add("Forward", panel);
        //            _panel.SelectionColor = SelectionColor;
        //            _panel.Selected = true;
        //        }
        //        else if ((player.Position - DisplacementX) == coordinate)
        //        {
        //            if (_panel.CheckPanelCapacity(_currentBlock) && buildStateEnabled && !DeleteEnabled)
        //            {
        //                _panel.Selected = false;
        //                continue;
        //            }
        //            panels_in_range.Add("Behind", panel);
        //            _panel.SelectionColor = SelectionColor;
        //            _panel.Selected = true;
        //        }
        //        else if ((player.Position + DisplacementY) == coordinate)
        //        {
        //            if (_panel.CheckPanelCapacity(_currentBlock) && buildStateEnabled && !DeleteEnabled)
        //            {
        //                _panel.Selected = false;
        //                continue;
        //            }
        //            panels_in_range.Add("Above", panel);
        //            _panel.SelectionColor = SelectionColor;
        //            _panel.Selected = true;
        //        }
        //        else if ((player.Position - DisplacementY) == coordinate)
        //        {
        //            if (_panel.CheckPanelCapacity(_currentBlock) && buildStateEnabled && !DeleteEnabled)
        //            {
        //                _panel.Selected = false;
        //                continue;
        //            }
        //            panels_in_range.Add("Below", panel);
        //            _panel.SelectionColor = SelectionColor;
        //            _panel.Selected = true;
        //        }
        //    }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
