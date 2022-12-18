using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSearcher : MonoBehaviour
{
    public Transform pointer;
    public Image crosshair;
    public float maxDistance;

    public Transform PlayerHand;
    //public bool isEmpty;

    public Item CurrentSelectedItem;
    [SerializeField] private LayerMask _layerMask;

    private void LateUpdate()
    {
        SelectItemHandler();

    }
    private void SelectItemHandler()
    {
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);

        Debug.DrawRay(transform.position, ray.direction * 10, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Camera.main.ScreenPointToRay(crosshair.transform.position).direction, out hit, maxDistance, _layerMask))
        {
            pointer.position = hit.point;

            Item interactableItem = hit.collider.GetComponent<Item>();

            if (interactableItem != null)
            {
                if (CurrentSelectedItem != null && CurrentSelectedItem != interactableItem)
                {
                    CurrentSelectedItem.DisableItemOutline();
                }
                CurrentSelectedItem = interactableItem;
                interactableItem.EnableItemOutline();
            }
            else
            {
                if (CurrentSelectedItem != null)
                {
                    CurrentSelectedItem.DisableItemOutline();
                    CurrentSelectedItem = null;
                }
            }
        }
        else
        {
            if (CurrentSelectedItem != null)
            {
                CurrentSelectedItem.DisableItemOutline();
                CurrentSelectedItem = null;
            }
        }
    }
}
