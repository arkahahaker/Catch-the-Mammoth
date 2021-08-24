using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game 
{

    public static int CurrentLevel;
    public static bool isActive;

    public static int LevelsCount = 10;

    public static List<Color> BackgroundColors;

    public static Color ChoosenColor;

    public static Archive Archive;
    public static AudioManager AudioManager;
    public static AdsManager AdsManager;

    public static bool IsLevelCompleted () {
        return CurrentLevel <= PlayerPrefs.GetInt("CompletedLevels");
    }

}