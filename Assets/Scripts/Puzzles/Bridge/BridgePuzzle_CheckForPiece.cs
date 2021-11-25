using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class BridgePuzzle_CheckForPiece : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Missing Item")]
    public GroundItem missingItem;

    [Header("Fixed Bridge")]
    public Mesh fixedBridgeMesh;
    public GameObject missingPieceBoxCollder;

    [Header("Broken Bridge")]
    public GameObject brokenBridge;
    //public GameObject brokenBridgeMesh;


    [Header("Photon Settings")]
    PhotonView PV;                                              //Setting up photon view 

    public bool DebugOutSideOfNetwork = false;

    public void Start()
    {
        DebugOutSideOfNetwork = false;
        missingPieceBoxCollder.SetActive(false);

      //  missingItem = lever.missingItem;

        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            PV = this.gameObject.GetComponent<PhotonView>();        //Get the photonview on the player
        }
    }

    private void Update()
    {
        if (DebugOutSideOfNetwork == true)
        {
            brokenBridge.GetComponent<MeshFilter>().mesh = fixedBridgeMesh;
            Debug.Log("2 golden keys");
            missingPieceBoxCollder.SetActive(true);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInventory inventory = other.gameObject.GetComponent<PlayerInventory>();

            for (int i = 0; i < inventory.inventory.Container.Items.Count; i++)
            {
                if (inventory.inventory.Container.Items[i].item == missingItem.item)

                {

                    if (SceneSettings.Instance.isMultiPlayer == true)
                    {
                        PV.RPC("RPC_PropChangeModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                    }

                    if (SceneSettings.Instance.isSinglePlayer == true)
                    {
                        FixBridge();
                    }
                    //Container[i].AddAmount(_amount);
                    //  hasItem = true;

                    // if (_item.name == "GoldenKey")
                    // {

                    // }
                }

                else
                {
                    Debug.Log("No Luck");
                    Debug.Log("----------");
                    //Debug.Log("inventory Item = " + inventory.inventory.Container[i].item);
                }
            }



            //    inventory.inventory

        }


    }

    public void OnTriggerStay(Collider other)
    {

    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }


    [PunRPC]
    void RPC_PropChangeModel()
    {
        //if (!PV.IsMine)
        //   return;

        //PhotonView targetPV = PhotonView.Find(targetPropID);

        // if (targetPV.gameObject == null)
        //     return;

        brokenBridge.GetComponent<MeshFilter>().mesh = fixedBridgeMesh;
        Debug.Log("2 golden keys");
        missingPieceBoxCollder.SetActive(true);
        //}

    }
}



//PlayerInventory inventory = other.gameObject.GetComponent<PlayerInventory>();

//for (int i = 0; i < inventory.inventory.Container.Count; i++)
//{
//    if (inventory.inventory.Container[i].item == missingItem.puzzleInfo)
//    {

//        if (SceneSettings.Instance.isMultiPlayer == true)
//        {
//            PV.RPC("RPC_PropChangeModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
//        }

//        if (SceneSettings.Instance.isSinglePlayer == true)
//        {
//            FixBridge();
//        }
//        //Container[i].AddAmount(_amount);
//        //  hasItem = true;

//        // if (_item.name == "GoldenKey")
//        // {

//        // }
//    }

//    else
//    {
//        Debug.Log("No Luck");
//        Debug.Log("----------");
//        Debug.Log("inventory Item = " + inventory.inventory.Container[i].item);
//    }
//}
