using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChangeButton : MonoBehaviour {

    private Image image;

    [SerializeField] private Sprite[] flags;

    private void Awake () {
        image = GetComponent<Image>();
    }

    private void Start () {
        ChangeSprite();
    }    

    public void ChangeLanguage () {
        LanguageManager.NextLanguage();
        ChangeSprite();
    }

    private Sprite GetButtonSprite() {
        return flags[LanguageManager.getLangNumber()];
    }

    private void ChangeSprite () {
        image.sprite = GetButtonSprite();
    }

}