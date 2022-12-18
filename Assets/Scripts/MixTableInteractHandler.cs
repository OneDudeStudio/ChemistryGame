using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixTableInteractHandler : InteractableEnviroment
{
    [SerializeField] private GameObject _mixTableCanvas;

    [Header("Particle System")]
    public ParticleSystem NewElementCreated;
    public ParticleSystem ElementNotCreated;


    [Header("Points")]
    [SerializeField] private Transform _firstElementPoint;
    [SerializeField] private Transform _secondElementPoint;
    public Transform ResultElementPoint;

    [Header("Current Elements")]
    public DragableItem _firstSlotDragableItem;
    public DragableItem _secondSlotDragableItem;
    [SerializeField] private bool _readyForMerge;

    [Header("UI Elements")]
    [SerializeField] private GameObject _reactArea;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TextMeshProUGUI _firstElementText;
    [SerializeField] private TextMeshProUGUI _secondElementText;

    [SerializeField] private Button _addFirstElementBtn;
    [SerializeField] private Button _addSecondElementBtn;
    [SerializeField] private Button _addResultElementBtn;

    [Header("Messages")]

    public string NoneElementString;
    public string AddElementString;
    public string FirstElementString;
    public string SecondElementString;
    public string ResultElementString;


    private GameManager _gameManager;

    public override void OnEnable()
    {
        base.OnEnable();
        _mixTableCanvas.SetActive(false);
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void UpdateTableUI()
    {
        if (_readyForMerge == false)
        {
            _addResultElementBtn.gameObject.SetActive(false);
            if (_playerManager.CurrentDragableItem == null)
            {
                _messageText.text = NoneElementString;
                _reactArea.gameObject.SetActive(false);
                _addFirstElementBtn.gameObject.SetActive(false);
                _addSecondElementBtn.gameObject.SetActive(false);

            }
            else if (_playerManager.CurrentDragableItem != null)
            {
                _messageText.text = AddElementString + _playerManager.CurrentDragableItem._element.ElementName;
                _reactArea.gameObject.SetActive(true);
                _addFirstElementBtn.gameObject.SetActive(true);
                _addSecondElementBtn.gameObject.SetActive(true);
            }
        }
        else
        {
            _reactArea.gameObject.SetActive(true);
            _addResultElementBtn.gameObject.SetActive(true);
        }
    }

    public void MergeIngridients()
    {
        EndInteract();
        StartCoroutine(MergeIngridientsCoroutine(_firstSlotDragableItem, _secondSlotDragableItem, ResultElementPoint));
    }

    IEnumerator MergeIngridientsCoroutine(DragableItem item1, DragableItem item2, Transform parent)
    {
        item1.DisableItem(parent);
        item2.DisableItem(parent);
        float animationDuration = 1f;
        float t = 0;
        

        while (t < 1)
        {
            item1.transform.localPosition = Vector3.Lerp(item1.transform.localPosition, Vector3.zero, t);
            item2.transform.localPosition = Vector3.Lerp(item2.transform.localPosition, Vector3.zero, t);
            t += Time.deltaTime / animationDuration;
            yield return null;
        }
        Debug.Log(item1._element.ElementLat + "+" + item2._element.ElementLat);
        CreateNewElement(item1._element.ElementLat + "+" + item2._element.ElementLat);
        Destroy(item1.gameObject);
        Destroy(item2.gameObject);
        yield return new WaitForSeconds(1f);
    }

    public void CreateNewElement(string potentialElementString)
    {
        _gameManager.CheckNewElement(potentialElementString);
    }


    public void AddFirstElement(Transform point)
    {
        if (_firstSlotDragableItem == null)
        {
            if (_playerManager.CurrentDragableItem != null)
            {
                _playerManager.SetItemToSlot(point);
                _firstSlotDragableItem = point.GetComponentInChildren<DragableItem>();
                _firstElementText.text = _firstSlotDragableItem._element.ElementReactLat;
                EndInteract();
                CheckForWaitResult();
            }

        }

    }

    public void AddSecondElement(Transform point)
    {
        if (_secondSlotDragableItem == null)
        {
            if (_playerManager.CurrentDragableItem != null)
            {
                _playerManager.SetItemToSlot(point);
                _secondSlotDragableItem = point.GetComponentInChildren<DragableItem>();
                _secondElementText.text = _secondSlotDragableItem._element.ElementReactLat;
                EndInteract();
                CheckForWaitResult();
            }
        }

    }


    public void CheckIngridients()
    {
        if (_firstElementPoint.transform.childCount == 0)
        {
            _firstSlotDragableItem = null;
            _firstElementText.text = "Добавить";
        }
        if (_secondElementPoint.transform.childCount == 0)
        {
            _secondSlotDragableItem = null;
            _secondElementText.text = "Добавить";
        }
        CheckForWaitResult();
    }
    public void CheckForWaitResult()
    {
        if (_secondSlotDragableItem != null && _firstSlotDragableItem != null)
        {
            _readyForMerge = true;
            _messageText.text = ResultElementString;
        }
        else
        {
            _readyForMerge = false;
        }
    }

    public override void Interact()
    {
        base.Interact();
        CheckIngridients();
        InteractMessageImage.GetComponent<InteractMessage>().HideThisObject();
        UpdateTableUI();
        _mixTableCanvas.SetActive(true);
        _playerLocomotion.GameOnPause = true;
        _playerLocomotion.StepPause();
        _playerLocomotion.lockedCursor = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public override void EndInteract()
    {
        base.EndInteract();
        _playerManager.CurrentInteractiveEnviroment = null;
        InteractMessageImage.GetComponent<InteractMessage>().ShowThisObject();
        _mixTableCanvas.SetActive(false);
        _playerLocomotion.GameOnPause = false;
        _playerLocomotion.lockedCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
