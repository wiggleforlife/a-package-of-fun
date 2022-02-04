using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [SerializeField] Animation mainCamAnim;
    [SerializeField] Animation factoryAnim;
    [SerializeField] Animation doorAnim;
    [SerializeField] Camera mainCam;
    [SerializeField] AudioClip nextSong;
    [SerializeField] Canvas canvas1;
    [SerializeField] Canvas canvas2;
    [SerializeField] Canvas canvas3;

    void Start() {
        canvas1.enabled = true;
        canvas2.enabled = false;
        canvas3.enabled = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }
    public void play() {

        AudioSource musicSource = mainCam.GetComponent<AudioSource>();
        StartCoroutine(AudioFadeOut.FadeOut (musicSource, 0.5f));
        StartCoroutine(AudioFadeIn.FadeIn (musicSource, 0.5f, nextSong, 0.7f));

        canvas1.enabled = false;

        mainCamAnim.Play();
        doorAnim.Play();

        StartCoroutine(waitForAnimation(1.2f));

    }

    public void sceneLoad(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }

    public void platformerLevels() {
        canvas2.enabled = false;
        canvas3.enabled = true;
    }

    public IEnumerator waitForAnimation(float seconds) {
        yield return new WaitForSeconds(seconds);
        canvas2.enabled = true;
    }
}
