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
    [SerializeField]
    private GameObject normalBullet;
    [SerializeField]
    private GameObject chargeBullet;
    private float chargeTimer;
    [SerializeField]
    private float timeUntilCharged;
    [SerializeField]
    private bool charged;
    [SerializeField]
    private GameObject chargeParticles;
    [SerializeField]
    private AudioSource chargingAudio;
    [SerializeField]
    private string shootAxis;
    private bool charging;
    private AudioSource tempSource;
    public int weaponUseAmount;
    private IUpgradable secondaryWeapon;
    private bool willInteract;
    [SerializeField] private SliderBehaviour sliderScript;
    [SerializeField] private IntVariable altAmmoAmount;
    // Use this for initialization
    void Start ()
	{
		player = GetComponent<PlayerMovementBehaviour>();
        _interactionRay = new RaycastHit();
        charged = false;
        InitializeBlackBoard();
	}
    public void InitializeBlackBoard()
    {
        altAmmoAmount.Val = 0;
        if (name == "Player1")
        {
            BlackBoard.altAmmoAmountP1 = altAmmoAmount;
        }
        else
        {
            BlackBoard.altAmmoAmountP2 = altAmmoAmount;
        }
    }
    public void TryInteract()
    {
        willInteract = true;
    }
	public void Interact()
	{
		if (player.canMove && willInteract)
		{
            int layerMask = 1 << 9;
            if(Physics.Raycast(transform.position,transform.forward,out _interactionRay,2,layerMask))
            {
                _currentBlock = _interactionRay.transform.gameObject;
                if(_currentBlock.CompareTag("Panel"))
                {
                    willInteract = false;
                    return;
                }
                _currentBlock.SendMessage("ActivateSpecialAction");
            }
		}
        willInteract = false;
    }
    public void EquipSecondaryWeapon()
    {
        willInteract = false;
        if (player.canMove)
        {
            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, transform.forward, out _interactionRay, 2, layerMask))
            {
                _currentBlock = _interactionRay.transform.gameObject;
                if (_currentBlock.CompareTag("Panel"))
                {
                    return;
                }
                _currentBlock.SendMessage("UpgradePlayer",this);
                sliderScript.gameObject.SetActive(true);
                sliderScript.Bar.maxValue = weaponUseAmount;
                sliderScript.fillImage.color = secondaryWeapon.displayColor;
            }
        }
    }
    private void ChargeGun()
    {
        charged = true;
        chargeParticles.SetActive(true);
    }
	public void FireGun()
	{
		if (player.canMove)
		{
            _gun.ChangeBullet(normalBullet);
            _gun.FireBullet();
		}
	}
    public void FireChargeGun()
    {
        if(charged)
        {
            _gun.ChangeBullet(chargeBullet);
            _gun.FireBullet();
            charged = false;
            chargeParticles.SetActive(false);
        }
        
    }
    public void SetSecondaryWeapon(IUpgradable weapon, int useAmount)
    {
        secondaryWeapon = weapon;
        altAmmoAmount.Val  = useAmount;
    }
    public void UseSecondaryWeapon()
    {
        if(altAmmoAmount.Val > 0)
        {
            secondaryWeapon.PlayerAttack();
            altAmmoAmount.Val--;
           
        }
        else if (secondaryWeapon != null)
        {
            sliderScript.gameObject.SetActive(false);
            secondaryWeapon.DetachFromPlayer();
            secondaryWeapon = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(secondaryWeapon != null)
        {
            secondaryWeapon.ResolveCollision(other.gameObject); 
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
        if(Input.GetButtonDown(shootAxis))
        {
            tempSource = Instantiate(chargingAudio);
        }
        else if(Input.GetButtonUp(shootAxis))
        {
            Destroy(tempSource);
        }
	}
}
