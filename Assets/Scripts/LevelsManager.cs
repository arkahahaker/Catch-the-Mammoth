using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelsManager {

    public static int CurrentLevel;
    public static bool isActive;

    public static int LevelsCount = 31;

    public static List<Color> BackgroundColors;

    public static Color ChoosenColor;

    public static int LevelsCompletedCount {
        get { return PlayerPrefs.GetInt("CompletedLevels");}    
    }

    public static bool IsLevelCompleted () {
        return CurrentLevel <= LevelsCompletedCount;
    }

    public static bool IsLevelCompleted(int level) {
        return level <= LevelsCompletedCount;
    }

}