using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapScroller : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

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
        }
    }

    public void OnEndDrag (PointerEventData eventData) {
        MapInScreen();
    }

    private void MapInScreen () {
        if (t.localPosition.x > -Screen.currentResolution.width/2) {
            t.localPosition = new Vector3(-Screen.currentResolution.width / 2, t.localPosition.y, t.localPosition.z);
        }
        if (t.localPosition.y < Screen.currentResolution.height/2) {
            t.localPosition = new Vector3(t.localPosition.x, Screen.currentResolution.height/2, t.localPosition.z);
        }
        if (t.localPosition.x < -t.rect.width + Screen.currentResolution.width / 2) {
            t.localPosition = new Vector3(-t.rect.width + Screen.currentResolution.width / 2, t.localPosition.y, t.localPosition.z);
        }
        if (t.localPosition.y > t.rect.height - Screen.currentResolution.height / 2) {
            t.localPosition = new Vector3(t.localPosition.x, t.rect.height - Screen.currentResolution.height / 2, t.localPosition.z);
        }
   }

}