using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class GateLever : MonoBehaviour
{


    [Header("Spawn Piece")]
    public GameObject fixedGatePiece;
    public GameObject brokenGatePiece;

    [Header("UI")]
    public GameObject InteractionUI;
    public GameObject PuzzleGuide;
    public Image clueImageContainer;
    public Image Objective;
    public Sprite GateObjectivePictureDescription;
    bool PuzzleGuideShowing, withinLeverZone;


    [Header("Player Required for Puzzle Hint")]
    public bool GhostPLayer;
    public bool HumanPlayer;

    [Header("Puzzle Barrier")]
    public GameObject humanBarrier;
    public GameObject ghostBarrier;

    [Header("Puzzle Hints")]
    public GameObject[] puzzleTriggers;
    NetworkedPlayerController controller;

    [Header("PuzzleState")]
    public PuzzleState state;

    [Header("Activation")]
    public PuzzleItemInfo activationSwitch;
    public GatePuzzleObjectTrigger gatePuzzleChecker;
    [HideInInspector] public bool correctPieceSelected;

    [Header("Rune")]
    public string runeDescription;

    [Header("Photon Settings")]
    PhotonView PV;
    GatePuzzleObjectTrigger trigger;

    /// <summary>
    /// 
    /// 
    /// When the player 'activates' the puzzle, the runes on the stones will appear. Then the other player can choose one. An incorrect selection resets the puzzle. 
    /// 
    /// 
    /// </summary>




    // Start is called before the first frame update
    void Start()
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            //Photon
            PV = GetComponent<PhotonView>();
        }
        InteractionUI.gameObject.SetActive(false);
        PuzzleGuide.gameObject.SetActive(false);
        PuzzleGuideShowing = false;

        if (SceneSettings.Instance.isSinglePlayer)
        {
            RandomizePuzzlePiece();
        }




        if (SceneSettings.Instance.DebugMode == true)
        {
            print("Debug Mode");
            brokenGatePiece.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }

        state = PuzzleState.Inactive;


        if (GhostPLayer)
        {
            humanBarrier.gameObject.SetActive(true);
            ghostBarrier.gameObject.SetActive(false);
        }

        if (HumanPlayer)
        {
            humanBarrier.gameObject.SetActive(false);
            ghostBarrier.gameObject.SetActive(true);
        }
    }

    public void Update()
    {
        if (state == PuzzleState.Active)
        {
            print("Checking for trigger");
            checkForCorrectTrigger();
        }


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
          
            //Open UI
            if (SceneSettings.Instance.isSinglePlayer == true)
            {
            
                withinLeverZone = true;
            }

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                if (PV.IsMine)
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
                 

                        if (state == PuzzleState.Inactive)
                        {
                            //SEND CALL FOR ACTION - suvscription 
                            //Spawn Bridge Piece
                            state = PuzzleState.Active;
                            Objective.sprite = GateObjectivePictureDescription;

                        }
                    }
                }

                if (SceneSettings.Instance.isMultiPlayer == true)
                {
                    if (PV.IsMine)
                    {
                        if (controller.PermormingAction == true)
                        {
                            PhotonView photonView = PhotonView.Get(this);
                            photonView.RPC("RPC_ActivatePuzzleStatus", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                   
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




    public enum PuzzleState
    {
        Active,
        Inactive,

        Default
    }






    public void checkForCorrectTrigger()
    {
        for (int i = 0; i < puzzleTriggers.Length; i++)
        {
             trigger = puzzleTriggers[i].gameObject.GetComponent<GatePuzzleObjectTrigger>();

            if (trigger.state == GatePuzzleObjectTrigger.PuzzleObjectState.Active)
            {
                print("Puzzle Piece Active");
                if (trigger.objectInfo == activationSwitch)
                {
                    //WIN METHOD

                    if (SceneSettings.Instance.isMultiPlayer == true)
                    {

                        PhotonView photonView = PhotonView.Get(this);
                        photonView.RPC("RPC_PropChangeModel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
                    }

                    if (SceneSettings.Instance.isSinglePlayer == true)
                    {
                        print("Correct Piece Active");
                        trigger.CorrectRune();
                        state = PuzzleState.Inactive;
                        correctPieceSelected = true;
                       
                        FixBridge();
                    }
                    return;


                }

                if (trigger.objectInfo != activationSwitch)
                {
                    //LOOSE METHOD
              


                    if (SceneSettings.Instance.isMultiPlayer == true)
                    {
                        PhotonView photonView = PhotonView.Get(this);
                        photonView.RPC("RPC_IncorrectPuzzlePiece", RpcTarget.All ); // reset puzzle
                        int index;
                        index = Random.Range(0, puzzleTriggers.Length);

                        photonView.RPC("RPC_SelectPuzzlePiece", RpcTarget.All, index); // choose new piece
                    }


                    if (SceneSettings.Instance.isSinglePlayer == true)
                    {
                        print("Incorrect Piece Active");
                        trigger.incorrectRune();
                        IncorrectSelection(); //Loose Method
                        state = PuzzleState.Inactive;
                    }


         
                    return;
                }
            }

        }
    }


    







    public void FixBridge()
    {
        brokenGatePiece.GetComponent<MeshFilter>().sharedMesh = fixedGatePiece.GetComponent<MeshFilter>().sharedMesh;
        humanBarrier.gameObject.SetActive(false);
        ghostBarrier.gameObject.SetActive(false);
        //  missingPieceBoxCollder.SetActive(true);
    }



    public void RandomizePuzzlePiece()
    {

        //if (SceneSettings.Instance.isMultiPlayer == true)
        //{
        //    PhotonView photonView = PhotonView.Get(this);
        //    photonView.RPC("RPC_SelectPuzzlePiece", RpcTarget.All );
        //}

        //Randomize _SP
        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            int index;
            index = Random.Range(0, puzzleTriggers.Length);
            activationSwitch = puzzleTriggers[index].GetComponent<GatePuzzleObjectTrigger>().objectInfo; ;
            print("randomized piece = " + activationSwitch.objectName);

            gatePuzzleChecker.objectInfo = activationSwitch;
            clueImageContainer.sprite = activationSwitch.uiImage;
            //  rune.gameObject.GetComponent<MeshFilter>().sharedMesh = activationSwitch.rune.gameObject.GetComponent<MeshFilter>().sharedMesh;
        }

     



    }


    public void IncorrectSelection()
    {
        RandomizePuzzlePiece(); // Choose a new puzzle piece
        //set state to inactive
        for (int i = 0; i < puzzleTriggers.Length; i++)
        {
            GatePuzzleObjectTrigger trigger = puzzleTriggers[i].gameObject.GetComponent<GatePuzzleObjectTrigger>();
            trigger.state = GatePuzzleObjectTrigger.PuzzleObjectState.Inactive;
            trigger.incorrectRune();
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
        if (withinLeverZone == true)
        {
            Debug.Log("myEvent received!");
            print("Check for UI");



            //event ending here


            if (SceneSettings.Instance.DebugMode == true)
            {
                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1)
                {
                    print("SettingBoolBasedon Debug human");
                    HumanPlayer = true;
                }

                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2)
                {
                    print("SettingBoolBasedon Debug ghost");
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
                        PhotonView photonView = PhotonView.Get(this);

                        int index;
                        index = Random.Range(0, puzzleTriggers.Length);

                        
                        ///?FUUUUUUUUUUUUUU
                        photonView.RPC("ChatMessage", RpcTarget.All, index);




                        photonView.RPC("RPC_SelectPuzzlePiece", RpcTarget.All, index);
                        return;
                    }

                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 && GhostPLayer == true && PuzzleGuideShowing == false)
                    {
                        print("Performing ghost Action");
                        PuzzleGuide.gameObject.SetActive(true);
                        PuzzleGuideShowing = true;

                        int index;
                        index = Random.Range(0, puzzleTriggers.Length);


                        PhotonView photonView = PhotonView.Get(this);
                        //   string intTest = int.Parse(index);

                        ///?FUUUUUUUUUUUUUU
                        photonView.RPC("ChatMessage", RpcTarget.All, index);




                        photonView.RPC("RPC_SelectPuzzlePiece", RpcTarget.All, index);
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



    [PunRPC]
    void RPC_PropChangeModel()
    {
        print("Correct Piece Active");
        trigger.CorrectRune();
        state = PuzzleState.Inactive;
        correctPieceSelected = true;

        brokenGatePiece.GetComponent<MeshFilter>().sharedMesh = fixedGatePiece.GetComponent<MeshFilter>().sharedMesh;
        //  Debug.Log("2 golden keys");
        //  missingPieceBoxCollder.SetActive(true);
        humanBarrier.gameObject.SetActive(false);
        ghostBarrier.gameObject.SetActive(false);
    }


    [PunRPC]
    void ChatMessage(int a)
    {
        int index = a;
        print("FUUUUUUUUUUU " + a);

       // Debug.Log(string.Format("ChatMessage {0} {1}", a, b));
    }






    [PunRPC]
    void RPC_SelectPuzzlePiece(int randomNO)
    {

      
        activationSwitch = puzzleTriggers[randomNO].GetComponent<GatePuzzleObjectTrigger>().objectInfo; ;
        print("randomized piece = " + activationSwitch.objectName);

        gatePuzzleChecker.objectInfo = activationSwitch;
        clueImageContainer.sprite = activationSwitch.uiImage;
    }


    [PunRPC]
    void RPC_ActivatePuzzleStatus()
    {
        if (state == PuzzleState.Inactive)
        {
            //SEND CALL FOR ACTION - suvscription 
            //Spawn Bridge Piece
            state = PuzzleState.Active;
            Objective.sprite = GateObjectivePictureDescription;
            print("State is now : " + state);


        }


    }


    [PunRPC]
    void RPC_IncorrectPuzzlePiece()
    {

        //LOOSE METHOD
        print("Incorrect Piece Active");
        trigger.incorrectRune();
        IncorrectSelection(); //Loose Method
        state = PuzzleState.Inactive;
        return;

    }
}
