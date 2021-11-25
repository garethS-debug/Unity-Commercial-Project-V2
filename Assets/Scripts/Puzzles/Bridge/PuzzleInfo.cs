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


  //  [Header("Photon Settings")]
  ////  PhotonView PV;
  //  private GameObject[] players;
   public GameObject myPlayer;


    public void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
       
            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                myPlayer = other.gameObject;
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

                myPlayer = other.gameObject;
                // myPlayer = SceneSettings.Instance.myPlayer.gameObject;
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
         
                    print("isMIne");
                    //Invert of Lever Choice
                    correctPlayerinTriggerZone = false;
                

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
              
                    if (item)
                    {
                       myPlayer.gameObject.GetComponent< PlayerInventory>().inventory.AddItem(item.item, 1);                    //Photon 

                        PhotonView photonView = PhotonView.Get(this);

                        photonView.RPC("RPC_DeleteModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                        //item.DestroyItem();
                    }
                
            }

            else if (SceneSettings.Instance.isSinglePlayer == true)
            {
                if (item)
                {
                    myPlayer.gameObject.GetComponent<PlayerInventory>().inventory.AddItem(item.item, 1);                    //singlePlayer


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



}
