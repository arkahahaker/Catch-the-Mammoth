using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour {

    #region Params
    public bool isFree;
    public bool isCaveman;

    public int X;

    public int Y;
    #endregion

    #region Start Actions
    private void Start() {
        isFree = true;
        isCaveman = false;
    }
    #endregion

    #region Helpers
    public override string ToString() {
        return "Cage (" + X + ", " + Y + ")";
    }
    #endregion

}