using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    [RequireComponent(typeof(PlayerInput))]
    public class CameraControl : MonoBehaviour
    {
        private const string ActionMove = "Move";
        private const string ActionSpeedModifier = "SpeedModifier";

        [SerializeField] private Transform _cameraTransform;

        [SerializeField] private bool _moveWidthKeyboad;
        [SerializeField] private bool _moveWidthEdgeScrolling;
        
        [Range(0, 1f)] [SerializeField] private float _fastSpeed = 0.05f;
        [Range(0, 1f)] [SerializeField] private float _normalSpeed = 0.01f;
        [Range(0, 50f)] [SerializeField] private float _movementSensitivity = 1f;

        [Range(0, 150f)] [SerializeField] private float _edgeSize = 50f;

        [SerializeField] private Texture2D _cursorArrowUp;
        [SerializeField] private Texture2D _cursorArrowDown;
        [SerializeField] private Texture2D _cursorArrowLeft;
        [SerializeField] private Texture2D _cursorArrowRight;
        [SerializeField] private Texture2D _cursorArrowUpLeft;
        [SerializeField] private Texture2D _cursorArrowDownLeft;
        [SerializeField] private Texture2D _cursorArrowUpRight;
        [SerializeField] private Texture2D _cursorArrowDownRight;

        private Vector3 _newPosition;
        private Vector3 _dragStartPosition;
        private Vector3 _dragCurrentPosition;

        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _speedModifierAction;

        private float _movementSpeed;
        private bool _isCursorSet;

        private CursorArrow _currentCursor = CursorArrow.Default;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions[ActionMove];
            _speedModifierAction = _playerInput.actions[ActionSpeedModifier];
        }

        private void Start()
        {
            _newPosition = transform.position;
            _movementSpeed = _normalSpeed;
        }

        private void Update()
        {
            HandleCameraMovement();
        }

        private void HandleCameraMovement()
        {
            if (_moveWidthKeyboad)
            {
                _movementSpeed = _speedModifierAction.IsPressed() ? _fastSpeed : _normalSpeed;

                Vector2 moveInput = _moveAction.ReadValue<Vector2>();

                _newPosition += transform.forward * (_movementSpeed * moveInput.y);
                _newPosition += transform.right * (_movementSpeed * moveInput.x);
            }

            if (_moveWidthEdgeScrolling)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector3 moveDirection = Vector3.zero;

                CursorArrow newCursor = CursorArrow.Default;

                if (mousePosition.x > Screen.width - _edgeSize)
                {
                    moveDirection += transform.right;
                    newCursor = CursorArrow.Right;
                }
                else if (mousePosition.x < _edgeSize)
                {
                    moveDirection -= transform.right;
                    newCursor = CursorArrow.Left;
                }

                if (mousePosition.y > Screen.height - _edgeSize)
                {
                    moveDirection += transform.forward;
                    newCursor = newCursor switch
                    {
                        CursorArrow.Left => CursorArrow.UpLeft,
                        CursorArrow.Right => CursorArrow.UpRight,
                        _ => CursorArrow.Up
                    };
                }
                else if (mousePosition.y < _edgeSize)
                {
                    moveDirection -= transform.forward;
                    newCursor = newCursor switch
                    {
                        CursorArrow.Left => CursorArrow.DownLeft,
                        CursorArrow.Right => CursorArrow.DownRight,
                        _ => CursorArrow.Down
                    };
                }

                if (moveDirection != Vector3.zero)
                {
                    _newPosition += moveDirection * _movementSpeed;
                    ChangeCursor(newCursor);
                    _isCursorSet = true;
                }
                else if (_isCursorSet)
                {
                    ChangeCursor(CursorArrow.Default);
                    _isCursorSet = false;
                }
            }

            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _movementSensitivity);
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void ChangeCursor(CursorArrow newCursor)
        {
            if (_currentCursor == newCursor)
                return;

            switch (newCursor)
            {
                case CursorArrow.Up:
                    Cursor.SetCursor(_cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.Down:
                    Cursor.SetCursor(_cursorArrowDown, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.Left:
                    Cursor.SetCursor(_cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.Right:
                    Cursor.SetCursor(_cursorArrowRight, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.UpLeft:
                    Cursor.SetCursor(_cursorArrowUpLeft, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.UpRight:
                    Cursor.SetCursor(_cursorArrowUpRight, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.DownLeft:
                    Cursor.SetCursor(_cursorArrowDownLeft, Vector2.zero, CursorMode.Auto);
                    break;

                case CursorArrow.DownRight:
                    Cursor.SetCursor(_cursorArrowDownRight, Vector2.zero, CursorMode.Auto);
                    break;


                case CursorArrow.Default:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newCursor), newCursor, null);
            }

            _currentCursor = newCursor;
        }
    }
}