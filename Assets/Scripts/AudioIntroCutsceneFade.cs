using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioIntroCutsceneFade : MonoBehaviour {

    public AudioSource audioSource; 
    public float fadeInDuration = 5f; 

    [SerializeField] private float targetVolume;
    private float fadeSpeed;

    void Start() {
        if (audioSource != null) {
            targetVolume = audioSource.volume;
            audioSource.volume = 0f;
            fadeSpeed = targetVolume / fadeInDuration;
            audioSource.Play(); 
        }
    }

    void Update() {
        if (audioSource != null && audioSource.volume < targetVolume) {
            audioSource.volume += fadeSpeed * Time.deltaTime;

            if (audioSource.volume > targetVolume) {
                audioSource.volume = targetVolume;
            }
        }
    }
}