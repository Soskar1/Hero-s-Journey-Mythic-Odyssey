using UnityEngine;

namespace HerosJourney.Core.Entities
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _groundLayer;

        public bool CheckForGround()
        {
            Collider[] overlapInfo = Physics.OverlapSphere(_groundCheckPoint.position, _radius, _groundLayer);

            if (overlapInfo.Length > 0)
                return true;

            return false;
        }
    }
}