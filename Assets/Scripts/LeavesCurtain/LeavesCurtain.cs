using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LeavesCurtain : MonoBehaviour
{

    public UnityEvent OnCurtainsClosed;
    public UnityEvent OnCurtainsOpened;

    public List<Leaf> Leaves;

    public bool isCovered { get; set; } // is leaves locked the screen
    public bool isAnimating { get; set; } // is animation working now

    private int sceneToLoad;

    public static LeavesCurtain Singleton;

    private void Awake () {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        } else Destroy(transform.parent.gameObject);
    }

    public IEnumerator ActCurtain () {
        if (!isAnimating) {
            // Check and do something only when leaves aren't currently moving
            isAnimating = true;
            if (isCovered) {    
                // if curtain covering, push leaves outside the screen
                for (int i = 1;i<Leaves.Count;i++) 
                    StartCoroutine(Leaves[i].ToFinalPosition());
                yield return StartCoroutine(Leaves[0].ToFinalPosition());
                foreach (Leaf leaf in Leaves) leaf.ToStartPosition();
                OnCurtainsOpened.Invoke();
            } else {            
                // if curtains outside the screen, push them inside  
                for (int i = 1; i < Leaves.Count; i++)
                    StartCoroutine(Leaves[i].ToInterPosition());
                yield return StartCoroutine(Leaves[0].ToInterPosition());
                OnCurtainsClosed?.Invoke();
            }
            isCovered = !isCovered;
            isAnimating = false;
        }
    }

}