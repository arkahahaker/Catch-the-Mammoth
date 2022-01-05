using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour, IUnityAdsShowListener {

    private static readonly string adsID = "4224728";//4224728
    private static readonly string interstitial = "Interstitial_Android";
    private static readonly string rewarded = "Rewarded_Android";

    private static readonly bool testmode = false;

    public static bool isReady = false;

    public static AdsManager Singleton;
    public static bool ForSkin = false;
    private void Awake() {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // If the user opts out of targeted advertising:
        MetaData gdprMetaData = new MetaData("gdpr");
        gdprMetaData.Set("consent", "false");
        Advertisement.SetMetaData(gdprMetaData);
        
        checkInit();
        Advertisement.Load(interstitial);
        Advertisement.Load(rewarded);
    }

    public void ShowRewardedVideo () {
        StartCoroutine(LoadRewardedVideo());
    }

    public IEnumerator LoadRewardedVideo() {
        checkInit();
        Advertisement.Load(rewarded);
        Advertisement.Initialize(rewarded);
        yield return new WaitUntil(() => Advertisement.isInitialized);
        Advertisement.Show(rewarded, this);
    }

    private void checkInit() {
        if (!Advertisement.isInitialized) {
            Advertisement.Initialize(adsID, testmode);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {}

    public void OnUnityAdsShowClick(string placementId) {}

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (!isReady) return;
        isReady = false;
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
            if (ForSkin) {
                PlayerPrefs.SetInt("Skin2Progress", PlayerPrefs.GetInt("Skin2Progress")+1);
                SecondSkinGet.Singleton.Refresh();
                ForSkin = false;
            } else {
                TipManager tips = FindObjectOfType<TipManager>();
                tips.BuyTips(1);
            }
        }
    }
}