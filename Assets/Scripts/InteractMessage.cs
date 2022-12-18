using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMessage : MonoBehaviour
{
    [SerializeField] private Vector3 _hidePosition;
    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
        HideThisObject();
    }

    public void HideThisObject()
    {
        transform.position = _hidePosition;
    }

    public void ShowThisObject()
    {
        transform.position = startPosition;
    }
}
