using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    private int numIdentified;
    public int NumIdentified
    {
        get
        {
            return numIdentified;
        }
    }
    public int NumTotal;
    public void AddOneIdentified()
    {
        numIdentified++;
    }
    public void Pause()
    {
        Time.timeScale = 0.0f;
    }
    public void Reset()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("main");
    }
}
