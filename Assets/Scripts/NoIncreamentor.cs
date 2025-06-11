using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoIncreamentor : MonoBehaviour
{
    float currentNumber, initialNumber, _desiredNumber;
    bool _startAddingCoins;
    public float coinsAnimationTime = 1f;
    public AudioSource  CoinSound;
    public Text goldText;
    // Update is called once per frame
    void Update()
    {
        if (_startAddingCoins)
        {
            if (currentNumber != _desiredNumber)
            {
                if (initialNumber < _desiredNumber)
                {
                   // if(!CoinSound.isPlaying)
                       // CoinSound.Play();
                    currentNumber += (coinsAnimationTime * Time.deltaTime) * (_desiredNumber - initialNumber);
                    if (currentNumber > _desiredNumber)
                    {
                        if (CoinSound && CoinSound.isPlaying)
                            CoinSound.Stop();

                        currentNumber = _desiredNumber;
                        _startAddingCoins = false;
                    }
                }
                else if(initialNumber > _desiredNumber)
                {
                  //  if (!CoinSound.isPlaying)
                       // CoinSound.Play();
                    currentNumber -= (coinsAnimationTime * Time.deltaTime) * (  initialNumber - _desiredNumber);
                    if (currentNumber < _desiredNumber)
                    {
                        if (CoinSound && CoinSound.isPlaying)
                            CoinSound.Stop();

                        currentNumber = _desiredNumber;
                        _startAddingCoins = false;
                    }
                }
            }


            goldText.text = ((int)currentNumber).ToString();
        }
    }

    public void AddRemoveGold(int initial , int goldAmount)
    {
        Debug.Log(" initial" + initial + "goldAmount "+ goldAmount);
        currentNumber = initialNumber = initial;       
        _desiredNumber = initial+ goldAmount;
        _startAddingCoins = true;
        if (CoinSound &&!CoinSound.isPlaying)
            CoinSound.Play();
    }

    public void AddRemoveGold(int goldAmount)
    {

        currentNumber = initialNumber = PlayerDataController.Instance.playerData.PlayerGold;

        PlayerDataController.Instance.playerData.PlayerGold = (PlayerDataController.Instance.playerData.PlayerGold + goldAmount);
        PlayerDataController.Instance.Save();
        _desiredNumber = PlayerDataController.Instance.playerData.PlayerGold;
        _startAddingCoins = true;
        if (CoinSound && !CoinSound.isPlaying)
            CoinSound.Play();
        //  SetCoinsText(goldAmount);
    }
}
