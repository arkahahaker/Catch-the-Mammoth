using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public static GameMenu Singleton;

    private bool isActive = true;

    public TileMenuScroll Scroll;

    private void Start() {
        Singleton = this;
    }

    private void Update() {
        if (Input.GetKey (KeyCode.Escape))
            StartCoroutine(CustomSceneManager.Singleton.LoadSceneCurtains("LevelsMenu"));
    }

    public void TurnScroll () {
        Scroll.Turn();
    }

    public void CompleteLevel() {
        Scroll.CompleteLevel();
    }

    public void NextLevel () {
        if (!isActive) return;
        isActive = false;
        if (LevelsManager.CurrentLevel != LevelsManager.LevelsCount) {
            LevelsManager.CurrentLevel++;
            StartCoroutine(CustomSceneManager.Singleton.LoadSceneCurtains("Level" + LevelsManager.CurrentLevel));
        } else {
            StartCoroutine(CustomSceneManager.Singleton.LoadSceneCurtains("LevelsMenu"));
        }
    }

    public void Restart () {
        StartCoroutine(CustomSceneManager.Singleton.LoadSceneCurtains("Level" + LevelsManager.CurrentLevel));
    }

    public void ToMenu () {
        if (!isActive) return;
        isActive = false;
        StartCoroutine(CustomSceneManager.Singleton.LoadSceneCurtains("LevelsMenu"));
    }

}