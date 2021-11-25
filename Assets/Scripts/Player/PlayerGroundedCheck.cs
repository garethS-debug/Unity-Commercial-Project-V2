using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedCheck : MonoBehaviour
{
public 	NetworkedPlayerController playerController;




	void Awake()
	{
		playerController = GetComponentInParent<NetworkedPlayerController>();
		print("Getting Controller from parent");
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerController = other.gameObject.GetComponent<NetworkedPlayerController>();
			print("Getting Controller from enter" + playerController);
		}
		//if (other.gameObject == playerController.gameObject)
		//	return;
		//	Debug.Log("Hit !!");

		if (playerController == null)
		{
			playerController = other.gameObject.GetComponent<NetworkedPlayerController>();
			print("Getting Controller from the parent again");
		}


		if (other.gameObject != playerController.gameObject)
			{
				playerController.SetGroundedState(true);
			}
		
	



	}


	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == playerController.gameObject)
			return;

	//	Debug.Log("Exit ground !!");

		playerController.SetGroundedState(false);
	}

	void OnTriggerStay(Collider other)
	{
		//if (other.gameObject.tag == "Player")
  //      {
		//	playerController = other.gameObject.GetComponent<NetworkedPlayerController>();

		//}

		
			if (other.gameObject != playerController.gameObject)
			{
				playerController.SetGroundedState(true);
				Debug.Log("Hit Continue  !!" + other.gameObject.name);
			}
	

		if (playerController == null)
        {
			playerController = other.gameObject.GetComponent<NetworkedPlayerController>();
			print("Getting Controller from the parent again" );
		}

	}
}
