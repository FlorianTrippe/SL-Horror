using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip _gasMaskReload;
    [SerializeField] private AudioClip _lighSwitchSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGasMaskReload()
    {
        SFXManager.Instance.PlayClip(_gasMaskReload);
    }

    public void PlayLightSwitchSound()
    {
        SFXManager.Instance.PlayClip(_lighSwitchSound);
    }
}
