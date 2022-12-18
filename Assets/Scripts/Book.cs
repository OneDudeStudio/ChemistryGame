using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct React
{
    public int ReactIndex;
    public string ReactNumber;
    public string ReactFormule;
    public Image CheckMark;
    public TextMeshProUGUI ReactText;
    public GameObject ReactionResult;

}


public class Book : MonoBehaviour
{
    public List<React> reactList = new List<React>();


    private void Start()
    {
        for (int i = 0; i < reactList.Count; i++)
        {
            reactList[i].ReactText.text = reactList[i].ReactNumber + reactList[i].ReactFormule;
            reactList[i].CheckMark.gameObject.SetActive(false);
        }
    }

    public void EnableCheckMark(int index)
    {
        reactList[index].ReactText.fontStyle = FontStyles.Strikethrough;
        reactList[index].CheckMark.gameObject.SetActive(true);
    }
}
