using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBehaviour : MonoBehaviour {
    private Rigidbody _rigidbody;
    private string _owner;
    private string _axis;
	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.parent.forward);
	}
	private void InitializeAxis()
    {
        if(_owner == "Player1")
        {
            _axis = "Special1";
        }
        else
        {
            _axis = "Special2";
        }
    }
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp(_axis))
        {

        }
	}
}
