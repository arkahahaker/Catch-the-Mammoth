using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    [SerializeField] private Image progressBar;
    [SerializeField] private Image progressBarFiller;
    [SerializeField] private Text levelsCompletedText;
    
    void Start()
    {
        RefreshText();
        progressBarFiller.fillAmount = (float)LevelsManager.LevelsCompletedCount / LevelsManager.LevelsCount;
    }

    public void RefreshText () {
        levelsCompletedText.text = LevelsManager.LevelsCompletedCount + " / " + LevelsManager.LevelsCount + " " + LanguageManager.language.levelsCompleted;
        levelsCompletedText.font = LanguageManager.font;
    }

}