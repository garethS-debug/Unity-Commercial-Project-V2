using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Cinemachine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]




public class NetworkedPlayerController : MonoBehaviour
{

	[Header("Anim Settings")]
	[HideInInspector] public Animator anim;                                                                                 //Anim settings


	////[Header("Idle Anim Settings")]
	//float idleAnimFloat;                                                                            //IDLE ANIM BLEND TREE - float variable
	//int IdleHash;                                                                                   //Idle anim hash code

	[Header("Velocity Anim Settings")]
	int velocityHash;                                                                               //movement anim hash code	
	//float vlocityFloat;
	//Vector3 playerVector;
	public float DampTime = 1.1f;                                                                           //Animation dampining time

	[Header("Turning  Settings")]
	int turningHash;
	public bool turning;

	[Header("Movement Settings")]
	//private float hDirection;
	//private float vDirection;
	//[SerializeField] private Rigidbody m_Rigidbody;
	[SerializeField] private CapsuleCollider m_Capsule;
	//public float rotSpeed = 5f;
	[SerializeField] float m_TurnAmount;
	float m_ForwardAmount;
//	Vector3 m_GroundNormal;
	private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.

	[Header("Jumping")]
	private float verticalVelocity;
	public float gravity = 15.0f;
	public float JumpForce = 10.0f;
	public KeyCode JumpInput = KeyCode.Space;
	[SerializeField] bool grounded;
	//	Vector3 smoothMoveVelocity;
	int jumpHash;
	float jumpDirForward; //-1 is Backwards, +1 is Forwards
	float jumpDirLeftRight; //-1 is Backwards, +1 is Forwards
	bool rayHitGround;
	//[Header("Sneak Settings")]
	//[HideInInspector] public int sneakHash;
	//[HideInInspector] public int againstWallHash;






	[Header("Photon Settings")]
	PhotonView PV;


	[Header("Bonfire")]
	public GameObject bonfireSpawn;
	//[Header("CameraInverstion")]
	//public int MovementInversion = 1;
	//Vector3 movementWithInversion;

	[Header("Rotation")]
	//Vector3 currentROT;
	[HideInInspector] public Vector3 toRot;
	public float lerpSpeed = 0.5f;
	public float durationTime;
	private float smooth;

	[Header("Camera")]
	public Camera camPrefab;
	Camera CameraPrefab;
	public GameObject cameraFollowTarget;
	[HideInInspector] public PlayerCameraController _camControll;
	public bool SpawnTestCam;
	public CinemachineVirtualCamera vcam;
	public CinemachineFreeLook fcam;



	//Vector3 newPOS;
	//public float CamXOffset = 0.0f;
	//public float CamYOffset = 10.0f;
	//public float CamZOffset = 5.0f;
	//public float camFollowDistance = 10.0f;
	//[Range(0.1f, 1.0f)]
	//public float camSmoothing = 0.1f;

	//public GameObject cameraTargetOnSpawn;

	[Header("LobbySettings")]
	public bool isInLobby = false;



	/// <summary>
	/// TO DO :
	/// 
	/// Either neeed to 
	/// reset 'keys' in relation to player orientation trelatie to the world
	/// Resetting player movement based on rotation
	/// 
	/// + Camera moving away from buildings
	/// </summary>
	/// <param name="move"></param>

	//Testing
	GameObject target;
	public float speedMove4;

	//Vector3 lastPos;
	//public Transform objToMonitor; // drag the object to monitor here
	//float threshold = 0.0f; // minimum displacement to recognize a 

	[Header("NewPLayerMovement")]
	public CharacterController controller;              //Motor that drives the player
	public KeyCode SprintInput = KeyCode.LeftShift;
	public Transform cam;
	[Range(0, 1)]
	public float turnSmoothTime = 0.1f;
	public float turningDamp;
	float turnSmoothvelocity;

	[Header("Speed Settings")]
	//public float rotationSpeed = 5f;
	[Range(1, 8)]
	public float anim_walkSpeed;
		[SerializeField] float  sprintSpeed, walkspeed = 8f , smoothTime;
	private float movementSpeed;


	[Header("Perform Action")]
	public KeyCode PerformAction = KeyCode.F;
	public bool PermormingAction;
	public delegate void MyEventDelegate();
	public static event MyEventDelegate myEvent;



	[Header("Test Settings")]
	float vlocityFloat;
	Vector3 playerVector;
	private float hDirection;
	private float vDirection;
	private Rigidbody m_Rigidbody;
	public float rotSpeed = 5f;
	Vector3 m_GroundNormal;


	[Header("Speed Settings")]
	public float speed = 5f;




	private void Awake()
	{

		//Settings 
		anim = this.gameObject.GetComponent<Animator>();                                            //Get reference to the animator
	//	m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();

		//Animation
		//Ide setup
	//	IdleHash = Animator.StringToHash("Idle_Float");                                             //Hash number for idle 


		//Movement Setup
		velocityHash = Animator.StringToHash("anim_velocity");                                      //Hash number for velocity  
		jumpHash = Animator.StringToHash("anim_JumpFloat");
		turningHash = Animator.StringToHash("anim_turn");                                      //Hash number for turning   

		//sneak
		//sneakHash = Animator.StringToHash("anim_sneak");                                      //Hash number for turning    
		//	againstWallHash = Animator.StringToHash("anim_isAgainstWall");                                      //Hash number for turning   

		


		//	controller
		controller = this.gameObject.GetComponent<CharacterController>();



	}

	// Start is called before the first frame update
	void Start()
	{

		if (SceneSettings.Instance.isMultiPlayer == true)
		{
			Debug.Log("Is Multiplayer??");
			//Photon
			PV = GetComponent<PhotonView>();
		}

		if (SceneSettings.Instance.isSinglePlayer == true)
		{
			SceneSettings.Instance.RemoveMultiplayerScript(this.gameObject);
		}

		if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1) // Ghost
        {
			Debug.Log("I am a ghost");
			turnSmoothTime = 0.5f;
        }

		// turn smooth time for ghost is : 0.497



		if (isInLobby && SpawnTestCam == false)
        {
			CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);
			//Camera
			_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
			_camControll.parent = this.gameObject;


			cam = CameraPrefab.gameObject.transform;

			//V-CAM
			if (camPrefab.GetComponentInChildren<CinemachineVirtualCamera>())
			{
				vcam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
				vcam.m_Follow = cameraFollowTarget.transform;
				vcam.m_LookAt = cameraFollowTarget.transform;
			}


			if (camPrefab.GetComponentInChildren<CinemachineFreeLook>())
            {
				fcam = cam.GetComponentInChildren<CinemachineFreeLook>();
				fcam.m_Follow = cameraFollowTarget.transform;
				fcam.m_LookAt = cameraFollowTarget.transform;
			}



			//Display the parent's name in the console.
			//	Debug.Log("Player's Parent: " + CameraPrefab.gameObject.transform.parent.name);
			controller = this.gameObject.GetComponent<CharacterController>();
		}

		/*

		if (SpawnTestCam)
        {
			testCameraPrefab = Instantiate(testCameraPrefab, this.transform.position, testCameraPrefab.transform.rotation);
			//Camera
			_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
			_camControll.parent = this.gameObject;
			cam = CameraPrefab.gameObject.transform;
			//Display the parent's name in the console.
			//	Debug.Log("Player's Parent: " + CameraPrefab.gameObject.transform.parent.name);
			controller = this.gameObject.GetComponent<CharacterController>();
		}

		*/


		if (isInLobby == false && SpawnTestCam == false)
        {
			if (SceneSettings.Instance.isMultiPlayer == true)
			{
			

			//If multiplayer and not my game object
			if (!PV.IsMine)
			{
				if (GetComponentInChildren<Camera>() != null)
				{
					Destroy(GetComponentInChildren<Camera>().gameObject);
				}

			}

			if (PV.IsMine)
			{
					CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);


					//Camera
					_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
					_camControll.parent = this.gameObject;

					cam = CameraPrefab.gameObject.transform;

					//V-CAM
					vcam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
					vcam.m_Follow = cameraFollowTarget.transform;

			
				controller = this.gameObject.GetComponent<CharacterController>();
			}

			}

			 if (SceneSettings.Instance.isSinglePlayer == true)
			{
					if (GetComponentInChildren<Camera>() != null)
					{
						Destroy(GetComponentInChildren<Camera>().gameObject);
					}


				
					CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);
					//Camera
					_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
					_camControll.parent = this.gameObject;

					cam = CameraPrefab.gameObject.transform;
					controller = this.gameObject.GetComponent<CharacterController>();

				//V-CAM
				vcam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
				vcam.m_Follow = cameraFollowTarget.transform;
			}



		}
	

		//Instantiate(camPrefab, camPrefab.GetComponent<PlayerCameraController>().FrontFacingPOS, Quaternion.identity);

		//MovementInversion = 1;

		//lastPos = this.transform.position;

	}

	// Update is called once per frame
	void Update()
	{
		//Move();


		//this wont allow for backwards movement
		//Rotate(toRot);
		if (isInLobby == false)
		{
			if (SceneSettings.Instance.isMultiPlayer == true)
			{
				if (!PV.IsMine)
				{
					return;
				}
			}
		}

		//--- moved Move and Jump here to stop camera null error 11/12/2021
		Move5();
		Jump();

		//Existing Movement Script
		//m_Rigidbody.MovePosition(m_Rigidbody.position + transform.TransformDirection(movementWithInversion) * Time.fixedDeltaTime);

		//Move 3 is the current edition 



		PerformActionCheck();


	}

	private void LateUpdate()
	{
		
	}


	void FixedUpdate()
	{
		//movied --- Move5(); and Jump(); from here to update to remove juddering camera 11/12/2021
	
	}



	public void Move5()
    {
		float horizontalInput = Input.GetAxisRaw("Horizontal"); //-1 and +1 (-1 for left , + 1 for right)
		float verticalInput = Input.GetAxisRaw("Vertical"); // -1 and +1  (+ 1 up, - 1 down) 

		Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

		if (direction.magnitude >= 0.1)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //How much to rotate the player on the y axis to point in the movement direction. ATan2 is a math function that returns an angle between the x axis and an angle starting 0 and terminating at x,y taking into account unity forward 
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothTime); //Smoothed angle of rotaiton 

			transform.rotation = Quaternion.Euler(0f, angle, 0f);


			if (controller.isGrounded || rayHitGround == true)
            {
				movementSpeed = Mathf.Lerp(movementSpeed, (Input.GetKey(SprintInput) ? sprintSpeed : walkspeed), smoothTime);
			}
		

			
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;        //move in direction of camera
			controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);


			//m_TurnAmount = Mathf.Atan2(movementDirection.x, movementDirection.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
			float normalDirection = moveDir.magnitude;
			m_ForwardAmount = Mathf.Lerp(m_ForwardAmount, normalDirection * movementSpeed * anim_walkSpeed * Time.deltaTime, lerpSpeed * Time.deltaTime * 10);


			 jumpDirForward = moveDir.normalized.z; //-1 is Backwards, +1 is Forwards
			 jumpDirLeftRight = moveDir.normalized.x; //-1 is Backwards, +1 is Forwards



			// ------------> removed 10/12/2021 to revert back to original player controller.
			//	print("Move Dir Forward" + moveDir.normalized.z + "Move Dir Side" + moveDir.normalized.x);
			/*

			//pressing Up and left / right 
			if (verticalInput < 0 )
			{
				if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1) // Ghost
				{
					Debug.Log("I am a ghost");
					turnSmoothTime = 1;
				}
			}


			if (Input.GetKey(SprintInput))
            {
				if (verticalInput > 0 && horizontalInput < 0 || verticalInput > 0 && horizontalInput > 0)
				{
					turnSmoothTime = 0.3f;

				}

				//pressing down and left / right 
				if (verticalInput < 0 && horizontalInput < 0 || verticalInput < 0 && horizontalInput > 0)
				{
					turnSmoothTime = 0.6f;
					
				}
			}
		




			else
			{
				if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1) // Ghost
				{
					Debug.Log("I am a ghost");
					turnSmoothTime = 0.5f;
				}
			}


			*/
		

			//	jumpDir = 
			//	print(m_ForwardAmount);

			UpdateAnimator();                                                                               //Update the aniumation 

		}

		else
        {
			m_ForwardAmount = 0;
			m_TurnAmount = 0;
			UpdateAnimator();
		}
	}







	public void Move6()
	{
		float horizontalInput = Input.GetAxisRaw("Horizontal"); //-1 and +1 (-1 for left , + 1 for right)
		float verticalInput = Input.GetAxisRaw("Vertical"); // -1 and +1  (+ 1 up, - 1 down) 

		Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

		if (direction.magnitude >= 0.1)
		{

			if (horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0)
			{
				//Normal camera setup 
				//-------> Set Cam Position to behind shoulder


				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //How much to rotate the player on the y axis to point in the movement direction. ATan2 is a math function that returns an angle between the x axis and an angle starting 0 and terminating at x,y taking into account unity forward 
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothTime); //Smoothed angle of rotaiton 
																																   //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, angle, 0f), turningDamp* Time.deltaTime);
																																   //	transform.rotation = Quaternion.Euler(0f, angle, 0f);



				float targetAngle2 = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + this.gameObject.transform.eulerAngles.y; //How much to rotate the player on the y axis to point in the movement direction. ATan2 is a math function that returns an angle between the x axis and an angle starting 0 and terminating at x,y taking into account unity forward 
				float angle2 = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle2, ref turnSmoothvelocity, turnSmoothTime); //Smoothed angle of rotaiton 
				transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, angle2, 0f), turningDamp * Time.deltaTime);



				if (Input.GetKey(SprintInput) == false)
                {
					//pressing Up and left / right 
					if (verticalInput > 0 && horizontalInput < 0 || verticalInput > 0 && horizontalInput > 0)
					{
						turnSmoothTime = 0.11f;
						turningDamp = 24;
					}


					// just left or right
					if (verticalInput == 0 && horizontalInput < 0 || verticalInput == 0 && horizontalInput > 0)
					{
						turnSmoothTime = 0.03f;
						turningDamp = 4.12f;
					}
				}


				if (Input.GetKey(SprintInput) == true)
				{
					//pressing Up and left / right 
					if (verticalInput > 0 && horizontalInput < 0 || verticalInput > 0 && horizontalInput > 0)
					{
						turnSmoothTime = 0.042f;
						turningDamp = 14.6f;
					}


					// just left or right
					if (verticalInput == 0 && horizontalInput < 0 || verticalInput == 0 && horizontalInput > 0)
					{
						turnSmoothTime = 0.005f;
						turningDamp = 3f;
					}
				}






				movementSpeed = Mathf.Lerp(movementSpeed, (Input.GetKey(SprintInput) && verticalInput > 0 ? sprintSpeed : walkspeed), smoothTime);

				Vector3 moveDir = Quaternion.Euler(0f, targetAngle2, 0f) * Vector3.forward;        //move in direction of camera







				controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);

				//m_TurnAmount = Mathf.Atan2(movementDirection.x, movementDirection.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
				float normalDirection = moveDir.magnitude;
				m_ForwardAmount = Mathf.Lerp(m_ForwardAmount, normalDirection * movementSpeed * anim_walkSpeed * Time.deltaTime, lerpSpeed * Time.deltaTime * 10);


				jumpDirForward = moveDir.normalized.z; //-1 is Backwards, +1 is Forwards
				jumpDirLeftRight = moveDir.normalized.x; //-1 is Backwards, +1 is Forwards

				//	print("Move Dir Forward" + moveDir.normalized.z + "Move Dir Side" + moveDir.normalized.x);



				//	jumpDir = 
				//	print(m_ForwardAmount);
			}


			if (verticalInput <0)
            {
				//Invert the camera
				//-------> Set Cam Position to facing player





				//----> Invert the movement



			}













			UpdateAnimator();                                                                               //Update the aniumation 

		}




		else
		{
			m_ForwardAmount = 0;
			m_TurnAmount = 0;
			UpdateAnimator();
		}

	}










	void UpdateAnimator()
	{
		anim.SetFloat(velocityHash, m_ForwardAmount);                                                               // update the velocity animator parameters
		anim.SetFloat(turningHash, m_TurnAmount);                                                                   // update the turning animator parameters
	}









	void Jump()
	{

		if (SceneSettings.Instance.isMultiPlayer == true)
		{
			if (isInLobby == true)
			{
				if (controller.isGrounded)
				{
					verticalVelocity = -gravity * Time.deltaTime;

					if (Input.GetKey(JumpInput))
					{
						print("Jumping");
						verticalVelocity = JumpForce;
						anim.SetBool("anim_Jumping", true); // Set jumping 

						if (jumpDirForward == 0 && jumpDirLeftRight == 0)
						{
							anim.SetFloat(jumpHash, 0);
						}

						if (jumpDirForward > 0 && jumpDirLeftRight <= 0)
						{
							//	print("Jump foward " + jumpDirForward );
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirForward < 0 && jumpDirLeftRight >= 0)
						{
							//print("Jump Backwards " + jumpDirForward);
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirLeftRight > 0 && jumpDirForward >= 0)
						{
							//print("Jump Side - forward " + jumpDirLeftRight);
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirLeftRight < 0 && jumpDirForward <= 0)
						{
							//print("Jump Side - forward " + jumpDirLeftRight);
							anim.SetFloat(jumpHash, 1);
						}


						//	anim.SetFloat(jumpHash, );

					}
				}

				else
				{
					verticalVelocity -= gravity * Time.deltaTime;
					anim.SetBool("anim_Jumping", false); // Set jumping 
				}

				Vector3 jumpvector = new Vector3(0, verticalVelocity, 0);
				controller.Move(jumpvector * Time.deltaTime);
				/*
						if (Input.GetKeyDown(JumpInput) && grounded)
						{
						//	m_Rigidbody.AddForce(transform.up * jumpForce);
						}

						*/
			}

			if (isInLobby == false)
			{

				if (!PV.IsMine)
				{
					return; //If multiplayer and not my game object
					print("PV is not mine Jumping");
				}

				if (PV.IsMine)
				{
					print("Controller grounded status " + controller.isGrounded);


					
						// Bit shift the index of the layer (8) to get a bit mask
						int layerMask = 1 << 8;

						// This would cast rays only against colliders in layer 8.
						// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
						layerMask = ~layerMask;

						RaycastHit hit;
						// Does the ray intersect any objects excluding the player layer
						if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, JumpForce-6, layerMask))
						{
						//	Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
							//Debug.Log("Did Hit");

						rayHitGround = true;
					}
						else
						{
						//	Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
							//Debug.Log("Did not Hit");
						rayHitGround = false;
						}







						if (rayHitGround == true)
					{

						if (Input.GetKey(JumpInput) || Input.GetKeyDown(JumpInput))
						{
							verticalVelocity = JumpForce;
							anim.SetBool("anim_Jumping", true); // Set jumping 


							if (jumpDirForward > 0 || jumpDirLeftRight <= 0)
							{
								//	print("Jump foward " + jumpDirForward );
								anim.SetFloat(jumpHash, 1);
							}


						}
					}

					else
					{
						verticalVelocity -= gravity * Time.deltaTime;
						anim.SetBool("anim_Jumping", false); // Set jumping 
					}

					Vector3 jumpvector = new Vector3(0, verticalVelocity, 0);
					controller.Move(jumpvector * Time.deltaTime);
				}


				


			}

		}
		//------------------------------------------------------->


		if (SceneSettings.Instance.isSinglePlayer == true)
		{
			
				if (controller.isGrounded)
				{
					verticalVelocity = -gravity * Time.deltaTime;

					if (Input.GetKey(JumpInput) || Input.GetKeyDown(JumpInput))
					{
						verticalVelocity = JumpForce;
						anim.SetBool("anim_Jumping", true); // Set jumping 


					if (jumpDirForward > 0 || jumpDirLeftRight <= 0)
					{
						//	print("Jump foward " + jumpDirForward );
						anim.SetFloat(jumpHash, 1);
					}



					/*
						if (jumpDirForward == 0 && jumpDirLeftRight == 0 )
						{
							anim.SetFloat(jumpHash, 0);
						}

						if (jumpDirForward > 0 && jumpDirLeftRight <= 0)
						{
							//	print("Jump foward " + jumpDirForward );
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirForward < 0 && jumpDirLeftRight >= 0)
						{
							//print("Jump Backwards " + jumpDirForward);
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirLeftRight > 0 && jumpDirForward >= 0)
						{
							//print("Jump Side - forward " + jumpDirLeftRight);
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirLeftRight < 0 && jumpDirForward <= 0)
						{
							//print("Jump Side - forward " + jumpDirLeftRight);
							anim.SetFloat(jumpHash, 1);
						}

					*/
					//	anim.SetFloat(jumpHash, );

				}
				}

				else
				{
					verticalVelocity -= gravity * Time.deltaTime;
					anim.SetBool("anim_Jumping", false); // Set jumping 
				}

				Vector3 jumpvector = new Vector3(0, verticalVelocity, 0);
				controller.Move(jumpvector * Time.deltaTime);
				/*
						if (Input.GetKeyDown(JumpInput) && grounded)
						{
						//	m_Rigidbody.AddForce(transform.up * jumpForce);
						}

						*/
			}

			




	
	}


	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}







	public void UpdateCamPosition(PlayerCameraController.CameraPosition cam)
	{

		switch (cam)
		{
			case PlayerCameraController.CameraPosition.FrontFacing:

				/*

					Vector3 oldPOs = CameraPrefab.transform.position;
					Vector3 offset1 = new Vector3(transform.position.x + CamXOffset, transform.position.y + CamYOffset, transform.position.z - CamZOffset);

					Vector3 newPOS1 = Vector3.Lerp(oldPOs, offset1 - transform.right * camFollowDistance, 0.1f);



					//Change in Position 



					Vector3 offsetToCheck = objToMonitor.position - lastPos;
					if (offsetToCheck.x > threshold)
					{
						lastPos = objToMonitor.position; // update lastPos
						print("// code to execute when X is getting bigger : " );                         // code to execute when X is getting bigger
					}
					else
					if (offsetToCheck.x < -threshold)
					{
						lastPos = objToMonitor.position; // update lastPos
						print(" // code to execute when X is getting smaller  : " );                   // code to execute when X is getting smaller 
					}

				//TESTING



				CameraPrefab.transform.position = newPOS1;

						*/


				CameraPrefab.transform.LookAt(transform.position);


				//Player Controls



				break;
			case PlayerCameraController.CameraPosition.OverShoulder:

				/*
			Vector3 oldPOS = CameraPrefab.transform.position;
			newPOS = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y + 10, this.gameObject.transform.position.z);

			Vector3 offset = new Vector3(transform.position.x, transform.position.y + CamYOffset, transform.position.z);
			Vector3 newPOS2 = Vector3.Lerp(oldPOS, offset - -transform.forward * camFollowDistance, 0.1f);

			print("OverShoulder Vector Difference in Cam : " + (oldPOS - newPOS2));


			CameraPrefab.transform.position = newPOS2;
			CameraPrefab.transform.LookAt(transform.position);

				*/
				CameraPrefab.transform.LookAt(this.gameObject.transform);



				//Player Controls





				break;

			default:
				print("Incorrect intelligence level.");
				break;
		}

	}

	public Vector3 UpdateControlPosition(PlayerCameraController.CameraPosition cam)
	{

		switch (cam)
		{
			case PlayerCameraController.CameraPosition.FrontFacing:

				float moveHorizontal = Input.GetAxis("Horizontal");
				float moveVertical = Input.GetAxis("Vertical");
				Vector3 moveDir;

				if (moveHorizontal > 0.1f || moveHorizontal < 0)
				{
					Vector3 moveDirFrontFacing = new Vector3(0, 0, Input.GetAxisRaw("Horizontal")).normalized; //Lock movement to forward and back 

					moveDir = moveDirFrontFacing;
					return moveDir;
				}

				else if (moveVertical > 0.1f || moveVertical < 0.0f)
				{
					Vector3 moveDirFrontFacing2 = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized; //Lock movement to forward and back 
					moveDir = moveDirFrontFacing2;
					return moveDir;
				}

				else
				{
					moveDir = new Vector3(0, 0, Input.GetAxisRaw("Horizontal")).normalized;
					return moveDir;
				}

				//Player Controls



				break;
			case PlayerCameraController.CameraPosition.OverShoulder:



				//Player Controls

				Vector3 moveDireOverShoulder = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;


				//Player Controls
				return moveDireOverShoulder;




				break;

			default:

				//Player Controls
				Vector3 test3 = new Vector3(0, 0, 0);

				//Player Controls
				return test3;
				//print("Incorrect intelligence level.");
				break;
		}



	}

	void OnDrawGizmosSelected()
	{
		// Draws a 5 unit long red line in front of the object
		Gizmos.color = Color.red;
		Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
		Gizmos.DrawRay(transform.position, direction);
	}


	private void PerformActionCheck()
	{
	 if (Input.GetKeyDown(PerformAction))
        {
			PermormingAction = true;
			print("player controller performing action");

			if (myEvent != null)
			{
				myEvent();
			}


		}
		if (Input.GetKeyUp(PerformAction))
		{
			print("player controller not performing action");
			PermormingAction = false;
		}
	
	}

}
