using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileZone : MonoBehaviour
{

    public List<DragableTile> tiles;

    public void Awake() {
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
            if (!tile.isSet)
                return tile;
        }
    }

    public bool AllSet () {
        foreach (var tile in tiles) {
            if (!tile.isSet)
                return false;
        }
        return true;
    }

}