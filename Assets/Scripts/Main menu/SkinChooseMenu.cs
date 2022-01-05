using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinChooseMenu : MonoBehaviour {

    [SerializeField] private List<GameObject> Clothes;
    [SerializeField] private List<Button> Buttons;

    [SerializeField] private Text WillBeSoonText;
    [SerializeField] private Text Skin2GetDescribe;
    [SerializeField] private Text WatchAdText;

    private void Start() {
        foreach (var cloth in Clothes) {
            cloth.SetActive(false);
        }
        Clothes[SkinSetsManager.SkinSetNumber].SetActive(true);
    }

    public void ChooseSkinPack(int number) {
        SkinSetsManager.Singleton.ChangeSkinSet(number);
        foreach (var cloth in Clothes) {
            cloth.SetActive(false);
        }
        Clothes[number].SetActive(true);
        SkinSetsManager.Singleton.ChangeSkinSet(number);
    }

    public void RefreshLanguage() {
        WillBeSoonText.text = LanguageManager.language.willBeSoonText;
        Skin2GetDescribe.text = LanguageManager.language.skin2Text;
        WatchAdText.text = LanguageManager.language.watchAdText;
    }
    
}