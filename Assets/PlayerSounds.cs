using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip _gasMaskReload;
    [SerializeField] private AudioClip _lighSwitchSound;
    [SerializeField] private AudioClip _squeak;
    [SerializeField] private Animator _anim;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChargeSound()
    {
        SFXManager.Instance.PlayClip(SFXManager.Instance.SFXList[1]);
    }
    public void PlayGeigerSound()
    {

    }
    public void ResetAnim()
    {
        _anim.SetBool("Charging", false);
    }
    public void PlayGasMaskReload()
    {
        SFXManager.Instance.PlayClip(SFXManager.Instance.SFXList[2]);
    }

    public void PlayLightSwitchSound()
    {
        SFXManager.Instance.PlayClip(_lighSwitchSound);
    }
}
