using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CustomSceneManager : MonoBehaviour
{

    public static CustomSceneManager Singleton;

    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else Destroy(gameObject);
    }

    public void LoadScene (int scene) {
        SceneManager.LoadScene(scene);
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public IEnumerator LoadSceneCurtains (int scene) {
        yield return StartCoroutine(LeavesCurtain.Singleton.ActCurtain());
        LoadScene(scene);
    }

    public IEnumerator LoadSceneCurtains(string scene) {
        yield return StartCoroutine(LeavesCurtain.Singleton.ActCurtain());
        LoadScene(scene);
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
        if (LeavesCurtain.Singleton != null && LeavesCurtain.Singleton.isCovered) StartCoroutine(LeavesCurtain.Singleton.ActCurtain());
    }

}