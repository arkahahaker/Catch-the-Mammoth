using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    
    void Start()
    {

        Text tutorial = GameObject.Find("TutorialText").GetComponent<Text>();

        if (tutorial != null) { 
            tutorial.text = LanguageManager.language.tutorials[Game.CurrentLevel - 1];
            Font f = Resources.Load<Font>("Font/" + LanguageManager.language.fontName);
            if (f != null) tutorial.font = f; else Debug.Log("Font doesn't loaded");
            Debug.Log(LanguageManager.language.fontName);
            Debug.Log(f);
            Debug.Log(tutorial.font);
        } else {
            Debug.Log("Doesn't found");
        }
        
    }

}