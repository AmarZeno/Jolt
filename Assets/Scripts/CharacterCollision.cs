using UnityEngine;
using System.Collections;

public class CharacterCollision : MonoBehaviour {
    public AudioSource sfx;
    public FadeInOutText identify;
    public Found found;
    public Game game;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            game.AddOneIdentified();
            sfx.Play();
            identify.Show();
            found.Show();
        }
    }
}
