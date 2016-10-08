using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform[] cameraTargets;
    int cameraIndex;

    public float cameraRotX = 0f;

    public float cameraRotMax = 45f;

    public float smoothing = 0f;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Tab)) {
            cameraIndex++;
            if (cameraIndex >= cameraTargets.Length) {
                cameraIndex = 0;
            }
        }

        if (cameraTargets[cameraIndex]) {
            if (smoothing == 0f)
            {
                transform.position = cameraTargets[cameraIndex].position;
                transform.rotation = cameraTargets[cameraIndex].rotation;
            }
            else {
                transform.position = Vector3.Lerp(transform.position, cameraTargets[cameraIndex].position, Time.deltaTime * smoothing);
                transform.rotation = cameraTargets[cameraIndex].rotation;
            }
        }

        cameraRotX -= Input.GetAxis("Mouse Y");

        cameraRotX = Mathf.Clamp(cameraRotX, -cameraRotMax, cameraRotMax);

        Camera.main.transform.Rotate(cameraRotX, 0f, 0f);
    }
}
