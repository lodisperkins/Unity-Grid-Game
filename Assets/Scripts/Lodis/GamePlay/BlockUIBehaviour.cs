using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUIBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject _block;
	// Update is called once per frame
	void Update () {
		transform.LookAt(_block.transform.forward);
       
	}
}
