using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StartButton : MonoBehaviour {

    public void Clicked(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void Exit() {
        Application.Quit();
    }

    public void OnMouseEnter() {
        transform.localScale = new Vector3(1.2F, 1.2F, 1.2F);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);
    }

    public void OnMouseExit() {
        transform.localScale = new Vector3(1F, 1F, 1F);
    }
}
