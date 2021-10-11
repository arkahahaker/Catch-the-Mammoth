using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioButton : MonoBehaviour {

    private Image image;

    [SerializeField] private Sprite[] sprites;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Start() {
        ChangeSprite();
    }
    public void TurnAudio() {
        AudioManager.Singleton.IsAudioOn = !AudioManager.Singleton.IsAudioOn;
        ChangeSprite();
    }

    private Sprite GetButtonSprite() {
        return sprites[!AudioManager.Singleton.IsAudioOn ? 0 : 1];
    }

    private void ChangeSprite() {
        image.sprite = GetButtonSprite();
    }
}