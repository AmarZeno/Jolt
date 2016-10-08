using UnityEngine;
using System.Collections;

public class UnitPlayer : Unit {

    

	// Use this for initialization
	public override void Start () {
        base.Start();
	}

    // Update is called once per frame
    public override void Update () {

        // Rotation

        transform.Rotate(0f, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0f);

        move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        move.Normalize();

        move = transform.TransformDirection(move);

        if (Input.GetKeyDown(KeyCode.Space)) {
            jump = true;
        }

        running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        base.Update();
	}
}
