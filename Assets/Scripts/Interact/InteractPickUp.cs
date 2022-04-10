using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    BonfireKey,
    Charger,
    FlashLight,
    Geiger,
    Filter,
    GasMask
}
public class InteractPickUp : Interactable
{
    [SerializeField] private GameObject _thisObject;
    [SerializeField] private ItemType type;


    public override void Interact(GameObject player)
    {
        player.GetComponent<CableScripts>().FillInventory(type);
        Destroy(this.gameObject);
    }
}
