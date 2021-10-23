using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour {

    private bool isAudioOn;
    public bool IsAudioOn { 
        get {
            return isAudioOn;
        } 
        set {
            isAudioOn = value;
            foreach (Sound sound in Sounds.Where(sound => sound.source.isPlaying)) {
                sound.source.mute = !isAudioOn;
                print(sound.name);
            }
            
        }
    }

    [SerializeField] private List<Sound> Sounds;
    public static AudioManager Singleton;

    private void Awake () {
        if (PlayerPrefs.HasKey("audio")) {
            PlayerPrefs.SetInt("audio", 1);
        }
        isAudioOn = PlayerPrefs.GetInt("audio") == 1;
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in Sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play (string name) {
        Sound current = Sounds.Find(sound => sound.name == name);
        current.source.loop = false;
        current.source.mute = !IsAudioOn;
        current.source.Play();
    }

    public void Loop(string name) {
        Sound current = Sounds.Find(sound => sound.name == name);
        current.source.loop = true;
        current.source.mute = !IsAudioOn;
        current.source.Play();
    }

    public void Stop(string name) {
        Sound current = Sounds.Find(sound => sound.name == name);
        current.source.Stop();
    }

    public bool IsPlaying(string name) {
        Sound current = Sounds.Find(sound => sound.name == name);
        return current.source.isPlaying;
    }

    public AudioSource GetSource(string name) {
        Sound current = Sounds.Find(sound => sound.name == name);
        return current.source;
    }

}