using UnityEngine;
using System;

namespace HerosJourney.Core.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class HighJump : MonoBehaviour, IJump
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _force;
        
        public void Jump() => _rigidbody.AddForce(Vector2.up * _force, ForceMode.Impulse);
    }
}