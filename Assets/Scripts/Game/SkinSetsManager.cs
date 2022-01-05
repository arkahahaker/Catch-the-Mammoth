using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetsManager : MonoBehaviour
{

    public static SkinSetsManager Singleton;
    public static SkinSet SkinSet;
    public static int SkinSetNumber;

    [SerializeField] private SkinSet skinSet0;
    [SerializeField] private SkinSet skinSet1;
    [SerializeField] private SkinSet skinSet2;
    
    private void Awake() {

        if (Singleton != null) Destroy(gameObject);
        else {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        SkinSet = gameObject.AddComponent<SkinSet>();
        if (!PlayerPrefs.HasKey("skin")) 
            PlayerPrefs.SetInt("skin", 0);
        ChangeSkinSet(PlayerPrefs.GetInt("skin"));
    }

    public void ChangeSkinSet (int set) {
        SkinSetNumber = set;
        PlayerPrefs.SetInt("skin", set);
        switch (set) {
            case 0:
                GetComponent<SkinSet>().Skins = skinSet0.Skins;
                break;
            case 1:
                GetComponent<SkinSet>().Skins = skinSet1.Skins;
                break;
            case 2:
                GetComponent<SkinSet>().Skins = skinSet2.Skins;
                break;
        }
    }

    public GameObject GetSkin(int set, int skin) {
        switch (set) {
            case 0: return skinSet0.Skins[skin];
            case 1: return skinSet1.Skins[skin];
            default: return skinSet2.Skins[skin];
        }
    }

}