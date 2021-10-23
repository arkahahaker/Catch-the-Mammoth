using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    #region Params
    public int Columns;
    public int Rows;

    public int StartX { get; set; }
    public int StartY { get; set; }
    public int CagesWidth { get; set; }
    public int CagesHeight { get; set; }    

    public int MammothX { get; set; }
    public int MammothY { get; set; }

    public Cage[,] Cages { get; set; }

    #endregion

    #region Start Actions
    private void Awake() {
        Get = this;
        Initialize();
    }

    private void Initialize () {

        if (AudioManager.Singleton != null) {
            if (AudioManager.Singleton.IsPlaying("MainMenu")) {
                AudioManager.Singleton.Stop("MainMenu");
                AudioManager.Singleton.Loop(Random.Range(0, 2) < 0.5 ? "LevelTheme1" : "LevelTheme2");
            } else if (AudioManager.Singleton.IsPlaying("LevelTheme1")) {
                AudioManager.Singleton.Stop("LevelTheme1");
                AudioManager.Singleton.Loop("LevelTheme2");
            } else if (AudioManager.Singleton.IsPlaying("LevelTheme2")) {
                AudioManager.Singleton.Stop("LevelTheme2");
                AudioManager.Singleton.Loop("LevelTheme1");
            } else {
                AudioManager.Singleton.Loop(Random.Range(0, 1) < 0.5 ? "LevelTheme1" : "LevelTheme2");
            }
        }

        GameObject startCage = GameObject.FindGameObjectWithTag("Start cage");
        CagesWidth = (int)startCage.GetComponent<RectTransform>().rect.width;
        CagesHeight = (int)startCage.GetComponent<RectTransform>().rect.height;
        StartX = (int)startCage.GetComponent<RectTransform>().localPosition.x-CagesWidth/2;
        StartY = (int)startCage.GetComponent<RectTransform>().localPosition.y+CagesHeight/2;
        Cages = new Cage[Columns, Rows];
        Cage[] cages = FindObjectsOfType<Cage>();
        foreach(Cage cage in cages) {
            int cx = (int)cage.GetComponent<RectTransform>().localPosition.x;
            int cy = (int)cage.GetComponent<RectTransform>().localPosition.y;
            cage.X = (cx - StartX) / CagesWidth;
            cage.Y = (StartY - cy) / CagesHeight;
            Cages[cage.X, cage.Y] = cage;
        }
        GameObject Mammoth = GameObject.FindGameObjectWithTag("Mammoth");
        MammothX = (int)(Mammoth.GetComponent<RectTransform>().localPosition.x - StartX) / CagesWidth;
        MammothY = (int)(StartY - Mammoth.GetComponent<RectTransform>().localPosition.y) / CagesHeight;
        LevelsManager.isActive = true;
    }
    #endregion

    #region Tile setting
    public bool CanSetTile(DragableTile tile) {
        bool can = true;
        foreach (GameObject point in tile.RaycastPoints) {
            Vector2 vector = tile.GetRealPointPosition(point);
            Cage cage = CageByVector(vector);
            if (!isFree(cage))
                can = false;
        }
        if (can) {
            Vector2 vector = tile.GetRealPointPosition(tile.Caveman);
            Cage cage = CageByVector(vector);
            if (!isFree(cage))
                can = false;
            
        }
        return can;
    }

    public void SetTile (DragableTile tile) {
        Cage cage = CageByVector(tile.GetRealPointPosition(tile.Caveman));
        tile.TranslateThePointToPoint(tile.Caveman, cage);
        if (CanSetTile(tile)) {
            foreach (GameObject point in tile.RaycastPoints) {
                cage = CageByVector(tile.GetRealPointPosition(point));
                cage.isFree = false;
                tile.OccupiedCages.Add(cage);
            }
            cage = CageByVector(tile.GetRealPointPosition(tile.Caveman));
            cage.isFree = false;
            cage.isCaveman = true;
            tile.OccupiedCages.Add(cage);

            tile.IsSet = true;

            tile.transform.parent = TileZone.Singleton.SetTilesParent;

            WinChecker.CheckWin();
        } else {
            StartCoroutine(tile.ReturnToStartPosition());
        }
    }
    #endregion

    #region Helpers
    public bool isFree(Vector2 location) {
        return isFree(CageByVector(location));
    }

    public bool isFree(Cage c) {
        return !(c == null || !c.isFree);
    }

    public bool isFree(int x, int y) {
        if (isCageExist(x,y))
            return !(Cages[x, y] == null || !Cages[x, y].isFree);
        else return false;
    }

    public Cage CageByVector (Vector2 location) {
        if (location.x > StartX + Columns * CagesWidth || location.x < StartX || location.y < StartY - (Rows+1) * CagesHeight || location.y > StartY)
            return null;
        else if (isCageExist((int)(location.x - StartX) / CagesWidth, (int)(StartY - location.y) / CagesHeight))
            return Cages[(int)(location.x - StartX) / CagesWidth, (int)(StartY - location.y) / CagesHeight];
        else
            return null;
    }

    public Vector2 VectorByCage (Cage c) {
        return new Vector2(StartX + c.X * CagesWidth, StartY - c.Y * CagesHeight);
    }

    public Vector2 VectorByCoordinates(int x, int y) {
        return new Vector2(StartX + x * CagesWidth, StartY - y * CagesHeight);
    }

    public bool isCageExist(int x, int y) {
        return x < Columns && y < Rows && x > -1 && y > -1;
    }

    public bool isCageExist(Vector2 location) {
        return CageByVector(location) != null;
    }
    #endregion

    public static Map Get;

}