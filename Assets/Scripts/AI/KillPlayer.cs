using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hitcollision: " + collision);
    }
}
