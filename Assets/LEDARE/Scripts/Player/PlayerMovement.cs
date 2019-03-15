using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
internal class PlayerMovement : MonoBehaviour
{
	enum MovementState
	{
		Idle,
		Running,
		Accelerating,
		Decelerating,
		Turning,
		JustTurned,
		Locked
	}

	bool applyForce;
	float forceOfApplied;

	//Animations
	Animator anim;
	//Variables
	public PlayerDatabase data;
	//Controller
	CharacterController controller;
	//Input
	float horizontalInput;

	float verticalInput;
	Vector2 planarInput;
	bool jumpInputDown;

	bool jumpInputHold;
	//Movement
	float speed;

	public Vector2 PlanarDirection { get; private set; }
	public Vector3 motion;
	public Vector2 PlanarMovement { get; private set; }
	float verticalMovement;
	Vector2 lastPlanarInput = Vector2.zero;
	float decelerationMagnitude;

	Vector2 airMove = Vector2.zero;
	//private float lastPlanarMagnitude = 0f;
	//States
	[SerializeField] MovementState moveState = MovementState.Idle;
	public bool grounded = false;
	public bool groundStick = false;
	//Camera
	Transform mainCamera;
	//References
	float movementSmoothDampReference = 0f;

	//Velocity & Acceleration
	Vector3 lastPosition = Vector3.zero;
	public Vector3 velocity = Vector3.zero;
	Vector3 lastVelocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;
	Vector3 initialJumpVelocity = Vector3.zero;

	[Header("Debug groundStick")]
	public Vector3 groundStickOffset;
	public float groundStickDistance = 5f;
	Vector3 point;

	public float tilt = 100f;

	void Awake()
	{
		if (!data)
		{
			data = Resources.Load("PlayerDatabase") as PlayerDatabase;
		}
		controller = GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator>();
	}

	void Start()
	{
		
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	void Update()
	{
	  

		CalculateVelocityAndAcceleration();

		CheckForCollisions();

		ApplyGravity();

		GroundCheck();

		SetVariables();

		Jump();

		JumpForce();

		Tilt();

		Rotate();

		SwitchMovementState();

		Move();

		ApplyMotion();

	}

	void GroundCheck()
	{

		point = controller.center;
		point -= new Vector3(0, controller.height / 2 - controller.radius, 0);
		point = transform.TransformPoint(point);
	 
		
		RaycastHit hit; 
		bool check = Physics.SphereCast(new Ray(point, Vector3.down), controller.radius, out hit ,data.groundCheckDistance);


		if (check && !grounded && !hit.collider.isTrigger) 
		{
			
		   
			decelerationMagnitude = 0f;
		}
		if (!check && grounded) //JUST Left THE GROUND
		{
		   
			initialJumpVelocity = velocity;

			StartCoroutine(RotateTowards(new Vector3(initialJumpVelocity.x, 0, initialJumpVelocity.z), data.atJumpRotationTime));

		}
		   
		grounded = check && !hit.collider.isTrigger;
	}

	void SetVariables()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
		jumpInputDown = Input.GetButtonDown("Jump");
		jumpInputHold = Input.GetButton("Jump");
		lastPlanarInput = planarInput;
		planarInput = new Vector2(horizontalInput, verticalInput);
		planarInput = Vector2.ClampMagnitude(planarInput, 1f);

	}

	void ApplyGravity()
	{
		if (groundStick)
		  verticalMovement = 0f;
		   
		else
			verticalMovement -= data.gravity;
	}

	public void Jump()
	{
		
		//implement hold jump
		if (grounded && jumpInputDown)
		{
			anim.SetTrigger("Jump");
			groundStick = false;
			StartCoroutine(HoldJump());
		}
	}

	public void ApplyForce(float force)  // Om denna metod kallas kommer spelaren hoppa högre. 
	{
		forceOfApplied = force;
		applyForce = true;

	}

	void JumpForce()
	{
		if (applyForce)
		{
			anim.SetTrigger("Jump");
			groundStick = false;
			StartCoroutine(JumpForce(forceOfApplied));
			applyForce = false;
		}
	}

	void Rotate()
	{
		if (HasMovementInput())
		{
			if (grounded)
			{
				Vector3 dir = Vector3.RotateTowards(transform.forward, new Vector3(velocity.x, 0, velocity.z).normalized, data.rotationSpeed * Time.deltaTime, 0.0F);

				transform.rotation = Quaternion.LookRotation(dir);
			}
			else
			{
				Vector3 dir = Vector3.RotateTowards(transform.forward, new Vector3(velocity.x, 0, velocity.z).normalized, data.airborneRotationSpeed * Time.deltaTime, 0.0F);

				transform.rotation = Quaternion.LookRotation(dir);
			}
		   
		}
	}

	void Tilt()
	{
	
		Quaternion rot = transform.rotation;
		Vector3 tiltAxis = Vector3.Cross(Vector3.up, new Vector3(velocity.x, 0 ,velocity.z));

		rot = Quaternion.AngleAxis(acceleration.normalized.magnitude * tilt, tiltAxis) * rot;
	}

	void SwitchMovementState()
	{
		switch (moveState)
		{
			case MovementState.Idle:
				if (HasMovementInput())
				{
					moveState = MovementState.Accelerating;
				}
				break;

			case MovementState.Running:

				break;

			case MovementState.Accelerating:

				if (lastPlanarInput.magnitude > planarInput.magnitude)
				{
					moveState = MovementState.Decelerating;
					decelerationMagnitude = lastPlanarInput.magnitude;
				}
				break;

			case MovementState.Decelerating:

				if (lastPlanarInput.magnitude < planarInput.magnitude)
				{
					moveState = MovementState.Accelerating;
				}
				break;

			case MovementState.Turning:

				break;

			case MovementState.JustTurned:

				break;
			case MovementState.Locked:
				//No way out
				break;
		}
	}

	void Move()
	{
		Vector3 dir = mainCamera.TransformDirection(new Vector3(planarInput.x,0, planarInput.y));
		planarInput.x = dir.x;
		planarInput.y = dir.z;
		Debug.DrawRay(transform.position, new Vector3(planarInput.x, 0, planarInput.y) * 5, Color.red);

		if (grounded)
			GroundMove();
		else
			AirMove();
	}

	void GroundMove()
	{   
		
		switch (moveState)
		{
			case MovementState.Idle:
				speed = 0f;
				break;

			case MovementState.Running:
				PlanarDirection = Vector2.Lerp(PlanarDirection, planarInput, Time.deltaTime * data.directionDelta);
				speed = data.maxSpeed;
				break;

			case MovementState.Accelerating:
				PlanarDirection = Vector2.Lerp(PlanarDirection, planarInput, Time.deltaTime * data.directionDelta);
				speed = Mathf.SmoothDamp(speed, data.maxSpeed, ref movementSmoothDampReference, data.accelerationTime);
				break;

			case MovementState.Decelerating:
				PlanarDirection = new Vector2(velocity.x, velocity.z).normalized * decelerationMagnitude;
				
				speed = Mathf.Lerp(speed, data.maxSpeed * planarInput.magnitude, data.deceleration*Time.deltaTime);
				break;

			case MovementState.Turning:

				break;

			case MovementState.JustTurned:

				break;
			case MovementState.Locked:
				break;
		}

		PlanarMovement = PlanarDirection*speed;
	}

	void AirMove()
	{
		if ((moveState != MovementState.Locked))
		{
			if (planarInput.magnitude != 0f)
			{
				Vector2 inp = planarInput.normalized;
				airMove = inp*data.airControlAcceleration;

				float factor = (PlanarMovement.magnitude / data.maxSpeed);

				factor = (factor < data.minPercentOfAirControl) ? data.minPercentOfAirControl : factor;
				airMove *= factor;

				//if destructive use deceleration
				if (PlanarMovement.magnitude > (PlanarMovement + airMove).magnitude)
				{
					airMove /= data.airControlAcceleration;
					airMove *= data.airControlDeceleration;
				}
				//if constructive use acceleration

				PlanarMovement += airMove;
				


			}

			else
			{
				PlanarMovement = Vector2.Lerp(PlanarMovement, Vector2.zero, data.passiveAirDeceleration * Time.deltaTime);
			}
			
		}
	}

	void ApplyMotion()
	{
		motion.x = PlanarMovement.x;
		motion.z = PlanarMovement.y;
		motion = Vector3.ClampMagnitude(motion, data.maxSpeed);
		verticalMovement = Mathf.Clamp(verticalMovement, -data.terminalVelocity, float.MaxValue);
		motion.y = verticalMovement;
		controller.Move(motion * Time.deltaTime);

		//animations
		anim.SetFloat("Magnitude", new Vector3(velocity.x, 0, velocity.z).magnitude / data.maxSpeed);
	}

	//BOOLS
	bool HasMovementInput()
	{
		return !(Mathf.Approximately(horizontalInput, 0)) || !(Mathf.Approximately(verticalInput, 0));
	}

	//IEnumerators
	IEnumerator LockMovement(float time, MovementState breakState)
	{
		moveState = MovementState.Locked;
		float t = time;
		while (t > 0)
		{
			t -= Time.deltaTime;
			yield return 0;
		}
		moveState = breakState;
		yield return 0;
	}

	IEnumerator RotateTowards(Vector3 dir, float time)
	{

		float t = time;

		while(t > 0)
		{
			t -= Time.deltaTime;
			
			Vector3 direction = Vector3.RotateTowards(transform.forward, dir.normalized, data.atJumpRotationSpeed * Time.deltaTime, 0.0F);

			transform.rotation = Quaternion.LookRotation(direction);

			yield return 0;
		}

		yield return 0;
		
	}

	public IEnumerator JumpForce(float force)
	{
		verticalMovement = force;
		float t = data.frameDelay;
		float jumpModifier = force;

		if (t > 0)
		{
			//t -= Time.deltaTime;

			verticalMovement *= data.jumpReleasePercentage;
			yield break;
		}

		t = data.jumpHoldTime;
		while (t > 0)
		{
			verticalMovement = jumpModifier;
			jumpModifier -= data.jumpHoldDecay * Time.deltaTime;
		
			t -= Time.deltaTime;

			yield return 0f;
		}

		yield return 0f;
	}

	IEnumerator HoldJump()
	{
		
		verticalMovement = data.initialJumpSpeed;
		float t = data.frameDelay;
		float jumpModifier = data.initialJumpSpeed;
		//float refe = 0f;
		//bool decay = false;
		float jumpSpeed = data.initialJumpSpeed;
		while (t > 0)
		{

			t -= Time.deltaTime;
			

			if (Input.GetButtonUp("Jump"))
			{
				verticalMovement *= data.jumpReleasePercentage;
				yield break;
			}
			verticalMovement = jumpSpeed;

			yield return 0f;
			
		}

		t = data.jumpHoldTime;
		while (t > 0)
		{ 
			if (jumpInputHold)
			{
				verticalMovement = jumpModifier;
				jumpModifier -= data.jumpHoldDecay*Time.deltaTime;

			}
			else
			{
				yield break;
			}
			t -= Time.deltaTime;


			yield return 0f;
		}

		yield return 0f;

	}
	//Collisions
	void CheckForCollisions()
	{
		if (controller.collisionFlags == CollisionFlags.None)
		{
			groundStick = false;
		}

		if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
		{

		}

		if (controller.collisionFlags == CollisionFlags.Sides)
		{

		}

		if ((controller.collisionFlags & CollisionFlags.Above) != 0)
		{
			verticalMovement = Mathf.Clamp(verticalMovement, float.MinValue, 0);
		}

		if (controller.collisionFlags == CollisionFlags.Above)
		{
			verticalMovement = Mathf.Clamp(verticalMovement, float.MinValue, 0);
		}
	 

		groundStick = CheckGroundStick();

	}

	bool CheckGroundStick()
	{
		point = controller.center;
		point -= new Vector3(0, controller.height / 2 - controller.radius, 0);
		point = transform.TransformPoint(point);
	  
		RaycastHit hit;
		bool collsion = Physics.SphereCast(new Ray(point, Vector3.down), controller.radius, out hit, groundStickDistance);
		return collsion && !hit.collider.isTrigger;

	}

	//Calculations
	void CalculateVelocityAndAcceleration()
	{
		velocity = (transform.position - lastPosition)/Time.deltaTime;

		acceleration = (velocity - lastVelocity)/Time.deltaTime;

		lastPosition = transform.position;
		lastVelocity = velocity;

   
	}
}
