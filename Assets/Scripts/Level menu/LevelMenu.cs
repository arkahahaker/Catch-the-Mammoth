using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {

    public static Vector2 mapSize;

    private bool isActive = true;

    [SerializeField] private GameObject Map;
    private MapScroller MapScroller { get; set; }

    [SerializeField] private Sprite LevelCompleted;
    [SerializeField] private Sprite LevelLocked;
    [SerializeField] private Sprite CurrentLevel;

    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private Text LevelsToUnlockText;
    [SerializeField] private Text OpenCloseButton;
    [SerializeField] private Slider LevelsToUnlockSlider;
    
    [SerializeField] private GameObject ToMainMenuButton;

    private int LevelsToUnlock = 0;

    private void Start() {

        if (AudioManager.Singleton != null) {
            if (AudioManager.Singleton.IsPlaying("LevelTheme1")) {
                AudioManager.Singleton.Stop("LevelTheme1");
                AudioManager.Singleton.Loop("MainMenu");
            } else if (AudioManager.Singleton.IsPlaying("LevelTheme2")) {
                AudioManager.Singleton.Stop("LevelTheme2");
                AudioManager.Singleton.Loop("MainMenu");
            } else if (!AudioManager.Singleton.IsPlaying("MainMenu")) {
                AudioManager.Singleton.Loop("MainMenu");
            }
        }

        mapSize = Map.GetComponent<RectTransform>().localPosition;
        LevelsStatusSetup();

        LevelsToUnlockSlider.minValue = 1;
        LevelsToUnlockSlider.maxValue = LevelsManager.LevelsCount;

        MapScroller = Map.GetComponent<MapScroller>();
    }

    private void Update() {
        if (Input.GetKey (KeyCode.Escape))
            StartCoroutine (CustomSceneManager.Singleton.LoadSceneCurtains("MainMenu"));
    }

    public void LoadLevel(int level) {
        if (!isActive) return;
        if (PlayerPrefs.GetInt("CompletedLevels") + 1 >= level) {
            isActive = false;
            LevelsManager.CurrentLevel = level;
            LevelsManager.isActive = true;
            ToMainMenuButton.SetActive(false); 
            StopAllCoroutines();
            MapScroller.StopAllCoroutines();
            StartCoroutine(CustomSceneManager.Singleton.LoadSceneCurtains("Level" + level));
        }
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelsStatusSetup() {
        if (!PlayerPrefs.HasKey("CompletedLevels"))
            PlayerPrefs.SetInt("CompletedLevels", 0);
        int levels = PlayerPrefs.GetInt("CompletedLevels");
        GameObject background = GameObject.Find("Background");
        for (int i = 1; i <= levels && i <= LevelsManager.LevelsCount; i++)
            background.transform.GetChild(i - 1).transform.GetChild(0).GetComponent<Image>().sprite = LevelCompleted;
        if (levels != LevelsManager.LevelsCount)
            background.transform.GetChild(levels).transform.GetChild(0).GetComponent<Image>().sprite = CurrentLevel;
        for (int i = levels + 2; i <= LevelsManager.LevelsCount; i++)
            background.transform.GetChild(i - 1).transform.GetChild(0).GetComponent<Image>().sprite = LevelLocked;
    }
    public void UnlockAllLevels() {
        PlayerPrefs.SetInt("CompletedLevels", LevelsManager.LevelsCount);
        SceneManager.LoadScene("LevelsMenu");
    }

    public void LockAllLevels() {
        PlayerPrefs.SetInt("CompletedLevels", 0);
        SceneManager.LoadScene("LevelsMenu");
    }

    public void UnlockLevels() {
        PlayerPrefs.SetInt("CompletedLevels", LevelsToUnlock);
        SceneManager.LoadScene("LevelsMenu");
    }

    public void SetLevelsToUnlock() {
        LevelsToUnlock = (int)LevelsToUnlockSlider.value;
        LevelsToUnlockText.text = "Unlock " + LevelsToUnlock.ToString() + " levels";
    }

    public void GiveTips () {
        if (PlayerPrefs.HasKey("Tips"))
            PlayerPrefs.SetInt("Tips", PlayerPrefs.GetInt("Tips") + 5);
    }

    public void StealTips() {
        PlayerPrefs.SetInt("Tips", 0);
    }

    public void OpenPanel () {
        if (OptionsPanel.activeSelf)
            OpenCloseButton.text = "Open";
        else
            OpenCloseButton.text = "Close";
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    }


}
