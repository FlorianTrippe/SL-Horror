using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class EcchiEnemy : MonoBehaviour
{
    [SerializeField] private BehaviorTree _behaviorTree;
    [SerializeField] private GameObject _sightCone;
    [SerializeField, Range(5, 175)] private float _sightAngle;
    [SerializeField] private float _sightDistance;
    [SerializeField] private Sight _sightTrigger;
    [SerializeField] private LayerMask _sightLayerMask;
    [SerializeField] private GameObject _targetTarget;
    


    void Start()
    {
        SetSightCone();
    }

    private void SetSightCone()
    {
        float beta = 90f - (_sightAngle / 2);
        float c = _sightDistance / Mathf.Sin(beta);
        float c2 = c * c;
        float b2 = _sightDistance * _sightDistance;
        float a = Mathf.Sqrt(c2 - b2);
        _sightCone.transform.localScale = new Vector3(a, a, _sightDistance);
    }

    public List<GameObject> CheckSight()
    {
        List<GameObject> returnList = new List<GameObject>();
        RaycastHit hit;

        foreach (GameObject obj in _sightTrigger.CheckSight())
        {
            Debug.DrawRay(_sightCone.transform.position, obj.transform.position + Vector3.down, Color.red, 1f);
            if (Physics.Raycast(_sightCone.transform.position, obj.transform.position + Vector3.down, out hit, Mathf.Infinity, _sightLayerMask))
            {
                if (hit.transform.gameObject == obj)
                {
                    returnList.Add(obj);
                }
            }
        }

        return returnList;
    }

    public bool CheckForNoise()
    {
        return NoiseManager.NoiseManagerReference.HeardNoise();
    }
    public Vector3 CheckForNoiseLocation()
    {
        return NoiseManager.NoiseManagerReference.NoiseLocation();
    }

    void FixedUpdate()
    {

    }

    void Update()
    {

    }
}
