using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using BehaviorDesigner.Runtime;
// using Speech_Recognition.Audio_Input;
// using Speech_Recognition.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Speech_Recognition.Spawn
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] protected float _HP;
        [SerializeField] private int _dmg;
        [SerializeField] private float _obstacleCheckDistance;
        [SerializeField] private float _gravityMultiply;
        [SerializeField] private float _sightDistance;
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] public float SpellTime;
        [SerializeField] public float SpellDelay;
        // [SerializeField] public Spell DefenceSpell;
        // [SerializeField] public Spell AttackSpell;
        [SerializeField] public GameObject SpellPosition;
        [SerializeField] private BehaviorTree _behaviorTree;
        [SerializeField] private GameObject _sightCone;
        [SerializeField, Range(5,175)] private float _sightAngle;
        [SerializeField] private Sight _sightTrigger;
        [SerializeField] private LayerMask _sightLayerMask;
        [SerializeField] public LayerMask GroundLayerMask;
        [SerializeField] private GameObject[] _eyeObjects = new GameObject[2];
        [SerializeField, Range(5, 20)] private float _coverSearchRange;
        [SerializeField] private LayerMask _coverLayerMask;

        /*[HideInInspector]*/
        public GameObject Target;
        [HideInInspector] public bool SpellReady;
        [HideInInspector] public bool JumpBool;
        [HideInInspector] public bool Grounded;
        [HideInInspector] public bool GotDamage;
        [HideInInspector] public bool HasShield;
        [HideInInspector] public int CurrentShield;
        [HideInInspector] public Vector3 DamagePosition;
        [HideInInspector] public Transform NearestCoverTransform;
        [HideInInspector] public GameObject ShieldObject;

        // private CharacterController _cC;
        private Animator _animator;
        private Vector3 _moveDirection;
        private Vector3 _nextMovePoint;
        private Vector3 _oldMovePoint;
        private NavMeshAgent _agent;
        private NavMeshData _mesh;
        private float _gravity = Physics.gravity.y;
        private float _currentJumpVelocity;
        private float _currentTime;
        private bool _canCast = true;

        protected virtual void Awake()
        {
            SetSightCone();
            // _cC = GetComponent<CharacterController>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _nextMovePoint = transform.position;
        }

        private void SetSightCone()
        {
            float beta = 90f - (_sightAngle / 2);
            float c =  _sightDistance / Mathf.Sin(beta);
            float c2 = c * c;
            float b2 = _sightDistance * _sightDistance;
            float a = Mathf.Sqrt(c2-b2);
            _sightCone.transform.localScale = new Vector3(a, a, _sightDistance);
        }

        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position, transform.up * -1 * _groundCheckDistance, Color.green);
            if (Physics.Raycast(transform.position, transform.up*-1, _groundCheckDistance, GroundLayerMask))
            {
                Grounded = true;
            }
            else
            {
                Grounded = false;
            }
        }
        
        protected virtual void Update()
        {
            // _agent.SetDestination(Target.transform.position);
            _animator.SetFloat("VelocityForward", _agent.velocity.magnitude);

            if (!_canCast && _currentTime >= SpellDelay)
            {
                _currentTime = 0;
                _canCast = true;
            }
            else if(!_canCast)
            {
                _currentTime += Time.deltaTime;
            }

            // if (!Grounded && !JumpBool)
            // {
            //     _animator.SetBool("JumpBool", true);
            //     _animator.SetFloat("JumpFloat", _agent.velocity.y);
            //     // _cC.Move(transform.up * _gravity * _gravityMultiply * Time.deltaTime);
            // }
            // else
            // {
            //     _animator.SetBool("JumpBool", false);
            // }
        }

        public bool WantToCast()
        {
            if (_canCast)
            {
                _canCast = false;
                SpellReady = true;
                return true;
            }

            return false;
        }

        public void NearestCover()
        {
            List<Collider> colliderList = Physics.OverlapSphere(transform.position, _coverSearchRange, _coverLayerMask).ToList();
            if (colliderList.Count > 0)
            {
                Transform nearestTransform = colliderList[0].gameObject.transform;

                float distance = Vector3.Distance(transform.position, nearestTransform.position);

                foreach (Collider collider in colliderList)
                {
                    float x = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                    if (x < distance)
                    {
                        distance = x;
                        nearestTransform = collider.gameObject.transform;
                    }
                }

                NearestCoverTransform = nearestTransform;
            }
            else
            {
                NearestCoverTransform = null;
            }
        }

        public void DoDamage()
        {
        }

        public void ToggleEyes()
        {
            _eyeObjects[0].SetActive(!_eyeObjects[0].active);
            _eyeObjects[1].SetActive(!_eyeObjects[1].active);
        }

        public bool Jump(float JumpHeight, float JumpSpeed)
        {
            if (_currentJumpVelocity < JumpHeight && JumpBool)
            {
                _currentJumpVelocity += JumpSpeed;
                Debug.Log(_currentJumpVelocity + " : " + JumpHeight);
                // _cC.Move(transform.up * _currentJumpVelocity * Time.deltaTime);
                return false;
            }
            else if (!Grounded)
            {
                _currentJumpVelocity = 0;
                JumpBool = false;
                return false;
            }
            return true;
        }

        public List<GameObject> CheckSight()
        {
            List<GameObject> returnList = new List<GameObject>();
            RaycastHit hit;
            foreach (GameObject obj in _sightTrigger.CheckSight())
            {
                #region DebugRays
                /*
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up * 0.2f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up * 1.8f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up * 0.2f + obj.transform.right * 0.2f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up * 1.8f + obj.transform.right * 0.2f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up + obj.transform.right * 0.2f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up * 0.2f + obj.transform.right * -0.2f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up * 1.8f + obj.transform.right * -0.2f), Color.red);
                Debug.DrawLine(_sightCone.transform.position, (obj.transform.position + obj.transform.up + obj.transform.right * -0.2f), Color.red);
                /**/
                #endregion
                
                if (Physics.Raycast(_sightCone.transform.position,Vector3.Normalize((obj.transform.position + obj.transform.up) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj)
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + obj.transform.up * 0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + obj.transform.up * 1.8f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + obj.transform.up * 0.2f + obj.transform.right * 0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + Vector3.up * 1.8f + obj.transform.right * 0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + Vector3.up + obj.transform.right * 0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + obj.transform.up * 0.2f + obj.transform.right * -0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + Vector3.up * 1.8f + obj.transform.right * -0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
                if (Physics.Raycast(_sightCone.transform.position, Vector3.Normalize((obj.transform.position + Vector3.up + obj.transform.right * -0.2f) - _sightCone.transform.position), out hit, Mathf.Infinity, _sightLayerMask))
                {
                    if (hit.transform.gameObject == obj && !returnList.Contains(obj))
                    {
                        returnList.Add(obj);
                    }
                }
            }

            return returnList;
        }

        public void Attack()
        {
            _animator.SetBool("AttackBool", true);
        }
        
        public virtual void GetDamage(float dmg, Vector3 position)
        {
            GotDamage = true;
            DamagePosition = position;
            bool getDamage = true;
            if (HasShield)
            {
                if (CurrentShield - dmg <= 0)
                {
                    float overflowDamage = dmg - CurrentShield;
                    ShieldBreak();
                    dmg = overflowDamage;
                    HasShield = false;
                    CurrentShield = 0;
                }
                else
                {
                    CurrentShield -= (int)dmg;
                    getDamage = false;
                }
            }

            if (getDamage)
            {
                Debug.Log("Damage: " + dmg);
                _HP -= dmg;

                if (_HP <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        protected void ShieldBreak()
        {
            Destroy(ShieldObject);
        }

        private bool FindObstacle(Vector3 start, Vector3 end, float length, LayerMask mask)
        {
            RaycastHit hit;
            Ray ray = new Ray(start,end);
            
            Debug.DrawRay(ray.origin,ray.direction * length, Color.red);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, length, mask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}