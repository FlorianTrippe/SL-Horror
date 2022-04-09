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

    public GameObject Obj;

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
            Vector3 pos = obj.transform.position - _sightCone.transform.position;
            Debug.DrawRay(_sightCone.transform.position, pos, Color.red, 1f);
            if (Physics.Raycast(_sightCone.transform.position, pos, out hit, Mathf.Infinity, _sightLayerMask))
            {
                Obj = obj;
                returnList.Add(obj);
            }
        }

        return returnList;
    }

    public void ResetNoise()
    {
        NoiseManager.NoiseManagerReference.ResetNoise();
    }

    public bool CheckForNoise()
    {
        if (NoiseManager.NoiseManagerReference.HeardNoise())
        {
            if (NoiseManager.NoiseManagerReference.FallOffDistance() >= Vector3.Distance(NoiseManager.NoiseManagerReference.NoiseLocation(), transform.position))
            {
                return true;
            }
        }
        return false;
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
