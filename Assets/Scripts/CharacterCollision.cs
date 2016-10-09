using UnityEngine;
using System.Collections;

public class CharacterCollision : MonoBehaviour {
    public AudioSource sfx;
    public Identified identify;
    public Found found;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            sfx.Play();
            identify.Show();
            found.Show();
        }
    }
}
