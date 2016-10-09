using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finish : MonoBehaviour {
    public Text text;
    public Game game;

    public float FadeInTime;
    public float FadeOutTime;
    public void Show()
    {
        text.text = "Level Complete\nYou Rescued " + game.NumIdentified + " / 4";
        StartCoroutine(FadeInOutText.FadeInOut(text, FadeInTime, FadeOutTime));
    }
}
