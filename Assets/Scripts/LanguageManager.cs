using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class LanguageManager {

    public static Language language = new Language();
    public static Font font;

    public static int languagesCount = 3;

    public static void NextLanguage() {
        PlayerPrefs.SetInt("Language", PlayerPrefs.GetInt("Language") == languagesCount - 1 ? 0 : PlayerPrefs.GetInt("Language") + 1);
        ApplyLanguage();
    }

    public static void ChooseLanguage() {
        if (!PlayerPrefs.HasKey("Language")) {
            if (Application.systemLanguage == SystemLanguage.Ukrainian)
                PlayerPrefs.SetInt("Language", (int)Langs.ukr);
            else if (Application.systemLanguage == SystemLanguage.Russian  || Application.systemLanguage == SystemLanguage.Russian)
                PlayerPrefs.SetInt("Language", (int)Langs.rus);
            else
                PlayerPrefs.SetInt("Language", (int)Langs.eng);
        }
        ApplyLanguage();
    }

    private static void ApplyLanguage() {
        string json = Resources.Load<TextAsset>("Languages/" + getLangSufix()).ToString();
        language = JsonUtility.FromJson<Language>(json);
        font = Resources.Load<Font>("Font/" + language.fontName);
        RefreshLanguage();
    }

    private static void RefreshLanguage() {
        TutorialManager tutorialManager = Object.FindObjectOfType<TutorialManager>();
        tutorialManager?.RefreshText();

        TileMenuScroll tileMenuScroll = Object.FindObjectOfType<TileMenuScroll>();
        tileMenuScroll?.RefreshMenu();

        ProgressBar progressBar = Object.FindObjectOfType<ProgressBar>();
        progressBar?.RefreshText();

        MainMenuMap map = Object.FindObjectOfType<MainMenuMap>();
        map?.RefreshText();
    }

    public static int getLangNumber () {
        return PlayerPrefs.GetInt("Language");
    }

    public static string getLangSufix () {
        return ((Langs)PlayerPrefs.GetInt("Language")).ToString();
    }

    public static string rand(string[] array) {
        return array[new System.Random().Next(0, array.Length-1)];
    }

    public class Language {

        public string fontName;

        public string level;

        public string toMenu;
        public string continueT;
        public string nextLevel;

        public string levelsCompleted;
        public string tapTheMapToPlay;

        public string[] statusInProgress;
        public string[] statusCompleted;

        public string[] tutorials;

        public override string ToString() {
            return "Language info\n" +
                "Font name: " + fontName +
                "\nLevel: " + level +
                "\nStatus \"in progress\": " + statusInProgress +
                "\nStatus \"Completed\": " + statusCompleted +
                "\nTutorials: " + tutorials;
        }

    }

    public enum Langs {
        ukr = 0,
        rus = 1,
        eng = 2
    }

}