using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;
    private List<string> themeArray = new List<string>();
    private bool isFocused = true;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start() {
        PlayRandomSong();
    }

    private void Update() {
        if(!isFocused) return;
        Sound sound = Array.Find(sounds, sound => sound.source.isPlaying);
        if (sound == null) {
            PlayRandomSong();
        }
    }

    public void PlayRandomSong() {
        if(themeArray.Count == 0) {
            for (int i = 0; i < 5; i++){  
                themeArray.Add("theme"+(i+1));
            }
        }
        int songIndex = UnityEngine.Random.Range(0, themeArray.Count);
        Play(themeArray[songIndex]);
        themeArray.RemoveAt(songIndex);
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if(sound == null) {
            Debug.LogWarning("Did not found sound file: " + name);
            return;
        }
        sound.source.Play();
    }

    public void Mute(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if(sound == null) {
            Debug.LogWarning("Did not found sound file: " + name);
            return;
        }
        sound.source.volume = 0f;
    }

    public void Unmute(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if(sound == null) {
            Debug.LogWarning("Did not found sound file: " + name);
            return;
        }
        sound.source.volume = .3f;
    }

    public static void ToggleAllSFX(bool disable) {
        if(disable){
            instance.Mute(SoundConst.BUTTON_CLICK);
            instance.Mute(SoundConst.BUTTON_CLOSE);
        } else {
            instance.Unmute(SoundConst.BUTTON_CLICK);
            instance.Unmute(SoundConst.BUTTON_CLOSE);
        }
    }

    public static void ToggleMusic(bool disable) {
        if(disable){
            instance.Mute(SoundConst.THEME1);
            instance.Mute(SoundConst.THEME2);
            instance.Mute(SoundConst.THEME3);
            instance.Mute(SoundConst.THEME4);
            instance.Mute(SoundConst.THEME5);
        } else {
            instance.Unmute(SoundConst.THEME1);
            instance.Unmute(SoundConst.THEME2);
            instance.Unmute(SoundConst.THEME3);
            instance.Unmute(SoundConst.THEME4);
            instance.Unmute(SoundConst.THEME5);
        }
    }

    private void OnApplicationFocus(bool focusStatus) {
        isFocused = focusStatus;
    }
}
