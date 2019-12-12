using System;
using System.Collections;
using System.Collections.Generic;
using Lodis;
using Lodis.GamePlay;
using UnityEngine;

public class MortarBlockBehaviour : MonoBehaviour
{
	//the grid space
	[SerializeField] private GridBehaviour _grid;
	//the block that this component is attached to
	private BlockBehaviour _block;
	//This will spawn bullets over the opponents panel
	[SerializeField]
	private GameObject _bulletEmitter;
	private GameObject _targetPanel;
	[SerializeField]
	private Vector3 _bulletEmitterPosition;
	private PanelBehaviour firstPanelFound;
	private int _yPosition;
	private Vector2 _aimOffSet;
	private Vector3 _targetPosition;
	// Use this for initialization
	void Start () {
		_block = GetComponent<BlockBehaviour>();
		AimAtOpponentPanel();
	}

	private void OnEnable()
	{
//		_block = GetComponent<BlockBehaviour>();
//		AimAtOpponentPanel();
	}
	//needs to be cleaned up
	public void AimAtOpponentPanel()
	{
		_yPosition = (int)_block.currentPanel.GetComponent<PanelBehaviour>().Position.y;
		for (int i = 0; i < _grid.CountP2; i++)
		{
			 firstPanelFound = _grid.getPanelFromP2List(i).GetComponent<PanelBehaviour>();
			int targetPositionY = (int)firstPanelFound.Position.y;
			if (targetPositionY != _yPosition)
			{
				firstPanelFound = null;
				continue;
			}
			break;
		}
		if (firstPanelFound == null)
		{
			return;
		}
		int fp = (int)firstPanelFound.GetComponent<PanelBehaviour>().Position.x;
		int cp = (int)_block.currentPanel.GetComponent<PanelBehaviour>().Position.x;
		int yPos = (int)_block.currentPanel.GetComponent<PanelBehaviour>().Position.y;
		int tp = fp + (fp - cp) - 1;
		if (tp > 9)
		{
			tp = 9;
		}
		else if(tp < 0)
		{
			tp = 0;
		}
		
		int target= _grid.getIndexFromP2List(new Vector2(tp, yPos));
		if (target == -1)
		{
			target = 0;
		}
		
		_targetPanel = _grid.getPanelFromP2List(target);
		_targetPosition = _targetPanel.transform.position + _bulletEmitterPosition;
	}
	public void FireMortar()
	{
		_bulletEmitter.GetComponent<GunBehaviour>().owner = _block.owner.name;
		Instantiate(_bulletEmitter, _targetPosition, Quaternion.Euler(90, 0, 0));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
