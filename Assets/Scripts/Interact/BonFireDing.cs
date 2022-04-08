using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFireDing : Interactable
{
    [SerializeField] private GameObject _key;

    private bool _activated;

    public override void Interact(GameObject player)
    {
        CableScripts script = player.GetComponent<CableScripts>();

        if (_activated)
        {
            script.PickUpBonFireKey();
            _key.SetActive(false);
        }
        else
        {
            script.PlaceBonFireKey();
            _key.SetActive(true);
        }

        script.BonFireRespawn(this.gameObject);
        _activated = !_activated;
    }

    public void Hold()
    {
        //TODO: do stuff//
    }
}