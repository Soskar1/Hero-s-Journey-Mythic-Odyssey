using UnityEngine;

namespace HerosJourney.Core
{
    public class ThirdPersonLook : MonoBehaviour
    {
        [SerializeField] private float _turnSmoothTime;
        private float _turnSmoothVelocity = 1f;

        
        private Transform _transform;

        private void Awake() 
        { 
            _transform = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Rotate(float targetRotation)
        {
            float rotation = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, _turnSmoothTime);

            _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }
}
