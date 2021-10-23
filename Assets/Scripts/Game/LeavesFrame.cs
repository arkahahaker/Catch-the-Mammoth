using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesFrame : MonoBehaviour {

    private void Start() {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }

}