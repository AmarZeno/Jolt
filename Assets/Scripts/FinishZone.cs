using UnityEngine;
using System.Collections;

public class FinishZone : MonoBehaviour {
    public Finish finish;
    public GameObject button;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            finish.Show();
            button.SetActive(true);
        }
    }
}
