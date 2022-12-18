using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerLocomotion _playerLocomotion;
    [SerializeField] private PlayerSearcher _playerSearcher;
    [SerializeField] private UIManager _uIManager;

    [SerializeField] private Item _currentItem;
    [SerializeField] private bool _isDragableItem = false;
    [SerializeField] private bool _isInteractiveItem = false;

    public DragableItem CurrentDragableItem;
    public InteractableEnviroment CurrentInteractiveEnviroment;


    private void Update()
    {
        _currentItem = _playerSearcher.CurrentSelectedItem;
        if (_currentItem != null)
        {
            if (_currentItem.DragableItem)
            {
                _isDragableItem = true;
                _isInteractiveItem = false;
            }
            else
            {
                _isDragableItem = false;
                _isInteractiveItem = true;
            }
        }
        
        if (_currentItem == null)
        {
            _isDragableItem = false;
            _isInteractiveItem = false;
        }
        ItemTakeHandler();
        InteractWithEnviroment();

    }


    private void ItemTakeHandler()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentDragableItem == null)
            {
                if (_isDragableItem)
                {
                    CurrentDragableItem = _currentItem.GetComponent<DragableItem>();
                    CurrentDragableItem.TakeItem();
                }
            }
            else if (CurrentDragableItem != null)
            {
                RemoveItemHandler();
            }
        }
    }
    


    public void SetItemToSlot(Transform point)
    {
        CurrentDragableItem.DropItem();
        CurrentDragableItem.transform.position = point.position;
        CurrentDragableItem.Rigidbody.isKinematic = true;
        CurrentDragableItem.transform.rotation = Quaternion.identity;
        CurrentDragableItem.transform.parent = point;
        CurrentDragableItem = null;
    }

    private void RemoveItemHandler()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentDragableItem != null)
            {
                CurrentDragableItem.DropItem();
                CurrentDragableItem = null;
            }
        }


    }

    private void InteractWithEnviroment()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CurrentInteractiveEnviroment == null)
            {
                if (_isInteractiveItem)
                {
                    _isDragableItem = false;
                    CurrentInteractiveEnviroment = _currentItem.GetComponent<InteractableEnviroment>();
                    CurrentInteractiveEnviroment.Interact();
                }
            }
            else if (CurrentInteractiveEnviroment != null)
            {
                Debug.Log("End interact");
                CurrentInteractiveEnviroment.EndInteract();
                CurrentInteractiveEnviroment = null;
            }

        }
    }

    public void UpdateCurrentItemToDragable()
    {
        _isDragableItem = true;
    }

    public void UpdateCurrentItemToInteractive()
    {
        _isInteractiveItem = true;
    }



}
