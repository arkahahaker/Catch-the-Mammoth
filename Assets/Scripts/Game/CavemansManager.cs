using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemansManager : MonoBehaviour {

    [SerializeField]
    private List<GameObject> CavemenPrefabs;

    private void Start() {
        Caveman[] cs = FindObjectsOfType<Caveman>();
        int[] randArr = new int[cs.Length];
        bool[] chosenElls = new bool[CavemenPrefabs.Count];
        foreach (var t in cs)
        {
            int temp;
            do {
                temp = Mathf.RoundToInt(Random.Range(-0.49f, CavemenPrefabs.Count - 0.51f));
            } while (chosenElls[temp]);
            chosenElls[temp] = true;
            GameObject newCav = Instantiate(CavemenPrefabs[temp], t.transform.position, t.transform.rotation, t.transform.parent);
            Destroy(newCav.transform.GetChild(0).GetChild(0).gameObject);
            Instantiate(SkinSetsManager.Singleton.GetSkin(SkinSetsManager.SkinSetNumber, temp), newCav.transform.GetChild(0));
            Destroy(t.gameObject);
            newCav.transform.parent.parent.GetComponent<DragableTile>().Setup();
        }
    }

}
