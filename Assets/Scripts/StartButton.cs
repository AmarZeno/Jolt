﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartButton : MonoBehaviour {

    public void Clicked(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
