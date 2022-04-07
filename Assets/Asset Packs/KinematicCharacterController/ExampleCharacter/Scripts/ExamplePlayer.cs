using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public CableScripts CableScript;
        public float MouseSensibilityX = 0.2f;
        public float MouseSensibilityY = 0.2f;

        private bool _jumpBool;
        private bool _crouchBoolDown;
        private bool _crouchBoolUp;
        private bool _runBoolDown;
        private bool _runBoolUp;
        private float _scrollInput;
        private float _mouseLookAxisUp;
        private float _mouseLookAxisRight;
        private Vector2 _moveVector;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);
            CharacterCamera.TargetDistance = 0f;

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        public void InputMove(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();
        }
        public void InputLook(InputAction.CallbackContext context)
        {
            Vector2 MouseDelta = context.ReadValue<Vector2>();
            _mouseLookAxisUp = MouseDelta.y * MouseSensibilityY;
            _mouseLookAxisRight = MouseDelta.x * MouseSensibilityX;
        }
        public void InputScroll(InputAction.CallbackContext context)
        {
            Vector2 ScrollVector = context.ReadValue<Vector2>();
        }
        public void InputLeftClick(InputAction.CallbackContext context)
        {

        }
        public void InputRightClick(InputAction.CallbackContext context)
        {
            // CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
        }
        public void InputJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _jumpBool = true;
            }
            else
            {
                _jumpBool = false;
            }
        }
        public void InputCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _crouchBoolDown = true;
                _crouchBoolUp = false;
            }
            else
            {
                _crouchBoolDown = !true;
                _crouchBoolUp = !false;
            }
        }
        public void InputRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _runBoolDown = true;
                _runBoolUp = false;
            }
            else
            {
                _runBoolDown = !true;
                _runBoolUp = !false;
            }
        }

        public void InputInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Vector3 position = CableScript._cableStartPoint.position;
                position += CharacterCamera.transform.forward;

                CableScript.UseCable(position);
            }
            else
            {
            }
        }
        public void InputSwitchMaskFilters(InputAction.CallbackContext context)
        {

        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            Vector3 lookInputVector = new Vector3(_mouseLookAxisRight, _mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }
            

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, _scrollInput, lookInputVector);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = _moveVector.y;
            characterInputs.MoveAxisRight = _moveVector.x;
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = _jumpBool;
            characterInputs.CrouchDown = _crouchBoolDown;
            characterInputs.CrouchUp = _crouchBoolUp;
            characterInputs.RunDown = _runBoolDown;
            characterInputs.RunUp = _runBoolUp;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}