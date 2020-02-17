using System.Collections;
using System.Collections.Generic;
using Lodis;
using UnityEngine;
using Event = Lodis.Event;

public class PlayerAttackBehaviour : MonoBehaviour
{
	[SerializeField]
	private GunBehaviour _gun;
	private float _time;
	[SerializeField]
	private float _hitboxActiveTime;
	private bool _meleeHitboxActive;
    [SerializeField]
    private GameObject _weapon;
	[SerializeField]
	private GameObject _weaponHitbox;
    [SerializeField]
    private Animator animator;
	private PlayerMovementBehaviour player;
    private RaycastHit _interactionRay;
    private GameObject _currentBlock;
	[SerializeField] private Event _onInteractPressed;
	// Use this for initialization
	void Start ()
	{
		player = GetComponent<PlayerMovementBehaviour>();
        _interactionRay = new RaycastHit();
	}

	public void Interact()
	{
		if (player.canMove)
		{
            int layerMask = 1 << 9;
            if(Physics.Raycast(transform.position,transform.forward,out _interactionRay,5,layerMask))
            {
                _currentBlock = _interactionRay.transform.gameObject;
                if(_currentBlock.CompareTag("Panel"))
                {
                    return;
                }
                _currentBlock.SendMessage("ActivateSpecialAction");
            }
            
		}
	}
	
	public void FireGun()
	{
		if (player.canMove)
		{
			_gun.FireBullet();
		}
	}
	// Update is called once per frame
	void Update () {
		if (_meleeHitboxActive)
		{
			_weaponHitbox.SetActive(true);
			if (Time.time >=_time )
			{
                _weapon.SetActive(false);
				_weaponHitbox.SetActive(false);
				_meleeHitboxActive = false;
			}
		}
	}
}
