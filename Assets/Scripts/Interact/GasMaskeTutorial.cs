using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasMaskeTutorial : Interactable
{
    [SerializeField] private Door door;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Interact(GameObject player)
    {
        door.Anim.SetBool("Opening", true);

        Destroy(gameObject);
    }
}
