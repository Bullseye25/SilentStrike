using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnvironmentLockController : MonoBehaviour {
	public int EnvironmentId;
	public GameObject lockOverLay;
	public Text priceText;
	void OnEnable(){
		PlayerDataSerializeable pDta = PlayerDataController.Instance.playerData;
		priceText.text = pDta.envioronmentList [EnvironmentId-1].UnlockPrice.ToString();

		for (int i = 0; i < pDta.envioronmentList.Count; i++) {
			if (pDta.envioronmentList [i].ID == EnvironmentId && !pDta.envioronmentList [i].isLocked) {
				lockOverLay.SetActive (false);
			}
		}
	}
}
