using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapScroller : MonoBehaviour, IBeginDragHandler, IDragHandler {

    private Vector2 dragDistanceFromMiddle;
    private Vector2 startPosition;
    private RectTransform t;

    [SerializeField] Image FadingPanel;

    private bool zooming = false;

    private void Start() {
        t = GetComponent<RectTransform>();
        MapInScreen();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!zooming) {
            startPosition = t.localPosition;
            dragDistanceFromMiddle = (Vector2)transform.GetComponent<RectTransform>().position - eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (!zooming) {
            GetComponent<RectTransform>().position = eventData.position + dragDistanceFromMiddle;
            MapInScreen();
        }
    }

    private void MapInScreen () {
        if (t.position.x > 0) {
            t.localPosition += new Vector3(-t.position.x, 0, 0);
        }
        if (t.position.y < Screen.height) {
            t.localPosition += new Vector3(0, Screen.height - t.position.y, 0);
        }
        if (t.position.x < -t.rect.width + Screen.width) {
            t.localPosition += new Vector3(-t.rect.width + Screen.width - t.position.x, 0, 0);
        }
        if (t.position.y > t.rect.height) {
            t.localPosition += new Vector3(0, t.rect.height - t.position.y, 0);
        }
    }

    public IEnumerator StartFading() {

        int framesCount = 100;

        float time = 2f;

        float framesTime = time / framesCount;

        FadingPanel.gameObject.SetActive(true);

        for (int i = 0; i <= framesCount; i++) {
            FadingPanel.color = new Color(FadingPanel.color.r, FadingPanel.color.g, FadingPanel.color.b, 1 - i / (float)framesCount);
            yield return new WaitForSeconds(framesTime);
        }

        FadingPanel.gameObject.SetActive(false);

    }

    public IEnumerator ZoomLevel (int level) {

        Game.AudioManager.Play("Zoom");
        Game.AudioManager.Stop("MainMenu");

        FadingPanel.color = new Color(FadingPanel.color.r, FadingPanel.color.g, FadingPanel.color.b, 0);
        FadingPanel.gameObject.SetActive(false);
        //Vector3 pointToTravel = Camera.main.WorldToScreenPoint(objectRectTransform.position);
        zooming = true;

        const int translateFramesCount = 50;

        const float translateTime = .5f;

        const int zoomFramesCount = 100;

        const float zoomTime = 1f;

        // Map centralizing
        Vector2 startPosition = transform.GetChild(level - 1).position;

        Vector2 finalDestination = new Vector2(Screen.width / 2f, Screen.height / 2f);
        
        Vector2 distByFrame = (finalDestination - startPosition) / translateFramesCount;

        // Zooming
        CanvasScaler canv = GameObject.Find("Canvas").GetComponent<CanvasScaler>();

        Vector2 startRes = canv.referenceResolution;

        Vector2 endRes = new Vector2(canv.referenceResolution.x, 200);

        Vector2 resByFrame = (endRes - startRes) / zoomFramesCount;

        for (int i = 0; i < translateFramesCount; i++) {
            transform.Translate(distByFrame);
            yield return new WaitForSeconds(translateTime/ translateFramesCount);
        }

        FadingPanel.gameObject.SetActive(true);

        for (int i = 0; i <= zoomFramesCount; i++) {
            FadingPanel.color = new Color(FadingPanel.color.r, FadingPanel.color.g, FadingPanel.color.b, i/(float)zoomFramesCount);
            canv.referenceResolution = canv.referenceResolution + resByFrame;
            yield return new WaitForSeconds(zoomTime / zoomFramesCount);
        }

        SceneManager.LoadScene("Level" + Game.CurrentLevel);

    }

}