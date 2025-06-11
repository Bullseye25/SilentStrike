using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Image loadingBar;
    public Text percentageText;

    private int Percentage;
    private bool StartPercentage;
    
    void Start()
    {
        StartPercentage = true;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("UIScene");

    }
    private void Update()
    {
        if (StartPercentage && Percentage < 100)
        {
            Percentage++;
            percentageText.text = Percentage + "%";
        }
        else if (Percentage == 100)
        {
            StartPercentage = false;
            Percentage = 0;
        }

        loadingBar.fillAmount += 0.01f;
    }
}
