using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipManager : MonoBehaviour
{

    private TileZone TileZone;
    private Button TipButton;
    private Text TipsAmountText;
    private Image BuyTipButton;

    private void Awake() {
        TipButton = GetComponent<Button>();
    }

    private void Start() {
        TileZone = FindObjectOfType<TileZone>();
        TipsAmountText = transform.GetChild(0).GetComponent<Text>();
        BuyTipButton = transform.GetChild(1).GetComponent<Image>();
        if (!PlayerPrefs.HasKey("Tips"))
            SetTipsCount(5);
        RefreshButton();
    }

    private void RefreshButton () {
        TipsAmountText.text = GetTipsCount().ToString();
        TipButton.interactable = GetTipsCount() != 0;
    }

    public void UseTip () {
        if (LevelsManager.isActive && !TileZone.AllSet() && TileMenuScroll.Singleton.canActing()) {
            StartCoroutine(TileZone.GetRandomUnsetTile().SetTileByTip());
            SpendOneTip();
            RefreshButton();
        }
    }

    public void BuyTip () {
        AdsManager.isReady = true;
        AdsManager.Singleton.ShowRewardedVideo();
        RefreshButton();
    }

    public void BuyTips(int count) {
        SetTipsCount(GetTipsCount() + count);
        RefreshButton();
    }

    private int GetTipsCount() {
        return PlayerPrefs.GetInt("Tips");
    }

    private void SetTipsCount(int count) {
        PlayerPrefs.SetInt("Tips", count);
    }

    private void SpendOneTip () {
        SetTipsCount(GetTipsCount() - 1);
    }

}