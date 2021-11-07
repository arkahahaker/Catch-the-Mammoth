using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DragableTile))]
[CanEditMultipleObjects]
public class TileCustomInspector : Editor
{
    
    public override void OnInspectorGUI() {

        Object[] objs = targets;

        base.OnInspectorGUI();

        serializedObject.Update();

        DragableTile[] tiles = new DragableTile[objs.Length];
        for (int i = 0; i < objs.Length; i++)
            tiles[i] = (DragableTile)objs[i];

        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Resize")) {
            foreach (DragableTile tile in tiles)
                tile.ResizeInMoment();
        }
        if (GUILayout.Button("Turn")) {
            foreach (DragableTile tile in tiles)
                tile.TurnInMoment();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Tip")) {
            foreach (DragableTile tile in tiles)
                tile.SavePositionForTip();
        }
        if (GUILayout.Button("Save Home")) {
            foreach (DragableTile tile in tiles)
                tile.SavePositionForHome();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Tip")) {
            foreach (DragableTile tile in tiles)
                tile.LoadPositionForTip();
        }
        if (GUILayout.Button("Load Home")) {
            foreach (DragableTile tile in tiles)
                tile.LoadPositionForHome();
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

}