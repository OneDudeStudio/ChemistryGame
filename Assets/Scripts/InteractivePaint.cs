using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePaint : InteractableEnviroment
{

    [SerializeField] private GameObject _paintCanvas;

    protected override void Awake()
    {
        base.Awake();
        _paintCanvas.SetActive(false);
    }
    public override void EnableItemOutline()
    {
        if (OutlineIsEnable == false)
        {
            _thisObjectOutline.enabled = true;
            OutlineIsEnable = true;
            _playerManager.UpdateCurrentItemToInteractive();
            _paintCanvas.SetActive(true);
        }
    }

    public override void DisableItemOutline()
    {
        if (OutlineIsEnable == true)
        {
            _thisObjectOutline.enabled = false;
            OutlineIsEnable = false;
            _paintCanvas.SetActive(false);
        }
    }
}
