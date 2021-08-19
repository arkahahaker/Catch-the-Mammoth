using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private List<Sound> Sounds;

    private void Awake () {
        if (Game.AudioManager == null) Game.AudioManager = this;
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
        current.source.Play();
    }

    public void Loop(string name) {
        Sound current = Sounds.Find(sound => sound.name == name);
        current.source.loop = true;
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