using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{

    private List<Color> Colors;
    public bool generated = false;

    private void Start() {
        Generate();
    }

    public void Generate () {
        if (generated) return;

        if (Game.BackgroundColors == null) {
            Color color;
            ColorUtility.TryParseHtmlString("#96a952ff", out color);
            Game.BackgroundColors = new List<Color> { color };
        }

        Colors = Game.BackgroundColors;
        Color selected = GenerateRandomColor();
        Game.ChoosenColor = selected;
        Color forTiles = new Color(selected.r - .05f, selected.g - .05f, selected.b - .05f);
        List<DragableTile> tiles = FindObjectOfType<TileZone>().tiles;
        foreach (var tile in tiles) {
            tile.GetComponent<Image>().color = forTiles;
        }
        GetComponent<Image>().color = selected;
        generated = true;
    }

    private Color GenerateRandomColor () {
        int rand = (new System.Random()).Next(0, Colors.Count - 1);
        return Colors[rand];
    }

}