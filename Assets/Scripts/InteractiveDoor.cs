using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoor : InteractableEnviroment
{
    public Transform pivot;
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private Vector3 target;
    private Quaternion _startRotation;

    private void Start()
    {
        _startRotation = transform.rotation;
    }

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(RotateDoor(_isOpen));
    }
    public override void EnableItemOutline()
    {
        if (OutlineIsEnable == false)
        {
            _thisObjectOutline.enabled = true;
            OutlineIsEnable = true;

            _interactMessage.text = "Открыть " + _itemName + " (F)";
            InteractMessageImage.GetComponent<InteractMessage>().ShowThisObject();
            _playerManager.UpdateCurrentItemToInteractive();
        }
    }


    private IEnumerator RotateDoor(bool isOpen)
    {
        _playerManager.CurrentInteractiveEnviroment = null;
        float t = 0;
        if (isOpen == false)
        {
            _isOpen = true;
            while (t < 1)
            {
                pivot.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(target), t);
                t += Time.deltaTime / _animationDuration;
                yield return null;
            }
        }
        else
        {
            _isOpen = false;
            
            while (t < 1)
            {
                pivot.rotation = Quaternion.Lerp(transform.rotation, _startRotation, t);
                t += Time.deltaTime / _animationDuration;
                yield return null;
            }
        }


    }
}
