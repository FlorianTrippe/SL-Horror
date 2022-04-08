using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using UnityEngine;
using DG.Tweening;

public class CableScripts : MonoBehaviour
{
    [SerializeField] private GameObject _cable;
    [SerializeField] public Transform _cableStartPoint;

    private GameObject _spawnedCable;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UseCable(Vector3 plugPosition)
    {
        _spawnedCable = Instantiate(_cable, _cableStartPoint.position, Quaternion.Euler(transform.forward.x, 0, transform.forward.z),this.gameObject.transform);
        _spawnedCable.transform.DOMove(plugPosition, 2, false);
    }

    public void RemoveCable()
    {

    }
}
