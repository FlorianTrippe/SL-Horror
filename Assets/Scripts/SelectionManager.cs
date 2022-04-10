using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject _interactablePopUp;
    [SerializeField] private Camera _cam;

    // Update is called once per frame
    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitGameobject = hit.transform.gameObject;
            InteractableDescription _interactableDescripiton = hitGameobject.GetComponent<InteractableDescription>();
            
            if (_interactableDescripiton != null)
            {
                _interactablePopUp.SetActive(true);
                _interactableDescripiton.SetText(_interactableDescripiton.Description);
            }
            else
            {
                _interactablePopUp.SetActive(false);
                //_interactableDescripiton.SetText("");
            }
        }
    }
}
