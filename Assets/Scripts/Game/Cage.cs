using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cage : MonoBehaviour {

    #region Params
    public bool isFree;
    public bool isCaveman;

    public int x;
    public int y;

    public DragableTile tile;
    #endregion

    #region Start Actions
    private void Start() {
        isFree = true;
        isCaveman = false;
    }
    #endregion

    #region Helpers
    public override string ToString() {
        return "Cage (" + x + ", " + y + ")";
    }
    #endregion

}