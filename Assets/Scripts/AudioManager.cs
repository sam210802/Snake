using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    public static readonly AudioClip deathSound = Resources.Load<AudioClip>("Death");
    public static readonly AudioClip smallWinSound = Resources.Load<AudioClip>("SmallWin");
    public static readonly AudioClip bigWinSound = Resources.Load<AudioClip>("BigWin");
    public static readonly AudioClip nomSound = Resources.Load<AudioClip>("Nom");

    public static readonly AudioClip backgroundMusicWallpaper = Resources.Load<AudioClip>("Wallpaper");
    public static readonly AudioClip backgroundMusicSpaceJazz = Resources.Load<AudioClip>("Space-Jazz");

    static GameObject audioHolder;
    static AudioSource audioSourceSFX;
    static AudioSource audioSourceMusic;


    public static void PlaySound(AudioClip audio) {
        createHolder();
        ChangeSFXVolume();
        audioSourceSFX.PlayOneShot(audio);
    }   

    public static void PlayMusic(AudioClip audio) {
        createHolder();
        ChangeMusicVolume();
        audioSourceMusic.clip = audio;
        audioSourceMusic.Play();
    }

    public static void ChangeMusicVolume() {
        audioSourceMusic.volume = PlayerPrefs.GetFloat("Main_Volume", 1.0f) * PlayerPrefs.GetFloat("Music_Volume", 1.0f);
    }

    public static void ChangeSFXVolume() {
        audioSourceSFX.volume = PlayerPrefs.GetFloat("Main_Volume", 1.0f) * PlayerPrefs.GetFloat("SFX_Volume", 1.0f);
    }

    static void createHolder() {
        if (audioHolder == null) audioHolder = new GameObject("Audio Holder");
        if (audioSourceSFX == null) {
            audioSourceSFX = audioHolder.AddComponent<AudioSource>();
        }
        if (audioSourceMusic == null) {
            audioSourceMusic = audioHolder.AddComponent<AudioSource>();
            audioSourceMusic.loop = true;
        }
    }
}
