using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Leaf))]
[CanEditMultipleObjects]
public class LeavesEditor : Editor
{

    public override void OnInspectorGUI() {

        Object[] leaves = targets;

        base.OnInspectorGUI();

        serializedObject.Update();

        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save start")) {
            foreach (Leaf leaf in leaves) {
                leaf.startPosition = leaf.transform.localPosition;
                leaf.startRotation = leaf.transform.localRotation;
            }
            
        }
        if (GUILayout.Button("To start")) {
            foreach (Leaf leaf in leaves) {
                leaf.transform.localPosition = leaf.startPosition;
                leaf.transform.localRotation = leaf.startRotation;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save inter")) {
            foreach (Leaf leaf in leaves) {
                leaf.intermediatePosition = leaf.transform.localPosition;
                leaf.intermediateRotation = leaf.transform.localRotation;
            }
        }
        if (GUILayout.Button("To inter")) {
            foreach (Leaf leaf in leaves) {
                leaf.transform.localPosition = leaf.intermediatePosition;
                leaf.transform.localRotation = leaf.intermediateRotation;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save final")) {
            foreach (Leaf leaf in leaves) {
                leaf.finalPosition = leaf.transform.localPosition;
                leaf.finalRotation = leaf.transform.localRotation;
            }
        }
        if (GUILayout.Button("To final")) {
            foreach (Leaf leaf in leaves) {
                leaf.transform.localPosition = leaf.finalPosition;
                leaf.transform.localRotation = leaf.finalRotation;
            }
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

}