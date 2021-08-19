using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DragableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

    #region Params

    public GameObject Caveman;
    public List<GameObject> RaycastPoints;
    public List<Cage> OccupiedCages;

    public bool isSet { get; set; }
    protected bool isDraging { get; set; }
    private bool isTip { get; set; }

    private RectTransform t { get; set; }
    private RectTransform cavt { get; set; }

    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector3 startRotation;

    #endregion

    #region Start Actions
    protected void Start() {


        GameObject pointsParent = transform.GetChild(0).gameObject;

        RaycastPoints = new List<GameObject>();

        for (int i = 0; i < pointsParent.transform.childCount; i++) {
            GameObject point = pointsParent.transform.GetChild(i).gameObject;
            if (point.GetComponent<Caveman>() != null)
                Caveman = point;
            else
                RaycastPoints.Add(point);
        }


        t = GetComponent<RectTransform>();
        cavt = Caveman.GetComponent<RectTransform>();

        OccupiedCages = new List<Cage>();

        if (startPosition == Vector2.zero)
            startPosition = t.localPosition;
        else
            t.localPosition = startPosition;

        if (startRotation != Vector3.zero)
           t.localEulerAngles = startRotation;

        cavt.localEulerAngles = -t.localEulerAngles;

        if (isExtended)
            ResizeInMoment();

        isSet = false;
        isDraging = false;
        isTip = false;

        resizeSizePerFrame = new Vector2(t.sizeDelta.x * (sizeMultiplicate - 1), t.sizeDelta.y * (sizeMultiplicate - 1)) / resizeFramesCount;
        resizeCavemanSizePerFrame = new Vector2(cavt.sizeDelta.x * (sizeMultiplicate - 1), cavt.sizeDelta.y * (sizeMultiplicate - 1)) / resizeFramesCount;
        resizeCavemanLocationPerFrame = cavt.localPosition * (sizeMultiplicate - 1) / resizeFramesCount;
    }
    #endregion

    #region Helpers

    public void TranslateThePointToPoint (GameObject rayPoint, Cage c) {
        if (c == null) return;
        t.localPosition = c.GetComponent<RectTransform>().localPosition - (GetRealPointPosition(rayPoint) - t.localPosition);
    }

    public Vector3 GetRealPointPosition (GameObject point) {
        var a = t.localPosition
            + Quaternion.AngleAxis(t.localEulerAngles.z, Vector3.forward) * point.GetComponent<RectTransform>().localPosition
        ;
        return a;
    }

    static public T FindInParents<T>(GameObject go) where T : Component {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null) {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    #endregion

    #region OnClick
    public virtual void OnPointerClick(PointerEventData eventData) {
        if (isDraging)
            return;
        if (isSet) {
            RemoveFromMap();
            StartCoroutine(ReturnToStartPosition());
        } else {
            StartCoroutine(Turn());
        }
    }
    #endregion

    #region Drag&Drop
    private Vector2 dragDistanceFromMiddle;

    public void OnBeginDrag(PointerEventData eventData) {
        
        if (!Game.isActive)
            return;

        isDraging = true;
        StopCoroutine(ReturnToStartPosition());
        if (!isExtended)
            StartCoroutine(Resize());
        if (isSet) {
            RemoveFromMap();
        }
        dragDistanceFromMiddle = (Vector2)t.position - eventData.position;
        Transform parent = transform.parent;
        transform.SetParent(transform.GetComponentInParent<Canvas>().transform);
        transform.SetParent(parent);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data) {
        if (!Game.isActive)
            return;
        if (isDraging)
            SetDraggedPosition(data);
    }

    protected virtual void SetDraggedPosition(PointerEventData data) {
        t.position = data.position + dragDistanceFromMiddle;
    }

    public virtual void OnEndDrag(PointerEventData eventData) {
        if (isDraging) {
            Invoke("EndDraging", .1f);
            if (Map.Get.CanSetTile(this)) {
                Map.Get.SetTile(this);
            } else {
                StartCoroutine("ReturnToStartPosition");
                if (isExtended)
                    StartCoroutine(Resize());
            }
        }
    }

    private void EndDraging () {
        isDraging = false;
    }

    #endregion

    #region Translating and Removing
    public void RemoveFromMap() {
        foreach (Cage cage in OccupiedCages) {
            cage.isFree = true;
            cage.isCaveman = false;
        }
        OccupiedCages.Clear();
        isSet = false;
    }

    protected IEnumerator ReturnToStartPosition() {
        if (isExtended)
            StartCoroutine(Resize());
        yield return TranslateTo(startPosition, 30, .01f);
    }

    private IEnumerator TranslateTo (Vector2 position, int framesCount, float framesTime) {
        Game.isActive = false;
        Vector2 distancePerFrame = (position - (Vector2)transform.localPosition) / framesCount;
        for (int i = 0; i < framesCount; i++) {
            transform.localPosition = (Vector2)transform.localPosition + distancePerFrame;
            yield return new WaitForSeconds(framesTime);
        }
        transform.localPosition = position;
        Game.isActive = true;
        yield return null;
    }
    #endregion

    #region Turn
    public IEnumerator Turn() {
        int frames = 10;
        float framesTime = .01f;
        float anglePerFrame = 90 / frames;
        for (int i = 0; i < frames; i++) {
            t.Rotate(0, 0, anglePerFrame);
            cavt.Rotate(0, 0, -anglePerFrame);
            yield return new WaitForSeconds(framesTime);
        }
    }

    public IEnumerator TurnTo(float zRotation) {
        int frames = 20;
        float framesTime = .01f;
        float anglePerFrame = (zRotation-t.localEulerAngles.z) / frames;
        for (int i = 0; i < frames; i++) {
            t.Rotate(0, 0, anglePerFrame);
            cavt.Rotate(0, 0, -anglePerFrame);
            yield return new WaitForSeconds(framesTime);
        }
    }
    #endregion

    #region Resize
    public bool isExtended = false;

    [SerializeField] private float sizeMultiplicate;
    private int resizeFramesCount = 10;
    private float resizeFramesTime = .01f;
    private Vector2 resizeSizePerFrame;
    private Vector2 resizeCavemanSizePerFrame;
    private Vector2 resizeCavemanLocationPerFrame;

    public IEnumerator Resize() {
        isExtended = !isExtended;        
        if (isExtended) {
            for (int i = 0; i < resizeFramesCount; i++) {
                t.sizeDelta = t.sizeDelta + resizeSizePerFrame;
                cavt.sizeDelta = cavt.sizeDelta + resizeCavemanSizePerFrame;
                cavt.localPosition += (Vector3)resizeCavemanLocationPerFrame;
                yield return new WaitForSeconds(resizeFramesTime);
            }
        } else {
            for (int i = 0; i < resizeFramesCount; i++) {
                t.sizeDelta = t.sizeDelta - resizeSizePerFrame;
                cavt.sizeDelta = cavt.sizeDelta - resizeCavemanSizePerFrame;
                cavt.localPosition -= (Vector3)resizeCavemanLocationPerFrame;
                yield return new WaitForSeconds(resizeFramesTime);
            }
        }
    }
    #endregion

    #region Tips
    [SerializeField] Vector2 truePosition;
    [SerializeField] float trueRotation;

    public IEnumerator SetTileByTip () {
        Game.isActive = false;
        isTip = true;

        yield return Resize();
        yield return TurnTo(trueRotation);
        yield return TranslateTo(truePosition, 30, .01f);

        Map.Get.SetTile(this);

        Game.isActive = true;
    }
    #endregion

    #region Inspector Actions
    public void TurnInMoment() {

        GameObject pointsParent = transform.GetChild(0).gameObject;

        RaycastPoints = new List<GameObject>();

        for (int i = 0; i < pointsParent.transform.childCount; i++) {
            GameObject point = pointsParent.transform.GetChild(i).gameObject;
            if (point.GetComponent<Caveman>() != null)
                Caveman = point;
            else
                RaycastPoints.Add(point);
        }

        GetComponent<RectTransform>().Rotate(0, 0, 90);
        Caveman.GetComponent<RectTransform>().Rotate(0, 0, -90);
        if (GetComponent<RectTransform>().rotation.z == -180)
            GetComponent<RectTransform>().Rotate(0, 0, 360);
    }

    public void ResizeInMoment() {

        GameObject pointsParent = transform.GetChild(0).gameObject;

        RaycastPoints = new List<GameObject>();

        for (int i = 0; i < pointsParent.transform.childCount; i++) {
            GameObject point = pointsParent.transform.GetChild(i).gameObject;
            if (point.GetComponent<Caveman>() != null)
                Caveman = point;
            else
                RaycastPoints.Add(point);
        }

        resizeSizePerFrame = new Vector2(GetComponent<RectTransform>().sizeDelta.x * (sizeMultiplicate - 1), GetComponent<RectTransform>().sizeDelta.y * (sizeMultiplicate - 1)) / resizeFramesCount;
        resizeCavemanSizePerFrame = new Vector2(Caveman.GetComponent<RectTransform>().sizeDelta.x * (sizeMultiplicate - 1), Caveman.GetComponent<RectTransform>().sizeDelta.y * (sizeMultiplicate - 1)) / resizeFramesCount;
        resizeCavemanLocationPerFrame = Caveman.GetComponent<RectTransform>().localPosition * (sizeMultiplicate - 1) / resizeFramesCount;

        isExtended = !isExtended;
        if (isExtended) {
            GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta + resizeSizePerFrame*resizeFramesCount;
            Caveman.GetComponent<RectTransform>().sizeDelta = Caveman.GetComponent<RectTransform>().sizeDelta + resizeCavemanSizePerFrame * resizeFramesCount;
            Caveman.GetComponent<RectTransform>().localPosition += (Vector3)resizeCavemanLocationPerFrame * resizeFramesCount;
        } else {
            GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta - resizeSizePerFrame * resizeFramesCount / sizeMultiplicate;
            Caveman.GetComponent<RectTransform>().sizeDelta = Caveman.GetComponent<RectTransform>().sizeDelta - resizeCavemanSizePerFrame * resizeFramesCount / sizeMultiplicate;
            Caveman.GetComponent<RectTransform>().localPosition -= (Vector3)resizeCavemanLocationPerFrame * resizeFramesCount / sizeMultiplicate ;
        }
    }

    public void SavePositionForTip() {
        truePosition = GetComponent<RectTransform>().localPosition;
        trueRotation = GetComponent<RectTransform>().localEulerAngles.z;
    }

    public void SavePositionForHome() {
        startPosition = GetComponent<RectTransform>().localPosition;
        startRotation = GetComponent<RectTransform>().localEulerAngles;
    }

    public void LoadPositionForTip() {

        GameObject pointsParent = transform.GetChild(0).gameObject;

        RaycastPoints = new List<GameObject>();

        for (int i = 0; i < pointsParent.transform.childCount; i++) {
            GameObject point = pointsParent.transform.GetChild(i).gameObject;
            if (point.GetComponent<Caveman>() != null)
                Caveman = point;
            else
                RaycastPoints.Add(point);
        }

        if (!isExtended)
            ResizeInMoment();
        GetComponent<RectTransform>().localPosition = truePosition;
        GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, trueRotation);
        Caveman.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -trueRotation);
    }

    public void LoadPositionForHome() {

        GameObject pointsParent = transform.GetChild(0).gameObject;

        RaycastPoints = new List<GameObject>();

        for (int i = 0; i < pointsParent.transform.childCount; i++) {
            GameObject point = pointsParent.transform.GetChild(i).gameObject;
            if (point.GetComponent<Caveman>() != null)
                Caveman = point;
            else
                RaycastPoints.Add(point);
        }

        if (isExtended)
            ResizeInMoment();
        GetComponent<RectTransform>().localPosition = startPosition;
        GetComponent<RectTransform>().localEulerAngles = startRotation;
        Caveman.GetComponent<RectTransform>().localEulerAngles = -startRotation;
    }
    #endregion

}