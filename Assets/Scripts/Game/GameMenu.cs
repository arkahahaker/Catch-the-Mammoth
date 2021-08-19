using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    [SerializeField] private GameObject InGameMenu;
    [SerializeField] private GameObject WinMenu;
    [SerializeField] private GameObject LoseMenu;
    [SerializeField] private Image FadingPanel1;
    [SerializeField] private Image FadingPanel2;
    [SerializeField] private TIleMenuScroll Scroll;

    private void Start() {
        Singleton = this;
        StartCoroutine(FadeLevel());
    }

    private static GameMenu Singleton;

    public static void ShowWinMenu () {
        Singleton.WinMenu.SetActive(true);
    }

    public static void ShowLoseMenu () {
        Singleton.LoseMenu.SetActive(true);
    }

    public static void ShowInGameMenu () {
        Singleton.InGameMenu.SetActive(true);
    }

    public void NextLevel () {
        if (Game.CurrentLevel != Game.LevelsCount) {
            SceneManager.LoadScene("Level" + (Game.CurrentLevel + 1));
            Game.CurrentLevel++;
        } else {
            SceneManager.LoadScene("LevelsMenu");
        }
    }

    public void Restart () {
        SceneManager.LoadScene("Level" + Game.CurrentLevel);
    }

    public void ToMenu () {
        SceneManager.LoadScene("LevelsMenu");
    }

    public void ContinueGame () {
        InGameMenu.SetActive(false);
    }

    public IEnumerator FadeLevel() {

        FadingPanel1.gameObject.SetActive(true);
        FadingPanel2.gameObject.SetActive(true);

        int framesCount = 100;

        float time = 1f;

        float framesTime = time / framesCount;

        GameObject.FindObjectOfType<ColorManager>().Generate();

        Color fp1Goal = Game.ChoosenColor, fp2Goal = GameObject.Find("TileZone").transform.GetChild(0).GetComponent<Image>().color;

        Vector3 ct1 = new Vector3(fp1Goal.r - FadingPanel1.color.r, fp1Goal.g - FadingPanel1.color.g, fp1Goal.b - FadingPanel1.color.b) / framesCount;

        Vector3 ct2 = new Vector3(fp2Goal.r - FadingPanel2.color.r, fp2Goal.g - FadingPanel2.color.g, fp2Goal.b - FadingPanel2.color.b) / framesCount;

        for (int i = 0; i <= framesCount; i++) {
            FadingPanel1.color = new Color(FadingPanel1.color.r + ct1.x, FadingPanel1.color.g + ct1.y, FadingPanel1.color.b + ct1.z);
            FadingPanel2.color = new Color(FadingPanel2.color.r + ct2.x, FadingPanel2.color.g + ct2.y, FadingPanel2.color.b + ct2.z);
            yield return new WaitForSeconds(framesTime);
        }

        int transpFramesCount = 100;

        float transpTime = 1f;

        float transpFramesTime = transpTime / transpFramesCount;

        for (int i = 0; i <= transpFramesCount; i++) {
            FadingPanel1.color = new Color(FadingPanel1.color.r, FadingPanel1.color.g, FadingPanel1.color.b, 1 - i / (float)transpFramesCount);
            FadingPanel2.color = new Color(FadingPanel2.color.r, FadingPanel2.color.g, FadingPanel2.color.b, 1 - i / (float)transpFramesCount);
            yield return new WaitForSeconds(transpFramesTime);
        }

        FadingPanel1.gameObject.SetActive(false);
        FadingPanel2.gameObject.SetActive(false);

    }

}