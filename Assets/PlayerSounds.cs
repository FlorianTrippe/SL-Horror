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
        //SFXManager.Instance.PlayClip(_squeak);
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
       // SFXManager.Instance.PlayClip(_gasMaskReload);
    }

    public void PlayLightSwitchSound()
    {
        //SFXManager.Instance.PlayClip(_lighSwitchSound);
    }
}
