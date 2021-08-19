using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DragableTileNotTurnable : DragableTile {


    #region OnClick
    public override void OnPointerClick(PointerEventData eventData) {
        if (isDraging)
            return;
        if (isSet) {
            RemoveFromMap();
            StartCoroutine(ReturnToStartPosition());
        }
    }
    #endregion


}