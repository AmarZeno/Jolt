using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCollision : MonoBehaviour {

    public float Magnitude;

    public Game Game;

    public AudioSource DieSFX;

    public GameObject reset;

    public FadeInOutText die;

    Rigidbody rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 impulse = collision.impulse;
        float cosAngle = Mathf.Abs(Vector3.Dot(impulse.normalized, transform.forward));
        Debug.Log("impulse: " + impulse + " velocity: " + transform.forward + " cosAngle: " + cosAngle + " Magnitude forward: " + impulse.magnitude * (1 - cosAngle));
        if (impulse.magnitude * (1 - cosAngle) > Magnitude)
        {
            DieSFX.Play();
            //Game.Pause();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            reset.SetActive(true);
            die.Show();
        }
    }
}
