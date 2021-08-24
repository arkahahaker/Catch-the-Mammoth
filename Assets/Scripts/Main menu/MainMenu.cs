using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    #region Game Setups

    [SerializeField] private List<Color> Colors;

    #endregion

    [SerializeField] private Text languageChangeLable;

    private void Start() {
        Application.targetFrameRate = 60;
        Game.isActive = false;

        LanguageManager.ChooseLanguage();
        //if (!Game.AudioManager.IsPlaying("MainMenu"))
        Game.AudioManager.Loop("MainMenu");

    }

    public void Play () {
        Game.BackgroundColors = Colors;
        SceneManager.LoadScene("LevelsMenu");
    }

    public void ChangeLanguage () {
        LanguageManager.NextLanguage();
        languageChangeLable.text = LanguageManager.getLangSufix().ToUpper();
    }

    public void Exit () {
        Application.Quit();
    }

}