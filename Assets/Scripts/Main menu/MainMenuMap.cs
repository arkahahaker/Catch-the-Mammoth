using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuMap : MonoBehaviour
{

    private Animator MapAnimator;
    private Text TapTheMapToPlayText;

    private void Awake () {
        MapAnimator = GetComponent<Animator>();
        TapTheMapToPlayText = transform.GetChild(0).GetComponent<Text>();
    }

    public IEnumerator ExtendMap () {
        Destroy(transform.GetChild(0).gameObject);
        MapAnimator.SetTrigger("Extend");
        yield return new WaitForSeconds(2f/3f); // 2/3 of second animation lasts
    }

    public void RefreshText () {
        TapTheMapToPlayText.text = LanguageManager.language.tapTheMapToPlay;
        TapTheMapToPlayText.font = LanguageManager.font;
    }

}