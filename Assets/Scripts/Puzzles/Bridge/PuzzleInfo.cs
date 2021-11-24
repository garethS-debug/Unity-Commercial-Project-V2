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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (SceneSettings.Instance.isSinglePlayer == true)
            {
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
            this.gameObject.tag = "item";

            //Force add to inventory



            //Delete 
            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                Destroy(this.gameObject);
            } 
          
            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                PV.RPC("RPC_DeleteModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
            }
        }

    }

    [PunRPC]
    void RPC_DeleteModel()
    {
        print("Destroying " + this.gameObject.name);
        Destroy(this.gameObject);
    }
}
