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
    private MeshRenderer flashRenderer;
    private MeshRenderer normalRenderer;
    // Use this for initialization
    void Start () {
        flashRenderer = flashObject.GetComponent<MeshRenderer>();
        normalRenderer = normalObject.GetComponent<MeshRenderer>();
    }
    private IEnumerator FlashMeshrenderer()
    {
        if (flashObject != null && normalObject != null)
        {
            for (var i = 0; i < numberOfFlashes; i++)
            {
                normalRenderer.enabled = false;
                flashRenderer.enabled = true;
                yield return new WaitForSeconds(flashSpeed);
                normalRenderer.enabled = true;
                flashRenderer.enabled = false;
                yield return new WaitForSeconds(flashSpeed);
            }
            normalRenderer.enabled = true;
        }
        else
        {
            for (var i = 0; i < numberOfFlashes; i++)
            {
                normalObject.GetComponent<MeshRenderer>().enabled = false;
                yield return new WaitForSeconds(flashSpeed);
                normalObject.GetComponent<MeshRenderer>().enabled = true;
                yield return new WaitForSeconds(flashSpeed);
            }
            normalRenderer.enabled = true;
        }
    }
    private IEnumerator FlashGameObject()
    {
        if (flashObject != null && normalObject != null)
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
        else
        {
            for (var i = 0; i < numberOfFlashes; i++)
            {
                normalObject.SetActive(false);
                yield return new WaitForSeconds(flashSpeed);
                normalObject.SetActive(true);
                yield return new WaitForSeconds(flashSpeed);
            }
            normalObject.SetActive(true);
        }
    }
    public void StartFlashing()
    {
        if(normalRenderer == null)
        {
            StartCoroutine(FlashGameObject());
            return;
        }
        StartCoroutine(FlashMeshrenderer());
    }
    // Update is called once per frame
    void Update () {
		
	}
}
