using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutorialManager : MonoBehaviour {

    private void Start() {
        RefreshText();
    }

    public void RefreshText() {
        Text tutorial = Resources.FindObjectsOfTypeAll<Text>().Where(t => { return t.gameObject.name == "TutorialText"; }).First();

        if (tutorial != null) {
            tutorial.text = LanguageManager.language.tutorials[LevelsManager.CurrentLevel - 1];
            Font f = LanguageManager.font;
            if (f != null) tutorial.font = f; else Debug.Log("Font doesn't loaded");
        }
    }

}