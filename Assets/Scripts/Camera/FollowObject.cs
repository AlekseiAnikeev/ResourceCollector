using UnityEngine;

namespace Camera
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _positionOffset = new (0, 2f, 0);

        private void LateUpdate()
        {
            if(_target != null)
            {
                transform.position = _target.position + _positionOffset;
            }
        }
    }
}