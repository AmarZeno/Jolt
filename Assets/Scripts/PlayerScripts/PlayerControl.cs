#define IS_DEVELOPMENT

using UnityEngine;
using System.Collections;
#if IS_DEVELOPMENT
#else
using Leap;
#endif
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{

	public float walkSpeed = 0.15f;
	public float runSpeed = 1.0f;
	public float sprintSpeed = 2.0f;
	public float flySpeed = 4.0f;

	public float turnSmoothing = 3.0f;
	public float aimTurnSmoothing = 15.0f;
	public float speedDampTime = 0.1f;

	public float jumpHeight = 5.0f;
	public float jumpCooldown = 1.0f;

	private float timeToNextJump = 0;
	
	private float speed;

	private Vector3 lastDirection;

	private Animator anim;
	private int speedFloat;
	private int jumpBool;
	private int hFloat;
	private int vFloat;
	private int aimBool;
	private int flyBool;
	private int groundedBool;
	private Transform cameraTransform;

	private float h;
	private float v;

	private bool aim;

	private bool run;
	private bool sprint;
    private bool forwardFly;

	private bool isMoving;

	// fly
	private bool fly = true;
	private float distToGround;
	private float sprintFactor;
    #if IS_DEVELOPMENT
#else
    Controller leapController;
    Quaternion leapRotation;
    Vector leapDirection;

    public Vector LeapDirection2
    {
        get
        {
            return leapDirection;
        }
    }
#endif

    void Awake()
	{
        #if IS_DEVELOPMENT
#else
        leapController = new Controller();
#endif
        anim = GetComponent<Animator> ();
		cameraTransform = Camera.main.transform;

		speedFloat = Animator.StringToHash("Speed");
		jumpBool = Animator.StringToHash("Jump");
		hFloat = Animator.StringToHash("H");
		vFloat = Animator.StringToHash("V");
		aimBool = Animator.StringToHash("Aim");
		// fly
		flyBool = Animator.StringToHash ("Fly");
		groundedBool = Animator.StringToHash("Grounded");
		distToGround = GetComponent<Collider>().bounds.extents.y;
		sprintFactor = sprintSpeed / runSpeed;
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void Update()
	{
		// fly
		//if(Input.GetButtonDown ("Fly"))
			//fly = !fly;
		aim = Input.GetButton("Aim");
		h = Input.GetAxis("Horizontal");
		//v = Input.GetAxis("Vertical");
        // Hard coding v value to be 1 for constant forward motion
        v = 1;
        run = Input.GetButton ("Run");
		sprint = Input.GetButton ("Sprint");
        forwardFly = Input.GetButton("ForwardFly");
		isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;

        #if IS_DEVELOPMENT
#else
        // Leap Motion
        Hand mainHand; // The front most hand captured by the Leap Motion Controller

        // Check if the Leap Motion Controller is ready
        if (!IsReady || Hands == null)
        {
            
            return;
        }

       mainHand = Hands[0];

        leapDirection = mainHand.Direction;
      //  Debug.Log(mainHand.Direction);

        leapRotation = Quaternion.Euler(mainHand.Direction.Pitch, mainHand.Direction.Yaw, mainHand.PalmNormal.Roll);
        
        // Debug.Log(leapRotation);
        // For relative orientation
        leapRotation *= Quaternion.Euler( mainHand.Direction.Pitch, mainHand.Direction.Yaw, mainHand.PalmNormal.Roll );
#endif
    }

    void FixedUpdate()
	{
        v = 1;
		anim.SetBool (aimBool, IsAiming());
		anim.SetFloat(hFloat, h);
		anim.SetFloat(vFloat, v);
		
		// Fly
		anim.SetBool (flyBool, fly);
		GetComponent<Rigidbody>().useGravity = !fly;
		anim.SetBool (groundedBool, IsGrounded ());

        #if IS_DEVELOPMENT
#else
        if (IsReady && Hands != null)
        {
            Debug.Log("here"+leapDirection);
           // h = 1;
            h = Mathf.Clamp(leapDirection.x, -1, 1);
        }
#endif


            if (fly)
			FlyManagement(h,v);

		else
		{
			MovementManagement (h, v, run, sprint);
			JumpManagement ();
		}
	}

	// fly
	void FlyManagement(float horizontal, float vertical)
	{
		Vector3 direction = Rotating(horizontal, vertical);
		GetComponent<Rigidbody>().AddForce(direction * flySpeed * 100 * (sprint?sprintFactor:1));
    }

	void JumpManagement()
	{
		if (GetComponent<Rigidbody>().velocity.y < 10) // already jumped
		{
			anim.SetBool (jumpBool, false);
			if(timeToNextJump > 0)
				timeToNextJump -= Time.deltaTime;
		}
		if (Input.GetButtonDown ("Jump"))
		{
			anim.SetBool(jumpBool, true);
			if(speed > 0 && timeToNextJump <= 0 && !aim)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
				timeToNextJump = jumpCooldown;
			}
		}
	}

	void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
	{
		Rotating(horizontal, vertical);

		if(isMoving)
		{
			if(sprinting)
			{
				speed = sprintSpeed;
			}
			else if (running)
			{
				speed = runSpeed;
			}
			else
			{
				speed = walkSpeed;
			}

			anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
		}
		else
		{
			speed = 0f;
			anim.SetFloat(speedFloat, 0f);
		}
		GetComponent<Rigidbody>().AddForce(Vector3.forward*speed);
	}

	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		if (!fly)
			forward.y = 0.0f;

		forward = forward.normalized;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 targetDirection;

		float finalTurnSmoothing;

		if(IsAiming())
		{
			targetDirection = forward;
			finalTurnSmoothing = aimTurnSmoothing;
		}
		else
		{
			targetDirection = forward * vertical + right * horizontal;
			finalTurnSmoothing = turnSmoothing;
		}

		if((isMoving && targetDirection != Vector3.zero) || IsAiming())
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
			// fly
			if (fly)
				targetRotation *= Quaternion.Euler (90, 0, 0);

			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
			lastDirection = targetDirection;
		}
		//idle - fly or grounded
		if(!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
		{
			Repositioning();
		}

		return targetDirection;
	}	

	private void Repositioning()
	{
		Vector3 repositioning = lastDirection;
		if(repositioning != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation (repositioning, Vector3.up);
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
		}
	}

	public bool IsFlying()
	{
		return fly;
	}

	public bool IsAiming()
	{
		return aim && !fly;
	}

	public bool isSprinting()
	{
		return sprint && !aim && (isMoving);
	}

    #if IS_DEVELOPMENT
#else
    // Leap Motion
    /// <summary>
    /// The current frame captured by the Leap Motion.
    /// </summary>
    Frame CurrentFrame
    {
        get { return (IsReady) ? leapController.Frame() : null; }
    }

    /// <summary>
    /// Gets the hands data captured from the Leap Motion.
    /// </summary>
    /// <value>
    /// The hands data captured from the Leap Motion.
    /// </value>
    List<Hand> Hands
    {
        get { return (CurrentFrame != null && CurrentFrame.Hands.Count > 0) ? CurrentFrame.Hands : null; }
    }

    /// <summary>
    /// Gets a value indicating whether the Leap Motion is ready.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
    /// </value>
    bool IsReady
    {
        get { return (leapController != null && leapController.Devices.Count > 0 && leapController.IsConnected); }
    }
#endif
    
}
