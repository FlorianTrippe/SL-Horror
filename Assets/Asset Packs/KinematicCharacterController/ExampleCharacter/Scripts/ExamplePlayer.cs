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
        private bool _geigerStatus;
        private bool _lampStatus;
        private bool _chargerStatus;
        private float _scrollInput;
        private float _mouseLookAxisUp;
        private float _mouseLookAxisRight;
        private Vector2 _moveVector;

        public bool ReadyToCharge = false;

        [SerializeField] private Animator _anim;
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
        }
        public void InputLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (ReadyToCharge)
                {
                    CableScript.Charge();
                    _anim.SetBool("Charging", true);
                }
                else
                {
                    Debug.Log("Click");
                    CableScript.Interact();                    
                }
            }
            else
            {
                _anim.SetBool("Charging", false);
            }
        }
        public void InputRightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _anim.SetBool("ChargerAway", false);
                Debug.Log("Click Right");
                CableScript.EquipCharger();
                ReadyToCharge = true;
                _anim.SetBool("ChargerOut", true);
            }
            if(context.phase == InputActionPhase.Canceled)
            {
                _anim.SetBool("ChargerOut", false);
                _anim.SetBool("ChargerAway", true);
                CableScript.EquipLastItem();
                ReadyToCharge = false;
            }
        }
        public void InputHold(InputAction.CallbackContext context)
        {

            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log("Hold Click");
            }
            else
            {
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
        public void InputGeiger(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                switch (_geigerStatus)
                {
                    case true:
                        _anim.SetBool("GeigerIn", true);
                        _geigerStatus = false;
                        break;
                    case false:
                        _anim.SetBool("GeigerOut", true);
                        _geigerStatus = true;
                        break;
                }
            }
            else
            {
                _anim.SetBool("GeigerOut", false);
                _anim.SetBool("GeigerIn", false);
            }

        }
        public void InputFlashLight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                switch (_lampStatus)
                {
                    case true:
                        _anim.SetBool("LampAway", true);
                        _lampStatus = false;
                        break;
                    case false:
                        _anim.SetBool("LampOut", true);
                        _lampStatus = true;
                        break;
                }
            }
            else
            {
                _anim.SetBool("LampOut", false);
                _anim.SetBool("LampAway", false);
            }
        }
        public void InputCharger(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                switch (_chargerStatus)
                {
                    case true:
                        _anim.SetBool("ChargerAway", true);
                        _chargerStatus = false;
                        break;
                    case false:
                        _anim.SetBool("ChargerOut", true);
                        _chargerStatus = true;
                        break;
                }
            }
            else
            {
                _anim.SetBool("ChargerOut", false);
                _anim.SetBool("ChargerAway", false);
            }
        }
        public void InputSwitchMaskFilters(InputAction.CallbackContext context)
        {
            
            if (context.phase == InputActionPhase.Performed)
            {
                _anim.SetBool("Reload", true);
                CableScript.DropKey();
                
            }
            else
            {
                _anim.SetBool("Reload", false);
            }
            
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