using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUIBehaviour : MonoBehaviour {

	public GameObject Target;
	public Vector3 targetPos;

	// Use this for initialization
	void Start () {
		targetPos = Camera.main.WorldToScreenPoint(Target.transform.position);
	}
 
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = Target.transform.position;
		transform.LookAt(targetPos);
       
	}
}
