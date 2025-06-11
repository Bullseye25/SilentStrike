using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinHandler : MonoBehaviour
{
    public Text totalGoldText;
    public NoIncreamentor noIncreamentor;
    public GameObject coin1xAnimation;
    public GameObject coin3xAnimation;
    int initailGold;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerDataController.Instance )
        {
            totalGoldText.text = PlayerDataController.Instance.playerData.PlayerGold.ToString();
            initailGold = PlayerDataController.Instance.playerData.PlayerGold;
        }
    }


    public void OnGoldAddition(int addGold,bool /*is3X*/is2x)
    {
     
        StartCoroutine(CoinAnim(addGold, /*is3X*/is2x));
    }

     IEnumerator CoinAnim(int addGold, bool /*is3X*/is2x)
    {
        
        if (/*is3X*/is2x)
        {
            
            addGold = addGold * 2;
            yield return new WaitForSeconds(0.5f);

            coin1xAnimation.SetActive(!/*is3X*/is2x);
            coin3xAnimation.SetActive(/*is3X*/is2x);
        }
        else
        {
            coin1xAnimation.SetActive(!/*is3X*/is2x);
            coin3xAnimation.SetActive(/*is3X*/is2x);
        }
        PlayerDataController.Instance.playerData.PlayerGold += addGold;

        yield return new WaitForSeconds(2);
        noIncreamentor.AddRemoveGold(initailGold, addGold);
        PlayerDataController.Instance.Save();

    }

}
