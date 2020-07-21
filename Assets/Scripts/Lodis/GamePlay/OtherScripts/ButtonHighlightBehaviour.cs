using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonHighlightBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Text text;
	// Use this for initialization
	void Start () {
		
	}
    // Update is called once per frame
    void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black;
    }
}
