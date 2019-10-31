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
        private float timer;
        private bool TimerSet;
        private Material _panelMat;
        private void Start()
        {
            CurrentColor = new Color();
            
            TimerSet = false;
        }

        private void Awake()
        {
            
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
                _panelMat.color = SelectionColor;
                return;
            }
            UpdateColor();
        }

        private void RevertToOwnerColor(float RevertTime)
        {
            if (_attackHighlight && TimerSet == false)
            {
                timer = Time.time + RevertTime;
                TimerSet = true;
                return;
            }
            else if (Time.time >= timer && _attackHighlight)
            {
                UpdateColor();
                _attackHighlight = false;
                TimerSet = false;
            }
        }
        
        public void UpdateColor()
        {
            _panelMat = GetComponent<MeshRenderer>().material;
            if(_attackHighlight)
             {
                 _panelMat.color = Color.yellow;
                 CurrentColor = Color.yellow;
             }
            else if (Owner == "Player1")
            {
                if (_player1Mat == null)
                {
                    _panelMat.color = Color.black;
                    CurrentColor = Color.red;
                }
                else
                {
                    _panelMat.color = _player1Mat.color;
                    CurrentColor = _player1Mat.color;
                }
            }
            else if (Owner == "Player2")
            {
                if (_player2Mat == null)
                {
                    _panelMat.color = Color.black;
                    CurrentColor = Color.cyan;
                }
                else
                {
                    _panelMat.color = _player2Mat.color;
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
                RevertToOwnerColor(.2f);
                UpdateColor();
            }
        }

        private void Update()
        {
            RevertToOwnerColor(1f);
            
        }
    }
    
}
