using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class BidgePuzzle_Lever : MonoBehaviour
{
    [Header("Spawn Piece")]
    public GameObject missingBridePiece;

    [Header("Spawn Points")]
    public GameObject SpawnPoint;
    bool SpawnedPiece;

    [Header("UI")]
    public GameObject InteractionUI;
    public GameObject PuzzleGuide;
    public Image clueImageContainer;
    bool PuzzleGuideShowing, withinLeverZone;


    [Header("PlayerData")]
   // public PlayerSO playerData;

    [Header("Player Required for Puzzle Hint")]
    public bool GhostPLayer;
    public bool HumanPlayer;

    [Header("Puzzle Hints")]
    public GroundItem[] puzzleClues;
    NetworkedPlayerController controller;

    [Header("Missing Item")]
    [HideInInspector] public GroundItem missingItem;
    public BridgePuzzle_CheckForPiece bridgePuzzleChecker;

    [Header("Photon Settings")]
    PhotonView PV;
    public GameObject[] puzzlePiecesForSpawn;
    public GameObject[] puzzlePieceSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        SpawnedPiece = false;
        InteractionUI.gameObject.SetActive(false);
        PuzzleGuide.gameObject.SetActive(false);
        PuzzleGuideShowing = false;

    
        
        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            RandomizePuzzlePiece();
        }

        if (SceneSettings.Instance.isMultiPlayer == true)
        {

        }
  

    }

    // Update is called once per frame
    void Update()
    {
      
      

    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                //Photon
                PV = other.gameObject.GetComponent<PhotonView>();

                if (PV.IsMine)
                {
                    print("PV is mine");
                    controller = other.gameObject.GetComponent<NetworkedPlayerController>();
                    InteractionUI.gameObject.SetActive(true);
                }


            }


            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                controller = other.gameObject.GetComponent<NetworkedPlayerController>();
                InteractionUI.gameObject.SetActive(true);
            }




            }








    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //  InteractionUI.gameObject.SetActive(true);
            //Open UI
            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                withinLeverZone = true;
            }

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                if(PV.IsMine)
                {
                    withinLeverZone = true;
                }
           
            }


            if (controller != null)
            {
                if (SceneSettings.Instance.isSinglePlayer == true)
                {
                if (controller.PermormingAction == true)
                {
                    print("Controller perfmorning action");
           
                        //SEND CALL FOR ACTION - suvscription 
                        //Spawn Bridge Piece
            
                        if (SceneSettings.Instance.isMultiPlayer == true)
                        {
                            //Removed as puzzle pieces are already available. 
                            // PhotonNetwork.Instantiate(missingBridePiece.name, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                        }
                    
                }
                }

                if (SceneSettings.Instance.isMultiPlayer == true)
                {
                    if (PV.IsMine)
                    {
                        if (controller.PermormingAction == true)
                        {
                            print("Controller perfmorning action");
                           
                                int index;
                                index = Random.Range(0, puzzleClues.Length);
                                
                                PhotonView photonView = PhotonView.Get(this);
                                photonView.RPC("RandomizePuzzlePieces", RpcTarget.All, index);

                                //SEND CALL FOR ACTION - suvscription 
                                //Spawn Bridge Piece
                           
                            
                            
                        }
                    }

                }


            
            }

            if (controller == null)
            {
                if (SceneSettings.Instance.isMultiPlayer == true)
                {
                    //Photon
                    PV = other.gameObject.GetComponent<PhotonView>();

                    if (PV.IsMine)
                    {
                        print("Controller was null so I went and got it");
                        controller = other.gameObject.GetComponent<NetworkedPlayerController>();

                    }


                }


                if (SceneSettings.Instance.isSinglePlayer == true)
                {
                    controller = other.gameObject.GetComponent<NetworkedPlayerController>();
                    InteractionUI.gameObject.SetActive(true);
                }

            }

        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InteractionUI.gameObject.SetActive(false);  //Close UI
            PuzzleGuide.gameObject.SetActive(false); // close puzzle UI
            withinLeverZone = false;
        }
    }

    public void InteractionWithPuzzle()
    {

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
        if (withinLeverZone == true)
        {
            Debug.Log("myEvent received!");
            print("Check for UI");



            //event ending here


            if (SceneSettings.Instance.DebugMode == true)
            {
                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1)
                {
                    HumanPlayer = true;
                }

                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2)
                {
                    GhostPLayer = true;
                }
            }

            if (SceneSettings.Instance.isMultiPlayer == true)
            {

                if (PV.IsMine)
                {
                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1 && HumanPlayer == true && PuzzleGuideShowing == false)
                    {
                        print("Performing human Action");
                        PuzzleGuide.gameObject.SetActive(true);
                        PuzzleGuideShowing = true;
                        // performingLeverAction = false;
                        return;
                    }

                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 && GhostPLayer == true && PuzzleGuideShowing == false)
                    {
                        print("Performing ghost Action");
                        PuzzleGuide.gameObject.SetActive(true);
                        PuzzleGuideShowing = true;
                        //  performingLeverAction = false;
                        return;
                    }

                    if (PuzzleGuideShowing == true)
                    {
                        PuzzleGuide.gameObject.SetActive(false);
                        PuzzleGuideShowing = false;
                        //  performingLeverAction = false;
                        return;
                    }
                }
            }

            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1 && HumanPlayer == true && PuzzleGuideShowing == false)
                {
                    print("Performing human Action");
                    PuzzleGuide.gameObject.SetActive(true);
                    PuzzleGuideShowing = true;
                    // performingLeverAction = false;
                    return;
                }

                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 && GhostPLayer == true && PuzzleGuideShowing == false)
                {
                    print("Performing ghost Action");
                    PuzzleGuide.gameObject.SetActive(true);
                    PuzzleGuideShowing = true;
                    //  performingLeverAction = false;
                    return;
                }

                if (PuzzleGuideShowing == true)
                {
                    PuzzleGuide.gameObject.SetActive(false);
                    PuzzleGuideShowing = false;
                    //  performingLeverAction = false;
                    return;
                }
            }

        }
    }





    public void RandomizePuzzlePiece()
    {

        int index;
        index = Random.Range(0, puzzleClues.Length);
        missingItem = puzzleClues[index];

        bridgePuzzleChecker.missingItem = missingItem;

        clueImageContainer.sprite = missingItem.item.uiDisplay;
    }


    [PunRPC]
    void RandomizePuzzlePieces(int randomNum)
    {
        missingItem = puzzleClues[randomNum];

         bridgePuzzleChecker.missingItem = missingItem;

        clueImageContainer.sprite = missingItem.item.uiDisplay;
    }


    




}
