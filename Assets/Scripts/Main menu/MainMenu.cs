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
    private MainMenuMap map;

    private void Awake () {
        map = FindObjectOfType<MainMenuMap>();
    }

    private void Start() {
        Application.targetFrameRate = 120;

        LanguageManager.ChooseLanguage();

        AudioManager.Singleton.Loop("MainMenu");
    }

    public void Play() {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine () {
        LevelsManager.BackgroundColors = Colors;
        yield return StartCoroutine(map.ExtendMap());
        CustomSceneManager.Singleton.LoadScene("LevelsMenu");
    }

    public void ChangeLanguage () {
        LanguageManager.NextLanguage();
        languageChangeLable.text = LanguageManager.getLangSufix().ToUpper();
    }

    public void Exit () {
        Application.Quit();
    }

}