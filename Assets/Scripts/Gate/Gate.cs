using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Gate : MonoBehaviour
{
    private MeshRenderer gateRend;
    private static Gate instance;
    private GameObject player;
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    private Animator anim;

    public RoomManager roomManager;
    PhotonView photonView;

    private void Awake()
    {
        instance = this;
        gateRend = GetComponent<MeshRenderer>();
       // player = GameObject.FindGameObjectWithTag("Player");                //Players dont spawn on load, so this might miss the players. 
        anim = GetComponent<Animator>();
        photonView = PhotonView.Get(this);       //Get PhotonView on this gameobject

    }


    public void Start()
    {
        if (roomManager.networkedPlayer != null)
        {
            player = roomManager.networkedPlayer;                   //This gets the player from the Room Manager, which spawns the player
        }

        else
        {
            print("Error!");
        }
    
    }



    private void Update()
    {
        if (inventoryHuman.twoKeysCollected || inventoryGhost.twoKeysCollected)
        {
           // ChangeColor();
            photonView.RPC("ChangeTheColor", RpcTarget.All);    //Send an RPC call to everyone 
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
      gateRend.material.color = Color.yellow;

  }


    [PunRPC]
    void PauseAnimation()                                      //Making sure the object is destroyed on everyones copy of the game
    {
        anim.enabled = false;

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
