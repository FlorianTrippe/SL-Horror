using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BonFireDing : Interactable
{
    [SerializeField] private GameObject _key;

    [SerializeField] private bool _activated;

    public UnityEvent<InteractionEventArgs> InteractionEvent;

    public UnityEvent HoldEvent;

    public class InteractionEventArgs : EventArgs
    {
        public bool BatteryActive;
    }

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

        InteractionEvent?.Invoke(new InteractionEventArgs{ BatteryActive = _activated });

        script.BonFireRespawn(this.gameObject);
        _activated = !_activated;
    }

    

    public void Hold()
    {
        HoldEvent?.Invoke();
    }
}