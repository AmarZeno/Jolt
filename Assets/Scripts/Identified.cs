using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Identified : MonoBehaviour {
    public Text text;
    public float FadeInTime;
    public float FadeOutTime;
    public void Show()
    {
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        text.enabled = true;
        float time = 0.0f;
        while(time < FadeInTime)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, (time / FadeInTime));            
            time += Time.deltaTime;
            yield return null;
        }
        time = 0.0f;
        while (time < FadeOutTime)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1-(time / FadeOutTime));
            time += Time.deltaTime;
            yield return null;
        }
        text.enabled = false;
        yield return null;
    }
}
