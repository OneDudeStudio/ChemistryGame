using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableEnviroment : Item
{
    public Image InteractMessageImage;
    protected TextMeshProUGUI _interactMessage;
    public string _itemName;
    protected PlayerManager _playerManager;

    public virtual void OnEnable()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        InteractMessageImage = FindObjectOfType<InteractMessage>().GetComponent<Image>();
        _interactMessage = FindObjectOfType<InteractMessageText>().GetComponent<TextMeshProUGUI>();
    }

    public override void EnableItemOutline()
    {
        if (OutlineIsEnable == false)
        {
            _thisObjectOutline.enabled = true;
            OutlineIsEnable = true;

            _interactMessage.text = "Взаимодействовать с  " + _itemName + " (F)";
            InteractMessageImage.GetComponent<InteractMessage>().ShowThisObject();
            _playerManager.UpdateCurrentItemToInteractive();
        }
    }

    public override void DisableItemOutline()
    {
        if (OutlineIsEnable == true)
        {
            _thisObjectOutline.enabled = false;
            OutlineIsEnable = false;
            InteractMessageImage.GetComponent<InteractMessage>().HideThisObject(); ;
        }
    }


    public virtual void Interact()
    {
        
    }
    public virtual void EndInteract()
    {
        
    }
}
