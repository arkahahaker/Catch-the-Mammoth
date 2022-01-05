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
    #endregion

    #region Chekers

    public static void CheckWin() {
        if (!Singleton.tileZone.AllSet())
            return;
        LevelsManager.isActive = false;
        if (!Singleton.CanEscape()) {
            if (!LevelsManager.IsLevelCompleted())
                PlayerPrefs.SetInt("CompletedLevels", LevelsManager.CurrentLevel);
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
            if (CanMove(cage.x, cage.y + 1))
                cages.Add(Map.Get.Cages[cage.x, cage.y + 1]);
            if (CanMove(cage.x + 1, cage.y))
                cages.Add(Map.Get.Cages[cage.x + 1, cage.y]);
            if (CanMove(cage.x, cage.y - 1))
                cages.Add(Map.Get.Cages[cage.x, cage.y - 1]);
            if (CanMove(cage.x - 1, cage.y))
                cages.Add(Map.Get.Cages[cage.x - 1, cage.y]);
        }
        return false;
    }
    #endregion

    #region Helpers
    private bool CanMove (Cage c) {
        if (c != null && !c.isCaveman && !Checked[c.x, c.y]) {
            Checked[c.x, c.y] = true;
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
        return c.x == 0 || c.y == 0 || c.x == Map.Get.Columns - 1 || c.y == Map.Get.Rows - 1;
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

            int xMove, yMove;   // Discovering in which side does that trail looks
            if (i != wayToShow.Count - 1)
                xMove = wayToShow[i + 1].Item1 - wayToShow[i].Item1;
            else xMove = wayToShow[i].Item1 < 0 ? -1 : (wayToShow[i].Item1==Map.Get.Columns ? 1 : 0);
            if (i != wayToShow.Count - 1)
                yMove = wayToShow[i + 1].Item2 - wayToShow[i].Item2;
            else yMove = wayToShow[i].Item2 < 0 ? -1 : (wayToShow[i].Item2 == Map.Get.Rows ? 1 : 0);
            Quaternion rotation = Quaternion.Euler(0, 0, xMove == 1 ? -90 : (yMove == -1 ? 0 : (xMove == -1 ? 90 : 180))); // determine rotation of the trail

            GameObject pointer = Instantiate(wayPointerPrefab, new Vector2(), rotation, wayPointersParent.transform);
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
        AudioManager.Singleton.Play("Lose");
        List<Cage> cages = new List<Cage>();
        
        cages.Add(finalCage);
        Cage current = finalCage;
        Checked[current.x, current.y] = false;

        while (!mammothNear(current)) {
            if (Map.Get.isCageExist(current.x, current.y + 1) && Checked[current.x, current.y + 1]) {
                cages.Add(Map.Get.Cages[current.x, current.y + 1]);
            } else if (Map.Get.isCageExist(current.x + 1, current.y) && Checked[current.x + 1, current.y]) {
                cages.Add(Map.Get.Cages[current.x + 1, current.y]);
            } else if (Map.Get.isCageExist(current.x, current.y - 1) && Checked[current.x, current.y - 1]) {
                cages.Add(Map.Get.Cages[current.x, current.y - 1]);
            } else if (Map.Get.isCageExist(current.x - 1, current.y) && Checked[current.x - 1, current.y]) {
                cages.Add(Map.Get.Cages[current.x - 1, current.y]);
            } else {
                cages.Remove(current);
            }
            if (cages.Count != 0) {
                current = cages[cages.Count - 1];
                Checked[current.x, current.y] = false;
            }
        }

        List < (int, int) > way = new List<(int, int)>();

        way.Add((Map.Get.MammothX, Map.Get.MammothY));

        cages.Reverse();

        foreach (Cage c in cages) {
            way.Add((c.x, c.y));
        }

        if (finalCage.x == Map.Get.Columns-1) {
            way.Add((finalCage.x + 1, finalCage.y));
        } else if (finalCage.x == 0) {
            way.Add((finalCage.x - 1, finalCage.y));
        } else if (finalCage.y == Map.Get.Rows - 1) {
            way.Add((finalCage.x, finalCage.y + 1));
        } else if (finalCage.y == 0) {
            way.Add((finalCage.x, finalCage.y - 1));
        }

        /*Debug.Log("(X, Y) [" + way.Count + "]:");

        foreach ((int, int) ints in way) {
            Debug.Log(ints.Item1);
            Debug.Log(ints.Item2);
        }*/

        StartCoroutine(ShowLoseWay(way));

    }

    private bool mammothNear(Cage c) {
        return ((Mathf.Abs(Map.Get.MammothX - c.x) < 2 && Map.Get.MammothY == c.y) || (Mathf.Abs(Map.Get.MammothY - c.y) < 2 && Map.Get.MammothX == c.x));
    }

    #endregion

}