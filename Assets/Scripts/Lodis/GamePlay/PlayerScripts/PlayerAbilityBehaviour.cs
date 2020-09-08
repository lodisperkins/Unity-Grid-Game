using Lodis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityBehaviour : MonoBehaviour {
	[SerializeField]
	private List<GameObject> _abilities;
	[SerializeField]
	private PlayerSpawnBehaviour _spawnScript;
	[SerializeField]
	private GameObjectList _blockList;
	private PlayerAttackBehaviour playerAttackScript;
	[SerializeField]
	private List<string> _axis;
	public int player;
	// Use this for initialization
	void Start () {
		playerAttackScript = GetComponent<PlayerAttackBehaviour>();
		_abilities = new List<GameObject>();
		InitializeAbilities();
	}
	private void InitializeAbilities()
    {
		foreach(GameObject block in _blockList)
        {
			BlockBehaviour blockScript = block.GetComponent<BlockBehaviour>();
			if(blockScript.Type == "Special")
            {
				return;
            }
			GameObject ability = Instantiate(blockScript.specialFeature.gameObject);
			ability.SetActive(false);
			ability.GetComponent<IUpgradable>().UpgradePlayer(GetComponent<PlayerAttackBehaviour>());
			_abilities.Add(ability);
        }
    }
	public void UseAbility(int index)
    {
		
		if (_abilities[index] == null)
		{
			BlockBehaviour blockScript = _blockList[index].GetComponent<BlockBehaviour>();
			GameObject ability = Instantiate(blockScript.specialFeature.gameObject);
			
			_abilities[index] = ability;
		}
		if (_spawnScript.CheckMaterial((int)playerAttackScript.weaponUseAmount))
        {
			_abilities[index].GetComponent<IUpgradable>().UpgradePlayer(playerAttackScript);
			Debug.Log("hold");
			_spawnScript.BuyItem((int)playerAttackScript.weaponUseAmount);
			_abilities[index].SetActive(true);
			_abilities[index].GetComponent<IUpgradable>().ActivatePowerUp();
		}
		
    }
	public void DisableAbility(int index)
    {
		if(_abilities[index] != null)
        {
			_abilities[index].SetActive(false);
			_abilities[index].GetComponent<IUpgradable>().DeactivatePowerUp();
		}
		
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("SelectBlock" + player + "(Y)") != 1)
        {
			DisableAbility(2);
        }
		if(Input.GetAxis("SelectBlock" + player + "(Y)") != -1)
        {
			DisableAbility(1);
		}
		if (Input.GetAxis("SelectBlock" + player + "(X)") != -1)
		{
			DisableAbility(0);
		}
	}
}
