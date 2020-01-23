using System;
using System.Collections;
using System.Collections.Generic;
using Lodis;
using Lodis.GamePlay;
using TMPro.EditorUtilities;
using UnityEngine;

public class AIMovementBehaviour : MonoBehaviour
{
	private PlayerMovementBehaviour _moveScript;

	private GameObject goal;
	// Use this for initialization
	void Start ()
	{
		_moveScript = GetComponent<PlayerMovementBehaviour>();
	}
	public bool FindSafeSpot()
	{
		foreach (GameObject panel in _moveScript.Panels)
		{
			PanelBehaviour temp = panel.GetComponent<PanelBehaviour>();
			Vector2 panelPosition = new Vector2(temp.Position.x,temp.Position.y);
			if (panelPosition.x != _moveScript._currentPanel.GetComponent<PanelBehaviour>().Position.x)
			{
				continue;
			}
			foreach (GameObject bullet in GridBehaviour.bulletList)
			{
				BulletBehaviour tempBullet = bullet.GetComponent<BulletBehaviour>();
				Vector2 BulletPosition = new Vector2(temp.Position.x,temp.Position.y);
				if (BulletPosition.y == panelPosition.y)
				{
					continue;
				}
				goal = panel;
				return true;
			}
		}
		return false;
	}
	public void FindBestPath()
	{
		
	}

	private void Update()
	{
		
	}
}
