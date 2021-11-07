using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caveman : MonoBehaviour
{

    private RectTransform Skin; // skin that have animation. DO NOT MOVE IT VIA SCRIPT. Move SkinLocator
    private RectTransform SkinLocator; // child of this object, that is parent of skin, but skin animation
                                       // don't affect it. Using for move skin regarding caveman
    private RectTransform Own; // rectTransform of the object

    private Vector2 sizeSkinRatio;
    private Vector2 startOwnSize;
    private Vector2 startSkinPos;

    public void UpdateSkin() {
        SkinLocator = transform.GetChild(0).GetComponent<RectTransform>();
        Skin = SkinLocator.transform.GetChild(0).GetComponent<RectTransform>();
        Own = GetComponent<RectTransform>();
        sizeSkinRatio = new Vector2(Skin.sizeDelta.x / Own.sizeDelta.x, Skin.sizeDelta.y / Own.sizeDelta.y);
        startSkinPos = Skin.localPosition;
        startOwnSize = Own.sizeDelta;
        Debug.Log(Skin.gameObject.name);
    }

    private void Update () {
        if (Skin == null) UpdateSkin();
        Skin.sizeDelta = Vector2.Scale(Own.sizeDelta, sizeSkinRatio);
        SkinLocator.localPosition = Vector2.Scale(startSkinPos, new Vector2(
                (Own.sizeDelta.x / startOwnSize.x - 1),
                (Own.sizeDelta.y / startOwnSize.y - 1)
            )
        );
    }

}