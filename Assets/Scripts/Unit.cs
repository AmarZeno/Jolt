using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class Unit : MonoBehaviour {

    public CharacterController control;
    protected Vector3 move = Vector3.zero;

    public float walkSpeed = 1f;
    public float runSpeed = 5f;
    public float moveSpeed = 3f;
    public float turnSpeed = 90f;
    public float jumpSpeed = 6f;

    protected bool jump;
    protected bool running;

    protected Vector3 gravity = Vector3.zero;

	// Use this for initialization
	public virtual void Start () {
        if (!control) {
            Debug.LogError("Unit.Start()" + name + "has no Character Controller");
            enabled = false;
        }
	}

    // Update is called once per frame
    public virtual void Update () {
        //  control.SimpleMove(move * moveSpeed);

        if (running)
            move *= runSpeed;
        else
            move *= walkSpeed;

        move *= moveSpeed;

        if (!control.isGrounded)
        {
            // Adding gravity after climbing some objects to fall down
            gravity += Physics.gravity * Time.deltaTime;
        }
        else {
            gravity = Vector3.zero;
            if (jump)
            {
                gravity.y = jumpSpeed;
                jump = false;
            }
        }

        


        move += gravity;

        control.Move(move * Time.deltaTime);
    }
}
