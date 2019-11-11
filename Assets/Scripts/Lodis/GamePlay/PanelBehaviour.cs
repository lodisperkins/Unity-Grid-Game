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
        //the color the panel is set to by default in the editor
        private Color DefaultColor;
        //the current color the panel is being set to
        private Color _currentColor;
        //the color the panel should be highlighted when selected
        public Color SelectionColor;
        //whether or not the panel is currently being selected
        private bool _selected;
        //the reference tho the players material
        private Material _player1Mat;
        private Material _player2Mat;
        
        private bool _attackHighlight;
        private float timer;
        private bool TimerSet;
        //the current material on the panel
        private Material _panelMat;
        private void Start()
        {
            _currentColor = new Color();
            
            TimerSet = false;
        }

        private void Awake()
        {
            
        }
        //one set calls the highlight panel fucntion with current status of the _selected variable
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
            //highlights the panel if a bullet passes through it 
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
        //initializes the panel with the players materials and colors itself accordingly
        public void Init(Material player1Mat, Material player2Mat)
        {
            _player1Mat = player1Mat;
            _player2Mat = player2Mat;
            UpdateColor();
        }
        //Highlights the panel if the value passed in is true
        private void HighlightPanel(bool isSelected)
        {
            if (isSelected)
            {
                _panelMat.color = SelectionColor;
                return;
            }
            UpdateColor();
        }
        //(not working)changes the panel back to the owners color after a certain amount of time
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
                _attackHighlight = false;
                TimerSet = false;
                UpdateColor();
            }
        }
        //Updates the coor of the panel to be that of its current owner
        public void UpdateColor()
        {
            _panelMat = GetComponent<MeshRenderer>().material;
            if(_attackHighlight)
             {
                 _panelMat.color = Color.yellow;
                 _currentColor = Color.yellow;
             }
            else if (Owner == "Player1")
            {
                if (_player1Mat == null)
                {
                    _panelMat.color = Color.black;
                    _currentColor = Color.red;
                }
                else
                {
                    _panelMat.color = _player1Mat.color;
                    _currentColor = _player1Mat.color;
                }
            }
            else if (Owner == "Player2")
            {
                if (_player2Mat == null)
                {
                    _panelMat.color = Color.black;
                    _currentColor = Color.cyan;
                }
                else
                {
                    _panelMat.color = _player2Mat.color;
                    _currentColor = _player2Mat.color;
                }
            }
             
        }
        //changes the current owner of the panel to be the name of the item passsed in
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
            
        }
    }
    
}
