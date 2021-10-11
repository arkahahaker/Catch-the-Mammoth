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
        Application.targetFrameRate = 120;

        LanguageManager.ChooseLanguage();

        AudioManager.Singleton.Loop("MainMenu");
    }

    public void Play() {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine () {
        LevelsManager.BackgroundColors = Colors;
        yield return StartCoroutine(ZoomMap());
        CustomSceneManager.Singleton.LoadScene("LevelsMenu");
    }

    public void ChangeLanguage () {
        LanguageManager.NextLanguage();
        languageChangeLable.text = LanguageManager.getLangSufix().ToUpper();
    }

    public IEnumerator ZoomMap() {

        AudioManager.Singleton.Play("Zoom");

        const int zoomFramesCount = 100;

        const float zoomTime = 1f;

        CanvasScaler canv = GameObject.Find("Canvas").GetComponent<CanvasScaler>();

        Vector2 startRes = canv.referenceResolution;

        Vector2 endRes = new Vector2(canv.referenceResolution.x, 200);

        Vector2 resByFrame = (endRes - startRes) / zoomFramesCount;

        for (int i = 0; i <= zoomFramesCount; i++) {
            canv.referenceResolution = canv.referenceResolution + resByFrame;
            yield return new WaitForSeconds(zoomTime / zoomFramesCount);
        }
    }

    public void Exit () {
        Application.Quit();
    }

}