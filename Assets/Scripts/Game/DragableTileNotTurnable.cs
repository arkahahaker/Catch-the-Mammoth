using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DragableTileNotTurnable : DragableTile {


    #region OnClick
    public override void OnPointerClick(PointerEventData eventData) {
        if (!TileMenuScroll.Singleton.canActing()) return;
        if (isDraging)
            return;
        if (IsSet) {
            RemoveFromMap();
            StartCoroutine(ReturnToStartPosition());
        }
    }
    #endregion


}