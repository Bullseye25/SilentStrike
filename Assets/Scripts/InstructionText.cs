using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionText : MonoBehaviour
{
    public Sprite SpriteToDisplay;
    public Image InstrutionImage;
    public string MissionStatment = "";
    public bool IsTexttype;
    IEnumerator ShowMessageText()
    {
        yield return new WaitForSeconds(1.5f);

        InstrutionImage.gameObject.SetActive(true);
        InstrutionImage.sprite = SpriteToDisplay;
        yield return new WaitForSeconds(4f);
        InstrutionImage.gameObject.SetActive(false);

    }
    private void Start()
    {
        InstrutionImage = HudMenuManager.instance.InstrutionImage;
    }
    private void OnEnable()
    {
        if (IsTexttype == false)
        {
            StartCoroutine("ShowMessageText");
        }
        else
        {
            HudMenuManager.instance.MissionStatmentText.text = MissionStatment;
            HudMenuManager.instance.missionInfoPanel.SetActive(true);
        }
    }
}
