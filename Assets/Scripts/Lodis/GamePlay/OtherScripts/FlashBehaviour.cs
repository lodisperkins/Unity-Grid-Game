using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject normalObject;
    [SerializeField]
    private GameObject flashObject;
    [SerializeField]
    private int numberOfFlashes;
    [SerializeField]
    private float flashSpeed;
	// Use this for initialization
	void Start () {
		
	}
    private IEnumerator Flash()
    {
        for (var i = 0; i < numberOfFlashes; i++)
        {
            normalObject.SetActive(false);
            flashObject.SetActive(true);
            yield return new WaitForSeconds(flashSpeed);
            normalObject.SetActive(true);
            flashObject.SetActive(false);
            yield return new WaitForSeconds(flashSpeed);
        }
        normalObject.SetActive(true);
    }
    public void StartFlashing()
    {
        StartCoroutine(Flash());
    }
    // Update is called once per frame
    void Update () {
		
	}
}
