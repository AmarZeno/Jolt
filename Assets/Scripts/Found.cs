using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Found : MonoBehaviour {
    public Text text;
    public Game game;

    void Start()
    {
        text.text = "FOUND " + game.NumIdentified + "/" + game.NumTotal;
    }

    public void Show()
    {
        text.text = "FOUND " + game.NumIdentified + "/" + game.NumTotal;
    }
}
