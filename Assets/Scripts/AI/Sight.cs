using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    private List<GameObject> _sightObjectList = new List<GameObject>();

    private void OnTriggerEnter(Collider sightObjectCollider)
    {
        if (!_sightObjectList.Contains(sightObjectCollider.transform.gameObject))
        {
            _sightObjectList.Add(sightObjectCollider.transform.gameObject);
        }
    }

    private void OnTriggerExit(Collider sightObjectCollider)
    {
        if (_sightObjectList.Contains(sightObjectCollider.transform.gameObject))
        {
            _sightObjectList.Remove(sightObjectCollider.transform.gameObject);
        }
    }

    public List<GameObject> CheckSight()
    {
        return _sightObjectList;
    }
}
