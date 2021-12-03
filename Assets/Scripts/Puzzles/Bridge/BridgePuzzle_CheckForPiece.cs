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



    [Header("Key")]
    public GameObject KeyInScene;



    [Header("Photon Settings")]
    PhotonView PV;                                              //Setting up photon view 

    public bool DebugOutSideOfNetwork = false;

    public void Start()
    {
        KeyInScene.gameObject.SetActive(false);

        DebugOutSideOfNetwork = false;
        missingPieceBoxCollder.SetActive(false);

      //  missingItem = lever.missingItem;

        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            PV = this.gameObject.GetComponent<PhotonView>();        //Get the photonview on the player
        }


        if (SceneSettings.Instance.DebugMode == true)
        {
            missingPieceBoxCollder.SetActive(true);
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

            if (inventory != null )
            {

            if (missingItem != null)
                {

                

            if (inventory.inventory.database.Items.Length > 0)
            {
                for (int i = 0; i < inventory.inventory.database.Items.Length ; i++)
                {
                    Debug.Log("-------------------");
                    Debug.Log("Looping through inv");

                    //  Debug.Log("inventory.inventory.database.Items[i] = " + inventory.inventory.database.Items[i]);       //Key Object
                    //   Debug.Log("missingItem.item =  " + missingItem.item);       //Key Object
                    
                    if (inventory.inventory.Container.Items[i].ID == missingItem.item.Id)

                    {
                        Debug.Log("-------------------");
                        Debug.Log("found our boy");

                        if (SceneSettings.Instance.isMultiPlayer == true)
                        {
                            PhotonView photonView = PhotonView.Get(this);
                            photonView.RPC("RPC_PropChangeModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                            photonView.RPC("RPC_ShowKey", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                        }

                        if (SceneSettings.Instance.isSinglePlayer == true)
                        {
                            FixBridge();
                            s_ShowKey();
                        }
             
                    }

                   
                        
                }
            }

            }


            //    inventory.inventory

        }
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

    public void FixBridge()
    {
        brokenBridge.GetComponent<MeshFilter>().mesh = fixedBridgeMesh;
        missingPieceBoxCollder.SetActive(true);
    }


    public void s_ShowKey()
    {
        KeyInScene.gameObject.SetActive(true);
    }
    [PunRPC]
    void RPC_ShowKey()
    {
        KeyInScene.gameObject.SetActive(true);
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