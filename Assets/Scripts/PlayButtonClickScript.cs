using UnityEngine;
using UnityEngine.UI;

public class PlayButtonClickScript : MonoBehaviour
{
    private Button _button;
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if(SoundsManager.instance)
        SoundsManager.instance.PlayButtonClickSound();
    }
}
