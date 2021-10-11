using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener {

    private static readonly string adsID = "4224728";//4224728
    private static readonly string interstitial = "Interstitial_Android";
    private static readonly string rewarded = "Rewarded_Android";

    private static readonly bool testmode = false;

    public static AdsManager Singleton;
    private void Awake() {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        checkInit();
        Advertisement.AddListener(this);
        Advertisement.Load(interstitial);
        Advertisement.Load(rewarded);
    }

    public void ShowCommonVideo() {
        StartCoroutine(LoadCommonVideo());
    }

    public IEnumerator LoadCommonVideo () {
        checkInit();
        if (!Advertisement.IsReady(interstitial)) {
            Advertisement.Load(interstitial);
            yield return new WaitUntil(() => Advertisement.IsReady(interstitial));
        }
        Advertisement.Show(interstitial);
    }

    public void ShowRewardedVideo () {
        StartCoroutine(LoadRewardedVideo());
    }

    public IEnumerator LoadRewardedVideo() {
        checkInit();
        Advertisement.Load(rewarded);
        yield return new WaitUntil(() => Advertisement.IsReady(rewarded));
        Advertisement.Show(rewarded);
    }

    private void checkInit () {
        if (!Advertisement.isInitialized) {
            Advertisement.Initialize(adsID, testmode);
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (placementId == rewarded && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED)) {
            TipManager tips = FindObjectOfType<TipManager>();
            tips.BuyTips(2);
        }
    }

    public void OnUnityAdsAdLoaded(string placementId) {}
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        Debug.LogError(placementId + " error: " + error.ToString() + " " + message + ".");
    }

    public void OnUnityAdsReady(string placementId) {
    }

    public void OnUnityAdsDidError(string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        throw new System.NotImplementedException();
    }
}