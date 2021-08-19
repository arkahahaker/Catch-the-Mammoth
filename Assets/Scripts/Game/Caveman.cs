using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caveman : MonoBehaviour
{

    private RectTransform Skin;
    private RectTransform SkinLocator;
    private RectTransform Own;

    private Vector2 sizeSkinRatio;
    private Vector2 startOwnSize;
    private Vector2 startSkinPos;

    private void Start() {
        SkinLocator = transform.GetChild(0).GetComponent<RectTransform>();
        Skin = SkinLocator.transform.GetChild(0).GetComponent<RectTransform>();
        Own = GetComponent<RectTransform>();
        sizeSkinRatio = new Vector2(Skin.sizeDelta.x / Own.sizeDelta.x, Skin.sizeDelta.y / Own.sizeDelta.y);
        startSkinPos = Skin.localPosition;
        startOwnSize = Own.sizeDelta;
    }

    private void Update () {
        Skin.sizeDelta = Vector2.Scale(Own.sizeDelta, sizeSkinRatio);
        SkinLocator.localPosition = Vector2.Scale(startSkinPos, new Vector2(
                (Own.sizeDelta.x / startOwnSize.x - 1),
                (Own.sizeDelta.y / startOwnSize.y - 1)
            )
        );
    }

}