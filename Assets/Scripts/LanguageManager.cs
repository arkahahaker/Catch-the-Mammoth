using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager {

    public static Language language = new Language();

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
        Debug.Log(language.fontName);
        Debug.Log(language.tutorials.ToString());
    }

    public static string getLangSufix () {
        return ((Langs)PlayerPrefs.GetInt("Language")).ToString();
    }

    public class Language {

        public string fontName;

        public string[] tutorials;

    }

    public enum Langs {
        ukr = 0,
        rus = 1,
        eng = 2
    }

}