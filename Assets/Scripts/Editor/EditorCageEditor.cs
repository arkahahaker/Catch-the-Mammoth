using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(EditorCage))]
[CanEditMultipleObjects]
public class EditorCageEditor : Editor
{

    public override void OnInspectorGUI() {

        Object[] cages = targets;

        base.OnInspectorGUI();

        serializedObject.Update();

        /*if (GUILayout.Button("Change")) {
            if (cage.gameObject.GetComponent<Cage>())
                DestroyImmediate(cage.gameObject.GetComponent<Cage>());
            else 
                cage.gameObject.AddComponent<Cage>();
        }*/

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("O")) {
            foreach (EditorCage cage in cages) {
                if (!cage.gameObject.GetComponent<Cage>())
                    cage.gameObject.AddComponent<Cage>();
                cage.GetComponent<Image>().sprite = FindObjectOfType<Archive>().FreeCage;
                cage.tag = "Untagged";
            }
        }
        if (GUILayout.Button("C")) {
            System.Random rand = new System.Random();
            foreach (EditorCage cage in cages) {
                if (cage.gameObject.GetComponent<Cage>())
                    DestroyImmediate(cage.gameObject.GetComponent<Cage>());
                cage.GetComponent<Image>().sprite = FindObjectOfType<Archive>().RandomObstacle(rand);
                cage.tag = "Untagged";
            }
        }
        if (GUILayout.Button("M")) {
            foreach (EditorCage cage in cages) {
                if (cage.gameObject.GetComponent<Cage>())
                    DestroyImmediate(cage.gameObject.GetComponent<Cage>());
                cage.GetComponent<Image>().sprite = FindObjectOfType<Archive>().Mammoth;
                cage.tag = "Mammoth";
            }
        }
        if (GUILayout.Button("E")) {
            foreach (EditorCage cage in cages) {
                if (cage.gameObject.GetComponent<Cage>())
                    DestroyImmediate(cage.gameObject.GetComponent<Cage>());
                cage.GetComponent<Image>().sprite = FindObjectOfType<Archive>().Nothing;
                cage.tag = "Untagged";
            }
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed) {
            foreach (EditorCage cage in cages) {
                EditorUtility.SetDirty(cage.gameObject.GetComponent<Image>());
                EditorSceneManager.MarkSceneDirty(cage.gameObject.scene);
            }
        }

    }

}