using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class PanelBehaviour : MonoBehaviour
    {

        //The position of the panel on the grid
        [SerializeField]
        public Vector2 Position;
        //Is true if the panel has been selected or highlighted by the player
        public string Owner;
        //Is true if a gameobjects position has been set to be the same as the panels
        public bool Occupied;
        private Color DefaultColor;
        private Color CurrentColor;
        public Color SelectionColor;
        private bool _selected;
        private Material _player1Mat;
        private Material _player2Mat;
        private bool _attackHighlight;
        private void Start()
        {
            CurrentColor = new Color();
        }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                HighlightPanel(value);
                _selected = value;

            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                _attackHighlight = true;
                UpdateColor();
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            _attackHighlight = false;
            UpdateColor();
        }

        public void Init(Material player1Mat, Material player2Mat)
        {
            _player1Mat = player1Mat;
            _player2Mat = player2Mat;
            UpdateColor();
        }
        
        private void HighlightPanel(bool isSelected)
        {
            if (isSelected)
            {
                GetComponent<MeshRenderer>().material.color = SelectionColor;
                return;
            }
            UpdateColor();
        }

        public void UpdateColor()
        {
            if(_attackHighlight)
             {
                 GetComponent<MeshRenderer>().material.color = Color.yellow;
                 CurrentColor = Color.yellow;
             }
            else if (Owner == "Player1")
            {
                
                if (_player1Mat == null)
                {
                    GetComponent<MeshRenderer>().material.color = Color.black;
                    CurrentColor = Color.red;
                }
                else
                {
                    GetComponent<MeshRenderer>().material.color = _player1Mat.color;
                    CurrentColor = _player1Mat.color;
                }
            }
            else if (Owner == "Player2")
            {
                if (_player2Mat == null)
                {
                    GetComponent<MeshRenderer>().material.color = Color.black;
                    CurrentColor = Color.cyan;
                }
                else
                {
                    GetComponent<MeshRenderer>().material.color = _player2Mat.color;
                    CurrentColor = _player2Mat.color;
                }
            }
             
        }
        public void UpdateOwner(string newOwner)
        {
            Owner = newOwner;
            if (_selected)
            {
                return;
            }
            else
            {
                UpdateColor();
            }
        }
    }
}
