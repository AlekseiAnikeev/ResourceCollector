using UnityEngine;

namespace Camera
{
    public class Billboard : MonoBehaviour
    {
        private UnityEngine.Camera _mainCamera;
        private Quaternion _initialRotation;

        private void Start()
        {
            _mainCamera = UnityEngine.Camera.main;
            _initialRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            if(_mainCamera != null)
            {
                transform.rotation = _mainCamera.transform.rotation * _initialRotation;
            }
        }
    }
}