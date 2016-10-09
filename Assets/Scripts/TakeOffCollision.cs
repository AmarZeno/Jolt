using UnityEngine;
using System.Collections;

public class TakeOffCollision : MonoBehaviour {
    public AudioSource sfx;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            sfx.Play();
        }
    }
}
