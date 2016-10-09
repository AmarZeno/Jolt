using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Found : MonoBehaviour {
    public Text text;
    public float FadeInTime;
    public float FadeOutTime;

    public float TotalNum;

    int foundNum;

    void Start()
    {
        foundNum = 0;
    }

    public void Show()
    {
        foundNum++;
        text.text = "FOUND " + foundNum + "/" + TotalNum;
    }
}
