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
        if (GetTipsCount() == 0) {
            TipButton.interactable = false;
            BuyTipButton.gameObject.SetActive(true);
        } else {
            TipButton.interactable = true;
            BuyTipButton.gameObject.SetActive(false);
        }
    }

    public void UseTip () {
        if (Game.isActive && !TileZone.AllSet()) {
            StartCoroutine(TileZone.GetRandomUnsetTile().SetTileByTip());
            SpendOneTip();
            RefreshButton();
        }
    }

    public void BuyTip () {
        Game.AdsManager.ShowRewardedVideo();
        //SetTipsCount(GetTipsCount() + 1);
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