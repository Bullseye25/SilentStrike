using UnityEngine.UI;
using UnityEngine;

public class AttributesCtri : MonoBehaviour
{
    public Image damage, capacity, firerate, acuracy, range;
    public float damageMax, capacityMax, firerateMax, acuracyMax, rangeMax;
    public float damageToShow, capacityToShow, firerateToShow, acuracyToShow, rangeToShow;
    public Text damageToShowTxt, capacityToShowTxt, firerateToShowTxt, acuracyToShowTxt, rangeToShowTxt;

    private void OnEnable()
    {
        damage.fillAmount = damageToShow / damageMax;
        capacity.fillAmount = capacityToShow / capacityMax;
        firerate.fillAmount = firerateToShow / firerateMax;
        acuracy.fillAmount = acuracyToShow / acuracyMax;
        range.fillAmount = rangeToShow / rangeMax;

        damageToShowTxt.text = damageToShow + "";
        capacityToShowTxt.text = capacityToShow + "";
        firerateToShowTxt.text = firerateToShow + "";
        acuracyToShowTxt.text = acuracyToShow + "";
        if (rangeToShow == 0)
        {
            rangeToShowTxt.text = "No Scope";
        }
        else
        {
            rangeToShowTxt.text = rangeToShow + "x";
        }
    }
}
