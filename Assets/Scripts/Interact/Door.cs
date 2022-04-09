using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private Vector3 _startPosition;
    [SerializeField] private Animator _anim;



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
        if (gameObject.transform.position == _startPosition)
            _anim.SetBool("Opening", true);
        else
            _anim.SetBool("Opening", false);
        
    }


}
