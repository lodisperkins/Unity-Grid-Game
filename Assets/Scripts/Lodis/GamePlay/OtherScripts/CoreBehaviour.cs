using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class CoreBehaviour : MonoBehaviour
    {

        [SerializeField] private Material _coreMaterial;
        [SerializeField]
        private Event _onExplosion;
        private Color _materialColor;
        [SerializeField]
        private List<GameObject> _pieces;
        [SerializeField]
        private HealthBehaviour _healthRef;
        private IntVariable _healthStamp;
        System.Random random;
        [SerializeField]
        private float _explosionForce;
        [SerializeField]
        private float _explosionRadius;
        [SerializeField]
        private float _upwardsModifier;
        private int currentPiece;
        // Use this for initialization
        void Start()
        {
            _coreMaterial.color = Color.white;
            _materialColor = Color.white;
            _healthStamp = IntVariable.CreateInstance(_healthRef.health.Val - 10);
            currentPiece = 0;
        }
        public void BlowPieceAway()
        {
            if(currentPiece < _pieces.Count)
            {
                GameObject temp = _pieces[currentPiece];
                _pieces.Remove(temp);
                temp.GetComponent<Rigidbody>().isKinematic = false;
                temp.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, temp.transform.position, _explosionRadius, _upwardsModifier, ForceMode.Impulse);
                _onExplosion.Raise(gameObject);
                Destroy(temp, 3);
                currentPiece++;
            }
        }
        private void OnTriggerStay(Collider other)
        {
            StartCoroutine(Flash());
        }
        //This makes the core flash for a few seconds when hit
        private IEnumerator Flash()
        {
            for (var i = 0; i < 5; i++)
            {
                _coreMaterial.color = Color.yellow;
                yield return new WaitForSeconds(.1f);
                _coreMaterial.color = _materialColor;
                yield return new WaitForSeconds(.1f);
            }
        }

        private void Update()
        {
            if (_healthRef.health.Val <= _healthStamp.Val)
            {
                _healthStamp.Val = _healthRef.health.Val - 10;
                BlowPieceAway();
            }
        }
    }
}
