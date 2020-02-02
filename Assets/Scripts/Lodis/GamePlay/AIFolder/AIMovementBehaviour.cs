using System;
using System.Collections;
using System.Collections.Generic;
using Lodis;
using Lodis.GamePlay;
using UnityEngine;

public class AIMovementBehaviour : MonoBehaviour
{
	private PlayerMovementBehaviour _moveScript;
	private List<PanelBehaviour> currentPath;
	private PanelBehaviour goal;
	// Use this for initialization
	void Start ()
	{
		_moveScript = GetComponent<PlayerMovementBehaviour>();
		currentPath = new List<PanelBehaviour>();
		GetComponent<InputButtonBehaviour>().enabled = false;
		GetComponent<InputAxisBehaviour>().enabled = false;
	}
	public bool FindSafeSpot()
	{
		List<GameObject> moveSpots = new List<GameObject>();
		foreach (GameObject panel in _moveScript.Panels)
		{
			if (panel.GetComponent<PanelBehaviour>().Position.x == _moveScript.Position.x)
			{
				moveSpots.Add(panel);
			}
		}

		foreach (GameObject bullet in GridBehaviour.bulletList)
		{
			Debug.Log(GridBehaviour.bulletList.Objects.Count);
			for (int i = 0; i < moveSpots.Count; i++)
			{
				if (bullet.GetComponent<BulletBehaviour>().currentPanel.Position.y == moveSpots[i].GetComponent<PanelBehaviour>().Position.y)
				{
					moveSpots.RemoveAt(i);
					i--;
				}
			}
		}

		if (moveSpots.Count == 0)
		{
			return false;
		}

		goal = moveSpots[0].GetComponent<PanelBehaviour>();
		return true;
	}

	public float Manhattan(PanelBehaviour panel)
	{
		return Math.Abs(goal.Position.x - panel.Position.x) + Math.Abs(goal.Position.y - panel.Position.y);
	}
		
	public List<PanelBehaviour> FindNeighbors(PanelBehaviour currentPanel )
    {
        //Creates a new dictionary to store the blocks in range
        List<PanelBehaviour> panelsInRange = new List<PanelBehaviour>();
        //Used to find the position the block can be placed
        Vector2 DisplacementX = new Vector2(1, 0);
        Vector2 DisplacementY = new Vector2(0, 1);
        //Loops through all panels to find those whose position is the
        //player current position combined with x or y displacement
        foreach (GameObject panel in _moveScript.Panels)
        {
            var coordinate = panel.GetComponent<PanelBehaviour>().Position;
            if ((currentPanel.Position + DisplacementX) == coordinate)
            {
                panelsInRange.Add(panel.GetComponent<PanelBehaviour>());
            }
            else if ((currentPanel.Position - DisplacementX) == coordinate)
            {
                panelsInRange.Add(panel.GetComponent<PanelBehaviour>());
            }
            else if ((currentPanel.Position + DisplacementY) == coordinate)
            {
                panelsInRange.Add(panel.GetComponent<PanelBehaviour>());
            }
            else if ((currentPanel.Position - DisplacementY) == coordinate)
            {
                panelsInRange.Add(panel.GetComponent<PanelBehaviour>());
            }
        }

        return panelsInRange;
    }
	public List<PanelBehaviour>SortPanels(List<PanelBehaviour> nodelist)
	{
		PanelBehaviour temp;
		for (int i = 0; i < nodelist.Count; i++)
		{
			for (int j = 0; j < nodelist.Count; j++)
			{
				if (nodelist[i].F > nodelist[j].F)
				{
					temp = nodelist[i];
					nodelist[i] = nodelist[j];
					nodelist[j] = temp;
				}
			}
		}

		return nodelist;
	}
	public void FindBestPath()
	{
		Debug.Log("Tried a star");
		PanelBehaviour panel;
		PanelBehaviour startPanel = _moveScript.CurrentPanel.GetComponent<PanelBehaviour>();
		List<PanelBehaviour> openList = new List<PanelBehaviour>();
		openList.Add(startPanel);
		List<PanelBehaviour> closedList = new List<PanelBehaviour>();
		_moveScript._currentPanel.GetComponent<PanelBehaviour>().F =
			Manhattan(_moveScript.CurrentPanel.GetComponent<PanelBehaviour>());
		while (openList.Count > 0)
		{
			openList = SortPanels(openList);
			panel = openList[0];
			if (panel == goal)
			{
				currentPath = closedList;
			}
			openList.Remove(panel);
			closedList.Add(panel);
			foreach (PanelBehaviour neighbor in FindNeighbors(panel))
			{
				if (closedList.Contains(neighbor) || openList.Contains(neighbor))
				{
					continue;
				}
				else if (neighbor.Occupied)
				{
					continue;
				}
				else
				{
					neighbor.G = panel.G + neighbor.G;
					neighbor.F = neighbor.G + Manhattan(neighbor);
					openList.Add(neighbor);
				}
			}
		}
	}

	public void Move()
	{
		Debug.Log("Tried move");
		foreach (PanelBehaviour  panel in currentPath)
		{
			if (panel.Position.x < _moveScript.Position.x)
			{
				_moveScript.MoveLeft();
			}

			if (panel.Position.x > _moveScript.Position.x)
			{
				_moveScript.MoveRight();
			}

			if (panel.Position.y > _moveScript.Position.y)
			{
				_moveScript.MoveUp();
			}

			if (panel.Position.y < _moveScript.Position.y)
			{
				_moveScript.MoveDown();
			}
		}
	}
	public void Dodge()
	{
		FindSafeSpot();
		FindBestPath();
		Move();
	}
	private void Update()
	{
		GetComponent<InputButtonBehaviour>().enabled = false;
		GetComponent<InputAxisBehaviour>().enabled = false;
	}
}
