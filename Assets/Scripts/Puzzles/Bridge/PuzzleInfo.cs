using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleInfo : MonoBehaviour
{
    public PuzzleItemInfo puzzleInfo;

    public KeyObject inventoryItem;

    public BidgePuzzle_Lever leverInfo;

    bool correctPlayerinTriggerZone;


    [Header("Photon Settings")]
    PhotonView PV;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
       
            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                player = other.gameObject;
                //Invert of Lever Choice
                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 && leverInfo.HumanPlayer == true)
                {
                    correctPlayerinTriggerZone = true;
                }

                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1 && leverInfo.GhostPLayer == true)
                {
                    correctPlayerinTriggerZone = true;
                }

                if (SceneSettings.Instance.DebugMode == true)
                {
                    correctPlayerinTriggerZone = true;
                }
            }

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                PV = other.gameObject.GetComponent<PhotonView>();
                if (PV.IsMine)
                {
                    player = other.gameObject;
                    //Invert of Lever Choice
                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 && leverInfo.HumanPlayer == true)
                    {
                        correctPlayerinTriggerZone = true;
                    }

                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1 && leverInfo.GhostPLayer == true)
                    {
                        correctPlayerinTriggerZone = true;
                    }

                    if (SceneSettings.Instance.DebugMode == true)
                    {
                        correctPlayerinTriggerZone = true;
                    }
                }
            }
  

        }
    }

    private void OnTriggerEnteOnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                //Invert of Lever Choice
                correctPlayerinTriggerZone = false;
            }

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                if (PV.IsMine)
                {
                    //Invert of Lever Choice
                    correctPlayerinTriggerZone = false;
                }

            }

        }
    }


    void OnEnable()
    {
        NetworkedPlayerController.myEvent += PrintStuff;
    }

    void OnDisable()
    {
        NetworkedPlayerController.myEvent -= PrintStuff;
    }

    void PrintStuff()
    {
        if (correctPlayerinTriggerZone == true)
        {
            print("Item has been Picked up");
           // this.gameObject.tag = "item";

            //Force add to inventory

            //PLayer Checking the item
            var item = this.gameObject.GetComponent<Item>();

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                if (PV.IsMine)                                          //Only run this script on the owning player who triggered the event
                {
                    if (player.gameObject.GetComponent<PhotonView>() != null)
                {
                    PV = player.gameObject.GetComponent<PhotonView>();        //Get the photonview on the player
                }

        
                    if (item)
                    {
                       player.gameObject.GetComponent< PlayerInventory>().inventory.AddItem(item.item, 1);                    //Photon 

                        PV.RPC("RPC_DeleteModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                        //item.DestroyItem();
                    }
                }
            }

            else if (SceneSettings.Instance.isSinglePlayer == true)
            {
                if (item)
                {
                    player.gameObject.GetComponent<PlayerInventory>().inventory.AddItem(item.item, 1);                    //singlePlayer


                 //   item.DestroyItem();
                    this.gameObject.SetActive(false);
                }
            }
        
   






            ////Delete 
            //if (SceneSettings.Instance.isSinglePlayer == true)
            //{
            //    this.gameObject.SetActive(false);
            //} 
          
            //if (SceneSettings.Instance.isMultiPlayer == true)
            //{
            //    PV.RPC("RPC_DeleteModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
            //}
        }

    }

    [PunRPC]
    void RPC_DeleteModel()
    {
        print("Destroying " + this.gameObject.name);
        //   Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }


    private void OnApplicationQuit()
    {
        // Clear the player's inventory when they quit
        player.gameObject.GetComponent<PlayerInventory>().inventory.Container.Clear();
    }
}
