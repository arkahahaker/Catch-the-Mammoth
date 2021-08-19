using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DragableTile))]
public class TileCustomInspector : Editor
{

    
    public override void OnInspectorGUI() {

        DragableTile tile = (DragableTile)target;

        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Resize")) {
            tile.ResizeInMoment();
        }
        if (GUILayout.Button("Turn")) {
            tile.TurnInMoment();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Tip")) {
            tile.SavePositionForTip();
        }
        if (GUILayout.Button("Save Home")) {
            tile.SavePositionForHome();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Tip")) {
            tile.LoadPositionForTip();
        }
        if (GUILayout.Button("Load Home")) {
            tile.LoadPositionForHome();
        }
        GUILayout.EndHorizontal();
    }

}