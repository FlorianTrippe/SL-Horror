using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public List<AudioClip> SFXList;
    public AudioSource SFX;
    public static AudioSource SFXInstance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SFXInstance = SFX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}