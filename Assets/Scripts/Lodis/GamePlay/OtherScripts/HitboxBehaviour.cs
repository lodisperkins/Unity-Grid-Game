using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lodis;
public class HitboxBehaviour : MonoBehaviour {
    private Collider collider;
    [SerializeField]
    private bool activeByDefault;
    [SerializeField]
    private int damageVal;
    [SerializeField]
    private bool doesKnockback;
    [SerializeField]
    private bool stunsOpponent;
    [SerializeField]
    private float stunTime;
    [SerializeField]
    private UnityEvent onEnabled;
    [SerializeField]
    private UnityEvent onDisabled;
	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
		if(!activeByDefault)
        {
            collider.enabled = false;
        }
	}
    IEnumerator MakeActiveTemporarily(float time)
    {
        collider.enabled = true;
        onEnabled.Invoke();
        yield return new WaitForSeconds(time);
        collider.enabled = false;
        onDisabled.Invoke();
    }
	public void MakeActive(float time = 0)
    {
        if(time <= 0)
        {
            collider.enabled = true;
            onEnabled.Invoke();
        }
        else if(!collider.enabled)
        {
            StartCoroutine(MakeActiveTemporarily(time));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        HealthBehaviour objectHealth = other.GetComponent<HealthBehaviour>();
        if(objectHealth != null)
        {
            objectHealth.takeDamage(damageVal);
            if(doesKnockback)
            {
                Vector3 direction = other.transform.position - transform.position;
                KnockBackBehaviour knockBackScript = other.gameObject.GetComponent<KnockBackBehaviour>();
                if (knockBackScript != null)
                {
                    knockBackScript.KnockBack(direction, 100, stunTime);
                }
            }
            else if(stunsOpponent && other.gameObject.CompareTag("Player"))
            {
                BlackBoard.grid.StunPlayer(stunTime, other.gameObject.name);
            }
        }
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
