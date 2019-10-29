using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lodis
{
    public class InputAxisBehaviour : MonoBehaviour
    {

        [SerializeField]
        private string HorizontalAxis;
        [SerializeField]
        private string VerticalAxis;
        [SerializeField]
        private Vector2Variable Direction;
        [SerializeField]
        private bool isRawInput;

        private float timer;
        private float StopRollTime;
        private bool _isMoving;

        private void Start()
        {
            StopRollTime = .3f;
            timer = StopRollTime + Time.time;
        }

        public void SendAnimationMessage()
        {
            _isMoving = CheckIfNotMoving();
            if (_isMoving)
            {
                SendMessage("EnableMoveAnimation");
            }
            else
            {
                SendMessage("DisableMoveAnimation");
            }
        }

        public bool InRange(float val,float num1, float num2)
        {
            return val >= num1 && val <= num2;
        }

        public bool CheckIfNotMoving()
        {
            if (InRange(Direction.Val.magnitude, 0, .2f))
            {
                if (Time.time >= timer)
                {
                    return false;
                }
            }
            else
            {
                timer = StopRollTime + Time.time;
            }
            return true;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (isRawInput)
            {
                Direction.X = Input.GetAxisRaw(HorizontalAxis);
                Direction.Y = Input.GetAxisRaw(VerticalAxis);
            }
            else
            {
                Direction.X = Input.GetAxis(HorizontalAxis);
                Direction.Y = Input.GetAxis(VerticalAxis);
            }
            SendAnimationMessage();
        }
    }
}
