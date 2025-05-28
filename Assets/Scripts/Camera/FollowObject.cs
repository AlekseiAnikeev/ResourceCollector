using UnityEngine;

namespace Camera
{
    public class FollowObject : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private Vector3 _positionOffset = new Vector3(0, 2f, 0);
    
        [Header("Зависимости")]
        [SerializeField] private Transform _target;

        private void LateUpdate()
        {
            if(_target != null)
            {
                transform.position = _target.position + _positionOffset;
            }
        }
    }
}