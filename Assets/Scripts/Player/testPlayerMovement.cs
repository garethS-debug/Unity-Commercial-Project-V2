using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class testPlayerMovement : MonoBehaviour
{

	//Singleton
	private static testPlayerMovement _instance;
	public static testPlayerMovement Instance { get { return _instance; } }

	[Header("Anim Settings")]
	[HideInInspector] public Animator anim;                                                                                 //Anim settings


	[Header("Idle Anim Settings")]
	float idleAnimFloat;                                                                            //IDLE ANIM BLEND TREE - float variable
	int IdleHash;                                                                                   //Idle anim hash code

	[Header("Velocity Anim Settings")]
	int velocityHash;                                                                               //movement anim hash code	
	float vlocityFloat;
	Vector3 playerVector;
	public float DampTime = 1.1f;                                                                           //Animation dampining time

	[Header("Turning  Settings")]
	int turningHash;
	public bool turning;

	[Header("Movement Settings")]
	private float hDirection;
	private float vDirection;
	private Rigidbody m_Rigidbody;
	private CapsuleCollider m_Capsule;
	public float rotSpeed = 5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;


	[Header("Speed Settings")]
	public float speed = 5f;

	[Header("Sneak Settings")]
	[HideInInspector] public int sneakHash;
	[HideInInspector] public int againstWallHash;


	private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.


	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad(this.gameObject);
	}




	void Start()
	{
		//Settings 
		anim = this.gameObject.GetComponent<Animator>();                                            //Get reference to the animator
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();

		//Animation
		//Ide setup
		IdleHash = Animator.StringToHash("Idle_Float");                                             //Hash number for idle 
		IdleAnimTree();
		//Movement Setup
		velocityHash = Animator.StringToHash("anim_velocity");                                      //Hash number for velocity  
																									//Turning
		turningHash = Animator.StringToHash("anim_turn");                                      //Hash number for turning   

		//sneak
		sneakHash = Animator.StringToHash("anim_sneak");                                      //Hash number for turning    
		againstWallHash = Animator.StringToHash("anim_isAgainstWall");                                      //Hash number for turning   
	}



	// Update is called once per frame
	void Update()
	{





	}


	void FixedUpdate()
	{

		//INput directions																				//Get the input directions 
		float hDirection = Input.GetAxis("Horizontal");
		float vDirection = Input.GetAxis("Vertical");


		m_Move = vDirection * Vector3.forward + hDirection * Vector3.right;                             // send the input directions into a vector 3 


		//Turning the character in opposity diretion
		Vector3 movementDirection = new Vector3(hDirection, 0, vDirection);

		movementDirection.Normalize();


		if (movementDirection != Vector3.zero)                                                          //If moving                                                    

		{
			/*
			transform.forward = -movementDirection;														//Turn character to opposite direction

			print("MoveDirection " + movedirec);

			Quaternion fromAngle = transform.rotation;
			Quaternion toRotation = Quaternion.Euler(movedirec.x, movedirec.y, movedirec.z); 
			transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
			*/


			Move(m_Move);
			//turning = true;

		}



		else
		{
			anim.SetFloat(turningHash, 0);                                                                          // When not moving turning is 0
			anim.SetFloat(velocityHash, 0);                                                                          // When not moving set move is 0
		}

		//Move the character on the fixed update using the m_move direction


	}


	// idle anim
	public void IdleAnimTree()                                                                                      //IDLE ANIM BLEND TREE - Function for randomizing the idle animation 
	{
		idleAnimFloat = Random.Range(0.0f, 1.0f);
		anim.SetFloat(IdleHash, idleAnimFloat);
	}


	public void Move(Vector3 move)
	{

		playerVector = new Vector3(-move.x * speed, 0, -move.z * speed);                                        //Players vector

		float absTurn = Mathf.Abs(m_TurnAmount);                                                                        //Get the absolute number of the turning


		move = transform.InverseTransformDirection(playerVector);                                                       //Transforms a direction from world space to local space.
		m_TurnAmount = Mathf.Atan2(move.x, move.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
		m_ForwardAmount = move.z;
		float turnSpeed = Mathf.Lerp(180, 360, m_ForwardAmount);


		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);




		//Turnign using quaternions
		/*
		float degreeStep = 360;
		m_TurnAmount = Mathf.Atan2(move.x, move.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
		m_ForwardAmount = move.z;
		Vector3 directionToTarget = -move;                                                                              // Get the direction to the target by inverting the move direction
		Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, transform.up);							
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * degreeStep );
		float angle = Quaternion.Angle(transform.rotation, targetRotation);
		*/


		//Update animation
		UpdateAnimator(playerVector);                                                                               //Update the aniumation 
																													//anim.SetFloat(velocityHash, idleAnimFloat);																//convert to single velocity number	


		// Transorm ROT or Quat Rot check
		if (absTurn < 0.1f && ThirdPersonCoverSystem.Instance.leaningAgainstWall == false /*angle < 0.1f */ )
		{


			//Moving
			m_Rigidbody.velocity = playerVector;                                                                        //Move the character

		}


		//print("move =   " + m_ForwardAmount + "turn =  " + absTurn + "QUAT turn: ");


	}

	void UpdateAnimator(Vector3 move)
	{

		if (move.magnitude > 1f)                                                                                    //Magnitude Returns the length of this vector.The length of the vector is square root of(x * x + y * y + z * z).
		{
			move.Normalize();                                                                                       //Makes this vector have a magnitude of 1.
		}


		//m_GroundNormal = Vector3.up;                                                                                //Shorthand for writing Vector3(0, 1, 0).
		//move = Vector3.ProjectOnPlane(move, m_GroundNormal);                                                        //Vector3 The location of the vector on the plane.



		anim.SetFloat(velocityHash, m_ForwardAmount);                                                               // update the velocity animator parameters
		anim.SetFloat(turningHash, m_TurnAmount);                                                                   // update the turning animator parameters


	}

	public void SetAnimFloatCreep(float val, float easing = 1)
	{
		val = (1 - easing) * anim.GetFloat(sneakHash) + easing * val;
		anim.SetFloat(sneakHash, val);
	}

}
