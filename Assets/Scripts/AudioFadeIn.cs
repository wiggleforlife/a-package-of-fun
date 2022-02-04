using UnityEngine;
using System.Collections;

public static class AudioFadeIn {

    public static IEnumerator FadeIn (AudioSource audioSource, float FadeTime, AudioClip newSong, float delay) {
        int num = 1;
        while(num == 1) {
            yield return new WaitForSeconds(delay);
            num = 0;
        }
        
        float startVolume = audioSource.volume;
        audioSource.clip = newSong;
        audioSource.Play();
        while (audioSource.volume < 100) {
            audioSource.volume += 0.005f;
            yield return null;
        }

    }

}