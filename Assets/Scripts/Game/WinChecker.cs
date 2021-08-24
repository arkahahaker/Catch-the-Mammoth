using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    #region Params
    private bool[,] Checked;

    private static WinChecker Singleton;

    private TileZone tileZone;
    #endregion

    #region Start Actions

    private void Awake() {
        tileZone = FindObjectOfType<TileZone>();
    }

    private void Start() {
        Singleton = this;
    }
    #endregion Singleton

    #region Chekers
    public static void CheckWin() {
        if (!Singleton.tileZone.AllSet())
            return;
        Game.isActive = false;
        if (!Singleton.CanEscape()) {
            if (!Game.IsLevelCompleted())
                PlayerPrefs.SetInt("CompletedLevels", Game.CurrentLevel);
            GameMenu.Singleton.CompleteLevel();
        }
    }

    private bool CanEscape () {
        List<Cage> cages = new List<Cage>();
        Checked = new bool[Map.Get.Columns, Map.Get.Rows];
        int startX = Map.Get.MammothX;
        int startY = Map.Get.MammothY;
        if (CanMove(startX, startY + 1)) 
            cages.Add(Map.Get.Cages[startX, startY + 1]);
        if (CanMove(startX + 1, startY))
            cages.Add(Map.Get.Cages[startX + 1, startY]);
        if (CanMove(startX, startY - 1))
            cages.Add(Map.Get.Cages[startX, startY - 1]);
        if (CanMove(startX - 1, startY))
            cages.Add(Map.Get.Cages[startX - 1, startY]);
        while (cages.Count>0) {
            Cage cage = cages[0];
            cages.RemoveAt(0);
            if (IsWinnerCage(cage)) {
                FindWay(cage);
                return true;
            }
            if (CanMove(cage.X, cage.Y + 1))
                cages.Add(Map.Get.Cages[cage.X, cage.Y + 1]);
            if (CanMove(cage.X + 1, cage.Y))
                cages.Add(Map.Get.Cages[cage.X + 1, cage.Y]);
            if (CanMove(cage.X, cage.Y - 1))
                cages.Add(Map.Get.Cages[cage.X, cage.Y - 1]);
            if (CanMove(cage.X - 1, cage.Y))
                cages.Add(Map.Get.Cages[cage.X - 1, cage.Y]);
        }
        return false;
    }
    #endregion

    #region Helpers
    private bool CanMove (Cage c) {
        if (c != null && !c.isCaveman && !Checked[c.X, c.Y]) {
            Checked[c.X, c.Y] = true;
            return true;
        } else {
            return false;
        }
    }

    private bool CanMove (int x, int y) {
        if (Map.Get.isCageExist(x, y))
            return CanMove(Map.Get.Cages[x, y]);
        else return false;
    }

    private bool IsWinnerCage (Cage c) {
        return c.X == 0 || c.Y == 0 || c.X == Map.Get.Columns - 1 || c.Y == Map.Get.Rows - 1;
    }
    #endregion

    #region WayShow

    [SerializeField] private GameObject wayPointerPrefab;
    [SerializeField] private GameObject wayPointersParent;

    private float newPointerSpawnLatency = .2f;
    private float pointerAnimationDurability = .5f;

    private IEnumerator ShowLoseWay(List<(int, int)> wayToShow) {
        List<GameObject> pointers = new List<GameObject>();
        StartCoroutine(RemoveLoseWay(pointers));
        for (int i = 0;i<wayToShow.Count;i++) {
            GameObject pointer = Instantiate(wayPointerPrefab, new Vector2(), Quaternion.identity, wayPointersParent.transform);
            pointer.transform.localPosition = new Vector2(Map.Get.StartX + wayToShow[i].Item1 * Map.Get.CagesWidth + Map.Get.CagesWidth/2, Map.Get.StartY - wayToShow[i].Item2 * Map.Get.CagesHeight - Map.Get.CagesHeight/2);
            pointers.Add(pointer);
            yield return new WaitForSeconds(newPointerSpawnLatency);
        }
    }

    private IEnumerator RemoveLoseWay(List<GameObject> pointers) {
        yield return new WaitForSeconds(pointerAnimationDurability);
        while (pointers.Count!=0) {
            GameObject lastPointer = pointers[0];
            pointers.Remove(lastPointer);
            Destroy(lastPointer);
            yield return new WaitForSeconds(newPointerSpawnLatency);
        }
    }

    private void FindWay(Cage finalCage) {
        Game.AudioManager.Play("Lose");
        List<Cage> cages = new List<Cage>();
        
        cages.Add(finalCage);
        Cage current = finalCage;
        Checked[current.X, current.Y] = false;

        while (!mammothNear(current)) {
            if (Map.Get.isCageExist(current.X, current.Y + 1) && Checked[current.X, current.Y + 1]) {
                cages.Add(Map.Get.Cages[current.X, current.Y + 1]);
            } else if (Map.Get.isCageExist(current.X + 1, current.Y) && Checked[current.X + 1, current.Y]) {
                cages.Add(Map.Get.Cages[current.X + 1, current.Y]);
            } else if (Map.Get.isCageExist(current.X, current.Y - 1) && Checked[current.X, current.Y - 1]) {
                cages.Add(Map.Get.Cages[current.X, current.Y - 1]);
            } else if (Map.Get.isCageExist(current.X - 1, current.Y) && Checked[current.X - 1, current.Y]) {
                cages.Add(Map.Get.Cages[current.X - 1, current.Y]);
            } else {
                cages.Remove(current);
            }
            if (cages.Count != 0) {
                current = cages[cages.Count - 1];
                Checked[current.X, current.Y] = false;
            }
        }

        List < (int, int) > way = new List<(int, int)>();

        way.Add((Map.Get.MammothX, Map.Get.MammothY));

        cages.Reverse();

        foreach (Cage c in cages) {
            way.Add((c.X, c.Y));
        }

        if (finalCage.X == Map.Get.Columns-1) {
            way.Add((finalCage.X + 1, finalCage.Y));
        } else if (finalCage.X == 0) {
            way.Add((finalCage.X - 1, finalCage.Y));
        } else if (finalCage.Y == Map.Get.Rows - 1) {
            way.Add((finalCage.X, finalCage.Y + 1));
        } else if (finalCage.Y == 0) {
            way.Add((finalCage.X, finalCage.Y - 1));
        }

        /*Debug.Log("(X, Y) [" + way.Count + "]:");

        foreach ((int, int) ints in way) {
            Debug.Log(ints.Item1);
            Debug.Log(ints.Item2);
        }*/

        StartCoroutine(ShowLoseWay(way));

    }

    private bool mammothNear(Cage c) {
        return ((Mathf.Abs(Map.Get.MammothX - c.X) < 2 && Map.Get.MammothY == c.Y) || (Mathf.Abs(Map.Get.MammothY - c.Y) < 2 && Map.Get.MammothX == c.X));
    }

    #endregion

}