using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GatePuzzleCheckForPiece : MonoBehaviour
{

    // Start is called before the first frame update
    [Header("Missing Item")]
    [HideInInspector] public PuzzleInfo activationPiece;

    // [Header("Lever")]
    //public BidgePuzzle_Lever lever;

    [Header("new Mesh")]
    public Mesh openMesh;
    public GameObject newBoxCollider;

    [Header("Broken Bridge")]
    public GameObject blockedMesh;
    //public GameObject brokenBridgeMesh;


    [Header("Photon Settings")]
    PhotonView PV;                                              //Setting up photon view 





    // Start is called before the first frame update
    void Start()
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            PV = this.gameObject.GetComponent<PhotonView>();        //Get the photonview on the player
        }
    }





    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

           
               //If Correct activation -- Check

                    if (SceneSettings.Instance.isMultiPlayer == true)
                    {
                        PV.RPC("RPC_PropChangeModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                    }

                    if (SceneSettings.Instance.isSinglePlayer == true)
                    {
                        FixBridge();
                    }
                }

                else
                {
                    Debug.Log("No Luck");
                    Debug.Log("----------");
 
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

        blockedMesh.GetComponent<MeshFilter>().mesh = openMesh;
        Debug.Log("Correct Rune Selected");
       // missingPieceBoxCollder.SetActive(true);
    }


    public void FixBridge()
    {
        blockedMesh.GetComponent<MeshFilter>().mesh = openMesh;
        Debug.Log("Correct Rune Selected");
        // missingPieceBoxCollder.SetActive(true);
    }

}


    

