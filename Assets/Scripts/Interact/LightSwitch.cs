using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    private bool _state;
    [SerializeField] private List<Light> _lights;
    [SerializeField] private float _lightIntensity;
    [SerializeField] private AudioClip _lightSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject player)
    {
        //SFXManager.Instance.PlayClip(_lightSound);
        switch (_state)
        {
            case true:
                foreach(Light light in _lights)
                {
                    light.intensity = 0;
                    
                    _state = false;
                }
                break;
            case false:
                foreach(Light light in _lights)
                {
                    light.intensity = _lightIntensity;

                    _state = true;
                }
                break;
        }
    }
}
