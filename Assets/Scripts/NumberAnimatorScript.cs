using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberAnimatorScript : MonoBehaviour
{
    float currentNumber, initialNumber, _desiredNumber;
    bool _startAddingCoins;
    public float coinsAnimationTime = 2f;
    public AudioSource CoinSound;
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
                else if (initialNumber > _desiredNumber)
                {
                    //  if (!CoinSound.isPlaying)
                    // CoinSound.Play();
                    currentNumber -= (coinsAnimationTime * Time.deltaTime) * (initialNumber - _desiredNumber);
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

    public void AddRemoveGold(int amountToAdd, int initialAmount)
    {

       
        currentNumber = initialNumber = initialAmount;

     
        _desiredNumber = initialAmount+ amountToAdd;
        _startAddingCoins = true;
        //  SetCoinsText(goldAmount);
    }
}
