using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetsManager : MonoBehaviour
{

    public static SkinSetsManager Singleton;
    public static SkinSet SkinSet;

    [SerializeField] SkinSet skinSet0;
    [SerializeField] SkinSet skinSet1;
    [SerializeField] SkinSet skinSet2;
    
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

}