using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class SecondSkinGet : MonoBehaviour {

	[SerializeField] private List<GameObject> ObjectToRemoveWhenGot;
	[SerializeField] private List<GameObject> Checks;
	[SerializeField] private Button SecondSkinButton;

	public static SecondSkinGet Singleton;
	
	private void Start() {
		Singleton = this;
		if (!PlayerPrefs.HasKey("Skin2Progress"))
			PlayerPrefs.SetInt("Skin2Progress", 0);
		Refresh();
	}

	public void Refresh() {
		int checksCount = PlayerPrefs.GetInt("Skin2Progress");
		for (var i = 1; i <= checksCount; i++) Checks[i-1].SetActive(true);
		if (checksCount == 3) {
			foreach (var obj in ObjectToRemoveWhenGot) {
				Destroy(obj);
			}
			SecondSkinButton.interactable = true;
		} else {
			SecondSkinButton.interactable = false;
		}
	}

	public void WatchAd() {
		AdsManager.isReady = true;
		AdsManager.ForSkin = true;
		StartCoroutine(AdsManager.Singleton.LoadRewardedVideo());
	}

}