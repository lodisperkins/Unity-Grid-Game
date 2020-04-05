using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay.GridScripts;
using UnityEngine;

namespace Lodis.GamePlay.AIFolder
{
	
	public class AIMovementBehaviour : MonoBehaviour
    {
    	private PlayerMovementBehaviour _moveScript;
    	private List<PanelBehaviour> _currentPath;
    	private PanelBehaviour _goal;
        [SerializeField]
        private PanelBehaviour _testGoal;
        [SerializeField]
        private float _movementDelay;

       

        [SerializeField] private GameObjectList _enemyBulletList;
        public PanelBehaviour Goal
        {
	        get { return _goal; }
	        set { _goal = value; }
        }

        private Condition SafeSpotCheck;
        private Condition NeighboorCheck;
    	private bool shouldDodge;

        private PanelBehaviour _currentPanelInPath;
        public GameObjectList EnemyBulletList
       {
        get { return _enemyBulletList; }
       }
    	// Use this for initialization
    	void Start ()
    	{
    		_moveScript = GetComponent<PlayerMovementBehaviour>();
    		_currentPath = new List<PanelBehaviour>();
    		GetComponent<InputButtonBehaviour>().enabled = false;
    		GetComponent<InputAxisBehaviour>().enabled = false;
            SafeSpotCheck += CheckIfSafeSpot;
            NeighboorCheck += CheckIfNeighboor;
        }

        public bool CheckIfSafeSpot(object[] arg)
        {
	        GameObject temp = (GameObject)arg[0];
	        Vector2 position = temp.GetComponent<PanelBehaviour>().Position;
	        if (position.x == _moveScript.Position.x || position.y == _moveScript.Position.y)
	        {
		        return true;
	        }
	        return false;
        }

        public bool CheckIfNeighboor(object[] arg)
        {
	        GameObject temp = (GameObject)arg[0];
	        Vector2 position = temp.GetComponent<PanelBehaviour>().Position;
	        Vector2 displacdementX = new Vector2(1,0);
	        Vector2 displacdementY = new Vector2(0,1);
	        if (position == _currentPanelInPath.Position+ displacdementX || position == _currentPanelInPath.Position -displacdementX)
	        {
		        return true;
	        }
	        if (position == _currentPanelInPath.Position +displacdementY || position == _currentPanelInPath.Position - displacdementY)
	        {
		        return true;
	        }
	        return false;
        }
        public bool CheckIfNeighboor(PanelBehaviour arg)
        {
            Vector2 position = arg.Position;
            Vector2 displacementX = new Vector2(1, 0);
            Vector2 displacementY = new Vector2(0, 1);
            if (position == _moveScript.Position + displacementX || position == _moveScript.Position - displacementX)
            {
                return true;
            }
            if (position == _moveScript.Position + displacementY || position == _moveScript.Position - displacementY)
            {
                return true;
            }
            return false;
        }
        public bool CheckIfNeighboor(PanelBehaviour arg , int range)
        {
            Vector2 position = arg.Position;
            Vector2 displacdementX = new Vector2(range, 0);
            Vector2 displacdementY = new Vector2(0, range);
            if (position == _moveScript.Position + displacdementX || position == _moveScript.Position - displacdementX)
            {
                return true;
            }
            if (position == _moveScript.Position + displacdementY || position == _moveScript.Position - displacdementY)
            {
                return true;
            }
            return false;
        }
        public bool FindSafeSpot()
        {
	        List<PanelBehaviour> moveSpots = new List<PanelBehaviour>();
	        if (_moveScript.Panels.GetPanels(SafeSpotCheck, out moveSpots))
	        {
		        for (int i =0; i < _enemyBulletList.Objects.Count;i++)
		        {
			        
			        if (_enemyBulletList[i].GetComponent<BulletBehaviour>().currentPanel != null)
			        {
				        Vector2 bulletPosition = _enemyBulletList[i].GetComponent<BulletBehaviour>().currentPanel.Position;
                        for (int j = 0; j < moveSpots.Count; j++)
                        {
                            Vector2 panelPosition = moveSpots[j].Position;
                            if (bulletPosition.y == panelPosition.y)
                            {
                                moveSpots.RemoveAt(j);
                                j--;
                                continue;
                            }
                            if (bulletPosition.x == panelPosition.x)
                            {
                                moveSpots.RemoveAt(j);
                                j--;
                            }
                        }
                        _enemyBulletList.Objects.RemoveAt(i);
                        i--;
			        }
			        
		        }
	        }
    		if (moveSpots.Count == 0)
    		{
    			return false;
    		}
    		_goal = FindClosestSafeSpot(moveSpots);
    		return true;
    	}
        public PanelBehaviour FindClosestSafeSpot(List<PanelBehaviour> moveSpots)
        {
            for(int i = 0; i<4; i++)
            {
                foreach(PanelBehaviour panel in moveSpots)
                {
                    if(CheckIfNeighboor(panel,i))
                    {
                        return panel;
                    }
                }
            }
            return moveSpots[0];
        }
    	public float Manhattan(PanelBehaviour panel)
    	{
    		return Math.Abs(_goal.Position.x - panel.Position.x) + Math.Abs(_goal.Position.y - panel.Position.y);
    	}
    		
    	public List<PanelBehaviour> FindNeighbors()
        {
            List<PanelBehaviour> panelsInRange = new List<PanelBehaviour>();
            if (_moveScript.Panels.GetPanels(NeighboorCheck, out panelsInRange))
            {
	            return panelsInRange;
            }
            Debug.Log("Couldn't find neighboors");
            return new List<PanelBehaviour>();
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

        public void ReconstructPath()
        {
	        _currentPath =new List<PanelBehaviour>();
	        PanelBehaviour temp = _goal;
	        while (temp != _moveScript._currentPanel.GetComponent<PanelBehaviour>())
	        {
		        _currentPath.Insert(0,temp);
		        temp = temp.previousPanel;
	        }
        }
    	public void FindBestPath()
    	{
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
    			if (panel == _goal)
    			{
    				ReconstructPath();
                    return;
    			}
    			openList.Remove(panel);
    			closedList.Add(panel);
                _currentPanelInPath = panel;
    			foreach (PanelBehaviour neighbor in FindNeighbors())
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
                        neighbor.previousPanel = panel;
    					openList.Add(neighbor);
    				}
    			}
    		}
    	}
    
    	public IEnumerator Move()
    	{
    		Debug.Log("Tried move");
    		for (int i =0; i< _currentPath.Count; i++)
            {
	            PanelBehaviour panel = _currentPath[i];
    			if (panel.Position.x < _moveScript.Position.x)
    			{
    				_moveScript.MoveLeft();
    			}
    
    			else if (panel.Position.x > _moveScript.Position.x)
    			{
    				_moveScript.MoveRight();
    			}
    
    			else if (panel.Position.y > _moveScript.Position.y)
    			{
    				_moveScript.MoveUp();
    			}
    
    			else if (panel.Position.y < _moveScript.Position.y)
    			{
    				_moveScript.MoveDown();
    			}
                yield return new WaitForSeconds(_movementDelay);
    		}
    	}
    	public void Dodge()
    	{
            if(_testGoal == null)
            {
                FindSafeSpot();
            }
            else
            {
                _goal = _testGoal;
            }
            Debug.ClearDeveloperConsole();
            Debug.Log("goal is " + _goal.Position);
    		FindBestPath();
            StartCoroutine(Move());
        }
    	private void Update()
    	{
    		GetComponent<InputButtonBehaviour>().enabled = false;
    		GetComponent<InputAxisBehaviour>().enabled = false;
    	}


}
}
