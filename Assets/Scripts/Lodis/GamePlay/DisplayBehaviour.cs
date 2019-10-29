using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace Lodis
{
    public class DisplayBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BlockVariable playerBlock;
        private bool canUpdate;

       private RawImage _image;
        // Use this for initialization
        void Start()
        {
            _image = GetComponent<RawImage>();
            _image.color = Color.red;
        }
        public void ChangeColor()
        {
            _image.color = playerBlock.Color;
        }
        public void EnableUpdate()
        {
            canUpdate = true;
        }
        public void DisableUpdate()
        {
            canUpdate = false;
        }
        private void Update()
        {
            if(canUpdate)
            {
                ChangeColor();
            }
        }
    }
}
