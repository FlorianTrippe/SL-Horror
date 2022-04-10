using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private Vector3 _startPosition;
    public Animator Anim;
    public bool TutorialDoor;
    public CableScripts Player;


    // Start is called before the first frame update
    void Start()
    {
        _startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject player)
    {
        if (!TutorialDoor)
        {
            if (gameObject.transform.position == _startPosition)
                Anim.SetBool("Opening", true);
            else
                Anim.SetBool("Opening", false);
        }
        else if (Player.HasGasMask)
        {
            Anim.SetBool("Opening", true);
        }
    }

    public void DoorOpen()
    {
        Destroy(gameObject);
    }


}
