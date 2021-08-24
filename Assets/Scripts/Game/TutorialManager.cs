using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    private void Start() {
        RefreshText();
    }

    public void RefreshText() {
        Text tutorial = GameObject.Find("TutorialText").GetComponent<Text>();

        if (tutorial != null) {
            tutorial.text = LanguageManager.language.tutorials[Game.CurrentLevel - 1];
            Font f = LanguageManager.font;
            if (f != null) tutorial.font = f; else Debug.Log("Font doesn't loaded");
        }
    }

}