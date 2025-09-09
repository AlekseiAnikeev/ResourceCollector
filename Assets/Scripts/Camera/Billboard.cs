using UnityEngine;

namespace Camera
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _mainCamera;
        private Quaternion _initialRotation;

        private void Awake()
        {
            if (_mainCamera == null)
                _mainCamera = UnityEngine.Camera.main;

            _initialRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            if (_mainCamera != null)
            {
                transform.rotation = _mainCamera.transform.rotation * _initialRotation;
            }
        }
    }
}