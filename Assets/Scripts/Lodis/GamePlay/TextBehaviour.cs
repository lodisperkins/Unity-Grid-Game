using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Lodis
{
    public class TextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private IntVariable Materials;
        // Update is called once per frame
        void Update()
        {
            GetComponent<Text>().text = "Materials: " + System.Convert.ToString(Materials.Val);
        }
    }
}
