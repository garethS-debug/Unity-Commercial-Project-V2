using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Gate : MonoBehaviour
{
    public MeshRenderer gateRend;
    public Renderer gateRend2;
    private static Gate instance;
    private GameObject player;
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    private Animator anim;

    public RoomManager roomManager;
    PhotonView photonView;

    public bool hasCollectedItems;

    private void Awake()
    {
        instance = this;
       // gateRend = GetComponent<MeshRenderer>();
       // player = GameObject.FindGameObjectWithTag("Player");                //Players dont spawn on load, so this might miss the players. 
        anim = GetComponent<Animator>();
        photonView = PhotonView.Get(this);       //Get PhotonView on this gameobject

    }


    public void Start()
    {
        hasCollectedItems = false;


      //  player = roomManager.networkedPlayerManager.gameObject.GetComponent<NetWorkedPlayerManager>().playerInScene;                   //This gets the player from the Room Manager, which spawns the player






    }



    private void Update()
    {
        if (inventoryHuman.twoKeysCollected || inventoryGhost.twoKeysCollected)
        {
            if (hasCollectedItems = false)
            {
                // ChangeColor();
                photonView.RPC("ChangeTheColor", RpcTarget.All);    //Send an RPC call to everyone 
                hasCollectedItems = true;
            }

        }
    }

    public void OnTriggerStay(Collider other)
    {
        if ((inventoryHuman.twoKeysCollected || inventoryGhost.twoKeysCollected) && other.gameObject.tag == "Player")
        {
         
            photonView.RPC("OpenDoor", RpcTarget.All);    //Send an RPC call to everyone 

        }
    }

    public void pauseAnimationEvent()
    {
      //  anim.enabled = false;
        photonView.RPC("PauseAnimation", RpcTarget.All);    //Send an RPC call to everyone 
    }

    /*
  public void ChangeColor()
  {
      gateRend.material.color = Color.yellow;
  }
      */

    [PunRPC]
  void OpenDoor()                                      //Making sure the object is destroyed on everyones copy of the game
  {
      print("Door Open");
      anim.SetTrigger("OpenDoor");

  }



  [PunRPC]
  void ChangeTheColor()                                      //Making sure the object is destroyed on everyones copy of the game
  {
     // gateRend.material.color = Color.yellow;
        gateRend2.material.color = Color.yellow;
    }


    [PunRPC]
    void PauseAnimation()                                      //Making sure the object is destroyed on everyones copy of the game
    {
      //  anim.enabled = false;                         //This removes the transform psotions that the animation was giving the door, and the door literally falls over. 

        print("Stop Door Closing");

    }
    /*
      [PunRPC]
      void DestroyObject()                                      //Making sure the object is destroyed on everyones copy of the game
      {
          print("Item being destroyed");
          this.gameObject.SetActive(false);

      }
      */

}
