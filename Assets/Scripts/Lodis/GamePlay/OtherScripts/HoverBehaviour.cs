using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBehaviour : MonoBehaviour {
    [SerializeField]
    private Vector3 startingPos;
    [SerializeField]
    private Vector3 endPos;
    [SerializeField]
    private float intensity;
    [SerializeField]
    private float lerpVal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 hoverPos = (Vector3.up * Mathf.Cos(Time.time))* intensity;
        transform.position = new Vector3(transform.position.x, hoverPos.y, transform.position.z) ;
	}
}
