using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Outline))]

public class Item : MonoBehaviour
{
    public bool DragableItem;
    protected PlayerLocomotion _playerLocomotion;
    protected PlayerSearcher _playerSearcher;
    public Rigidbody Rigidbody;
    public Outline _thisObjectOutline;
    [HideInInspector] public bool OutlineIsEnable = false;

    protected virtual void Awake()
    {
        _playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        _playerSearcher = FindObjectOfType<PlayerSearcher>();
        Rigidbody = GetComponent<Rigidbody>();
        _thisObjectOutline = GetComponent<Outline>();
        _thisObjectOutline.enabled = false;
    }

    public virtual void EnableItemOutline()
    {
        if (OutlineIsEnable == false)
        {
            _thisObjectOutline.enabled = true;
            OutlineIsEnable = true;
        }
    }

    public virtual void DisableItemOutline()
    {
        if (OutlineIsEnable == true)
        {
            _thisObjectOutline.enabled = false;
            OutlineIsEnable = false;
        }
    }
}
