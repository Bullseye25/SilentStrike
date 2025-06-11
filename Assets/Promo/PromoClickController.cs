using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PromoClickController : MonoBehaviour {

	public List<Sprite> promoSpriteList;
	PromoDataSerializeableScript promo ;
	public Image promoImage;


	// Use this for initialization
	void OnEnable() {

		refreshPromoData ();
	}



	void refreshPromoData(){

		int count = 0;
		do{
			promo =
				PromoDataController.Instance.playerData.PromoListList [Random.Range (0, PromoDataController.Instance.playerData.PromoListList .Count)];
			count++;
		}while(promo!= null && promo.isPromoClicked && count < PromoDataController.Instance.playerData.PromoListList .Count*4 );

		if (promo == null || promo.isPromoClicked) {
			promoImage.gameObject.SetActive (false);
		} else {
			promoImage.gameObject.SetActive (true);
			promoImage.sprite = promoSpriteList [promo.Id - 1];
		}

	}

	public void PromoteGame(){
		for (int i = 0; i <PromoDataController.Instance.playerData.PromoListList.Count; i++) {
			if(PromoDataController.Instance.playerData.PromoListList[i].Id == promo.Id){
				PromoDataController.Instance.playerData.PromoListList [i].isPromoClicked = true;
			}
		}
		//PromoDataController.Instance.Save ();
		Application.OpenURL(promo.gameLink);

		refreshPromoData ();

		//UnityAnalyticsScript.instance.AddUnityEvent ("PromoClicked", new Dictionary<string, object>{
		//});
	}
}
