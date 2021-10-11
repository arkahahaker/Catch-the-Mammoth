using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{

    public Vector2 startPosition;
    public Quaternion startRotation;

    public Vector2 intermediatePosition;
    public Quaternion intermediateRotation;

    public Vector2 finalPosition;
    public Quaternion finalRotation;

    public void Awake () {
        
    }

    public void ToStartPosition() {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    public IEnumerator ToInterPosition () {
        yield return StartCoroutine(toPositionCoroutine(intermediatePosition, intermediateRotation));
    }

    public IEnumerator ToFinalPosition() {
        yield return StartCoroutine(toPositionCoroutine(finalPosition, finalRotation));
    }

    private IEnumerator toPositionCoroutine (Vector2 toPosition, Quaternion toRotation) {
        float time = 0.5f;
        int frames = 60;
        float timePerFrame = time / frames;
        Vector3 posPerFrame = (toPosition - (Vector2)transform.localPosition) / frames;
        Vector3 rotPerFrame = (toRotation.eulerAngles - transform.localRotation.eulerAngles) / frames;
        for (int i = 0;i<frames;i++) {
            transform.localPosition += posPerFrame;
            transform.Rotate(rotPerFrame);
            yield return new WaitForSeconds(timePerFrame);
        }
    }
}