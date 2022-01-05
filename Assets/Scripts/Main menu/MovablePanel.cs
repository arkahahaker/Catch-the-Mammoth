using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePanel : MonoBehaviour {

    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 finalPosition;

    private bool IsShown => transform.localPosition.Equals(finalPosition);
    private bool isActive = true;

    void Start() {
        transform.localPosition = startPosition;
    }
    
    public void SetState() {
        if (isActive)
            StartCoroutine(MovePanel());
    }

    private IEnumerator MovePanel() {
        isActive = false;
        var to = IsShown ? startPosition : finalPosition;
        const float time = 0.2f;
        const int frames = 60;
        const float tick = time / frames;
        var movePerFrame = ((Vector3)to-transform.localPosition) / frames;
        for (var i = 0; i < frames; i++) {
            transform.localPosition += movePerFrame;
            yield return new WaitForSeconds(tick);
        }
        transform.localPosition = to;
        isActive = true;
    }

}