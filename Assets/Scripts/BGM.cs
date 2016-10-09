using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
