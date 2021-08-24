using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public static GameMenu Singleton;

    public TileMenuScroll Scroll;

    private void Start() {
        Singleton = this;
    }

    public void TurnScroll () {
        Scroll.Turn();
    }

    public void CompleteLevel() {
        Scroll.CompleteLevel();
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

}