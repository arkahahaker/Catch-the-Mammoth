using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileZone : MonoBehaviour
{

    public static TileZone Singleton;

    public Transform UnsetTilesParent;
    public Transform SetTilesParent;

    public List<DragableTile> tiles;

    public void Awake() {
        Singleton = this;
        tiles.AddRange(FindObjectsOfType<DragableTile>());
    }

    public DragableTile GetRandomTile () {
        return tiles[(int)Random.Range(0, tiles.Count - .01f)];
    }

    public DragableTile GetRandomUnsetTile() {
        if (AllSet())
            return null;
        while (true) {
            DragableTile tile = GetRandomTile();
            if (!tile.IsSet)
                return tile;
        }
    }

    public bool AllSet () {
        foreach (var tile in tiles) {
            if (!tile.IsSet)
                return false;
        }
        return true;
    }

}