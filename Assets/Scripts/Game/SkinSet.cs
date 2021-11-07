using System.Collections.Generic;
using UnityEngine;

public class SkinSet : MonoBehaviour {

    public static SkinSet Current;

    private void Start() {
        if (Current != null)
            Destroy(Current.gameObject);
        Current = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<GameObject> Skins;
}