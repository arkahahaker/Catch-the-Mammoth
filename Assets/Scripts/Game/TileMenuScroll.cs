using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TileMenuScroll : MonoBehaviour
{

    public static TileMenuScroll Singleton;

    public Text LevelTitle;
    public Text StatusTitle;

    public Text ContinueText;
    public Text NextLevelText;
    public Text ToMenuText;

    private bool turning;
    private TileZone tileZone;

    [SerializeField] List<GameObject> MenuObjects;
    [SerializeField] List<GameObject> TileSideObjects;

    private bool isMenu;
    public bool IsMenu { get { return isMenu; } set { isMenu = value; } }

    private void Start () {
        Singleton = this;
        turning = false;
        isMenu = false;
        tileZone = GameObject.FindObjectOfType<TileZone>();
        RefreshMenu();
    }

    public void CompleteLevel() {
        StatusTitle.text = LanguageManager.rand(LanguageManager.language.statusCompleted);
        Turn();
    }

    public void Turn () {
        if (!turning)
            StartCoroutine(TurnCoroutine());
    }

    public void RefreshMenu() {
        StatusTitle.text = LanguageManager.rand(Game.IsLevelCompleted() ?
            LanguageManager.language.statusCompleted :
            LanguageManager.language.statusInProgress);
        LevelTitle.text = LanguageManager.language.level + " " + Game.CurrentLevel;
        ContinueText.text = LanguageManager.language.continueT;
        ToMenuText.text = LanguageManager.language.toMenu;
        NextLevelText.text = LanguageManager.language.nextLevel;
    }

    /// <summary>
    /// True if isn't turning and isn't in menu mode, 
    /// so we can act with tiles and tips
    /// </summary>
    /// <returns></returns>
    public bool canActing () {
        return !isMenu && !turning;
    }

    private IEnumerator TurnCoroutine () {
        turning = true;

        int frames = 180;
        float turnTime = 1f;

        int halfFrames = frames / 2;
        float frameTime = turnTime / frames;

        for (int i = 0; i < halfFrames; i++) {
            transform.Rotate(Vector3.up, 1f);
            yield return new WaitForSeconds(frameTime);
        }
        foreach (GameObject menuObj in MenuObjects) menuObj.SetActive(!isMenu);
        foreach (GameObject tsObj in TileSideObjects) tsObj.SetActive(isMenu);
        for (int i = 0; i < halfFrames; i++) {
            transform.Rotate(Vector3.up, 1f);
            yield return new WaitForSeconds(frameTime);
        }
        turning = false;
        isMenu = !isMenu;
    }

    

}