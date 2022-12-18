using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public struct Element
{
    public string latName;
    public GameObject prefab;
}

public class GameManager : MonoBehaviour
{
    public GameObject GameWinCanvas;
    public GameObject GameControlCanvas;
    [Header("Tutorial Settings")]
    public GameObject TutorialCanvas;
    public TextMeshProUGUI TutorialText;
    public Item TutorialIngridientFirst;
    public Item TutorialIngridientSecond;


    public bool TutorialIsActive;
    public AudioSource Source;
    public AudioClip Good;
    public AudioClip Bad;
    [SerializeField] private Book _book;
    [SerializeField] private bool _bookIsOpened;
    [SerializeField] private MixTableInteractHandler _mixTable;

    [SerializeField] private List<React> currentReactList = new List<React>();


    private PlayerLocomotion _playerLocomotion;
    private void Start()
    {
        _playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        for (int i = 0; i < _book.reactList.Count; i++)
        {
            currentReactList.Add(_book.reactList[i]);
        }
        TutorialIngridientFirst.gameObject.SetActive(false);
        TutorialIngridientSecond.gameObject.SetActive(false);
        StartCoroutine(TutorialCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_bookIsOpened == false)
            {
                ShowBook();
            }
            else if (_bookIsOpened)
            {
                HideBook();
            }
        }
    }

    public bool GetKeyDownB()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            return true;
        }

        return false;
    }

    public bool GetKeyDownF()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }

        return false;
    }
    public bool GetKeyDownE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }

        return false;
    }

    public bool MixtableFirstSlot()
    {
        if(_mixTable._firstSlotDragableItem != null)
        {
            return true;
        }
        return false;
    }

    public bool MixtableSecondSlot()
    {
        if (_mixTable._secondSlotDragableItem != null)
        {
            return true;
        }
        return false;
    }


    public void ShowBook()
    {
        _bookIsOpened = true;
        _book.transform.localPosition = new Vector3(0, -0.1f, 0.55f);
    }

    public void HideBook()
    {
        _bookIsOpened = false;
        _book.transform.localPosition = new Vector3(0, -0.1f, -1f);
    }

    public void ContinueGame()
    {
        GameWinCanvas.SetActive(false);
        _playerLocomotion.GameOnPause = false;
        _playerLocomotion.lockedCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator TutorialCoroutine()
    {

        yield return new WaitUntil(GetKeyDownB);
        TutorialText.text = "Это книга с реакциями, которые ты должен провести.\n Закрой книгу (B) и я помогу тебе с твоей первой реакцией!";
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil(GetKeyDownB);
        TutorialText.text = "Видишь подсвеченный объект? \n Открой дверь шкафа и возьми подсвеченный объект";
        TutorialIngridientFirst.gameObject.SetActive(true);
        
        TutorialIngridientFirst._thisObjectOutline.enabled = true;
        yield return new WaitUntil(GetKeyDownE);
        TutorialText.text = "Супер, теперь подойди к столу для проведений реакций, и поставь объект в первый слот!";
        yield return new WaitUntil(MixtableFirstSlot);
        TutorialText.text = "Хорошо, теперь возьми второй подсвеченный объект и поставь во второй слот!";
        TutorialIngridientSecond.gameObject.SetActive(true);
        TutorialIngridientSecond._thisObjectOutline.enabled = true;
        yield return new WaitUntil(MixtableSecondSlot);
        TutorialText.text = "Отлично справляешься, теперь зайди в меню химического стола и нажми кнопку 'Смешать'!";
        yield return new WaitUntil(GetKeyDownF);
        TutorialText.text = "У тебя получилось! Проверь свою книгу, одна из реакций должна быть вычеркнута!\n Кстати на второй этаж тоже можно зайти!";
        yield return new WaitUntil(GetKeyDownB);
        TutorialText.text = "Подсказка:\n Не забывай заглядывать в ящики!\n Нажми E, чтобы закрыть это окно";
        yield return new WaitUntil(GetKeyDownE);
        TutorialCanvas.SetActive(false);
        GameControlCanvas.SetActive(true);
        yield return null;
    }
    public void CheckNewElement(string element)
    {
        for (int i = 0; i < _book.reactList.Count; i++)
        {
            if (element == _book.reactList[i].ReactFormule)
            {
                Instantiate(_book.reactList[i].ReactionResult, _mixTable.ResultElementPoint.position, Quaternion.identity);
                Source.clip = Good;
                Source.Play();
                _book.EnableCheckMark(i);
                for (int j = 0; j < currentReactList.Count; j++)
                {
                    if (element == currentReactList[j].ReactFormule)
                    {
                        _mixTable.NewElementCreated.Play();
                        currentReactList.Remove(currentReactList[j]);
                    }
                }
                CheckGameWinCondition();

                break;
            }
            else if (element != _book.reactList[i].ReactFormule)
            {
                Source.clip = Bad;
                Source.Play();
                _mixTable.ElementNotCreated.Play();

            }
            
        }
    }


    private void CheckGameWinCondition()
    {
       if(currentReactList.Count<= 0)
       {
           Debug.Log("you win");
            GameWinCanvas.SetActive(true);
            _playerLocomotion.GameOnPause = true;
            _playerLocomotion.StepPause();
            _playerLocomotion.lockedCursor = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
