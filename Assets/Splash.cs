using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Splash : MonoBehaviour
{
    public Image fillBar;
    private void OnEnable()
    {
       //f//illBar.DOFillAmount(1, 4f).SetEase(Ease.Linear);
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Loadscene", 1f);
    }
    void Loadscene()
    {
        //Application.LoadLevelAsync("UIScene");
        SceneManager.LoadSceneAsync("UIScene",LoadSceneMode.Single);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
