using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private float _yRotation;
    // Start is called before the first frame update
    void Start()
    {
        _yRotation = this.gameObject.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject player)
    {
        base.Interact(player);
        if(this.gameObject.transform.rotation.y == _yRotation)
        {
           
            StartCoroutine(MoveDoor(false));
        }
        else
        {
            StartCoroutine(MoveDoor(true));
        }
    }

    public IEnumerator MoveDoor(bool open)
    {
        bool wait = false;
        switch (open)
        {
            
            case true:
                while (gameObject.transform.eulerAngles.y >= _yRotation) 
                {
                    gameObject.transform.Rotate(new Vector3(0, Time.deltaTime * -10, 0));
                }
                wait = true;
                break;
            case false:
                while (gameObject.transform.eulerAngles.y <= _yRotation + 90)
                {
                    gameObject.transform.Rotate(new Vector3(0, Time.deltaTime * 10, 0));
                }
                wait = true;
                break;
        }
        yield return new WaitUntil(() => wait == true);
    }
}
