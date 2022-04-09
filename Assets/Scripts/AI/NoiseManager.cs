using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager NoiseManagerReference;

    private bool _heardNoise;
    private Vector3 _noiseLocation;
    private bool _alarmingNoise;
    private float _fallOffDistance;

    void Start()
    {
        NoiseManagerReference = this;
    }

    public void SetNoise(Vector3 location, bool alarmingNoise, float fallOffDistance)
    {
        _fallOffDistance = fallOffDistance;
        _alarmingNoise = alarmingNoise;
        _heardNoise = true;
        _noiseLocation = location;
    }

    public void CheckedNoise()
    {
        _heardNoise = false;
        _noiseLocation = Vector3.zero;
    }

    public bool HeardNoise()
    {
        return _heardNoise;
    }

    public Vector3 NoiseLocation()
    {
        return _noiseLocation;
    }
}
