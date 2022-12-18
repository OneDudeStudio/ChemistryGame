using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class DragableItem : Item
{
    private PlayerManager _playerManager;

    private bool _isInteracting;
    private BoxCollider _thisItemBoxCollider;

    #region UI settings
    [Header("UI Settings")]

    public ElementSetting _element;
    [SerializeField] private GameObject _itemCanvas;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemLatNameText;
    [SerializeField] private float _canvasSize = 0.2f;
    //
    [SerializeField] private Image _elementInteractMessage;
    [SerializeField] private TextMeshProUGUI _interactMessage;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _itemCanvas.gameObject.SetActive(false);
        //event camera
        _thisItemBoxCollider = GetComponent<BoxCollider>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _elementInteractMessage = FindObjectOfType<InteractMessage>().GetComponent<Image>();
        _interactMessage = FindObjectOfType<InteractMessageText>().GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
       if (_isInteracting)
       {
           transform.localPosition = Vector3.zero;
           transform.localRotation = Quaternion.identity;
       }
    }

    private void LateUpdate()
    {
        ItemCanvasHandler();
    }
    private void ItemCanvasHandler()
    {
        if (_itemCanvas.activeInHierarchy)
        {
            Vector3 toTarget = _playerLocomotion.playerCamera.transform.position - transform.position;
            _itemCanvas.transform.rotation = Quaternion.LookRotation(-toTarget);
            float scale = Vector3.Distance(transform.position, _playerLocomotion.playerCamera.transform.position);
            _itemCanvas.transform.localScale = Vector3.one * scale * _canvasSize;
        }
    }

    public void SetItemToInteract()
    {
        Rigidbody.isKinematic = true;
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }

    public override void EnableItemOutline()
    {
        if (OutlineIsEnable == false)
        {
            _thisObjectOutline.enabled = true;
            OutlineIsEnable = true;

            _itemNameText.text = _element.ElementName;
            _itemLatNameText.text = _element.ElementReactLat;
            _itemCanvas.gameObject.SetActive(true);
            //
            _elementInteractMessage.GetComponent<InteractMessage>().ShowThisObject();
            _interactMessage.text = "Подобрать " + _element.ElementName + " (E)";
            //
            _playerManager.UpdateCurrentItemToDragable();
        }
    }

    public override void DisableItemOutline()
    {
        if (OutlineIsEnable == true)
        {
            _thisObjectOutline.enabled = false;

            OutlineIsEnable = false;
            _itemCanvas.gameObject.SetActive(false);
            _elementInteractMessage.GetComponent<InteractMessage>().HideThisObject();
        }
    }
    public void TakeItem()
    {
        if(_isInteracting == false)
        {
            _isInteracting = true;
            Rigidbody.isKinematic = true;
            _thisItemBoxCollider.enabled = false;
            transform.parent = _playerSearcher.PlayerHand;
            transform.localScale = Vector3.one;
        }
        
    }

    public void DropItem()
    {
        if(_isInteracting == true)
        {
            Rigidbody.isKinematic = false;
            _isInteracting = false;
            _thisItemBoxCollider.enabled = true;
            transform.parent = null;
            transform.localScale = Vector3.one;
        }
        

    }

    public void DisableItem(Transform parent)
    {
        _thisItemBoxCollider.enabled = false;
        _isInteracting = false;
        transform.parent = parent;
    }

}
