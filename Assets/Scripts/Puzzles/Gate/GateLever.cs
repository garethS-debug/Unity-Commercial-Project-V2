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
    bool PuzzleGuideShowing, withinLeverZone;


    [Header("Player Required for Puzzle Hint")]
    public bool GhostPLayer;
    public bool HumanPlayer;

    [Header("Puzzle Hints")]
    public GameObject[] puzzleTriggers;
    NetworkedPlayerController controller;

    [Header("PuzzleState")]
    public PuzzleState state;

    [Header("Activation")]
    [HideInInspector] public PuzzleInfo activationSwitch;
    public BridgePuzzle_CheckForPiece bridgePuzzleChecker;

    [Header("Photon Settings")]
    PhotonView PV;


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

        InteractionUI.gameObject.SetActive(false);
        PuzzleGuide.gameObject.SetActive(false);
        PuzzleGuideShowing = false;
        RandomizePuzzlePiece();



        if (SceneSettings.Instance.isSinglePlayer == true)
        {

        }

        if (SceneSettings.Instance.isMultiPlayer == true)
        {

        }

        if (SceneSettings.Instance.DebugMode == true)
        {
            brokenGatePiece.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }


    }

    public void Update()
    {
        if (state == PuzzleState.Active)
        {
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
                            print("State is now : " + state);

                        }
                    }
                }

                if (SceneSettings.Instance.isMultiPlayer == true)
                {
                    if (PV.IsMine)
                    {
                        if (controller.PermormingAction == true)
                        {
        
                            if (state == PuzzleState.Inactive)
                            {
                                //SEND CALL FOR ACTION - suvscription 
                                //Spawn Bridge Piece
                                state = PuzzleState.Active;
                                print("State is now : " + state);


                            }
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
            GatePuzzleObjectTrigger trigger = puzzleTriggers[i].gameObject.GetComponent<GatePuzzleObjectTrigger>();

            if (trigger.state == GatePuzzleObjectTrigger.PuzzleObjectState.Active)
            {
                print("Puzzle Piece Active");
                if (trigger.objectInfo == activationSwitch)
                {
                    print("Correct Piece Active");
                }

                if (trigger.objectInfo != activationSwitch)
                {
                    print("Incorrect Piece Active");
                    IncorrectSelection();
                    state = PuzzleState.Inactive;
                    return;
                }
            }

        }
    }



    public void RandomizePuzzlePiece()
    {

        int index;
        index = Random.Range(0, puzzleTriggers.Length);
        activationSwitch = puzzleTriggers[index].GetComponent<PuzzleInfo>(); ;

        bridgePuzzleChecker.missingItem = activationSwitch;

        clueImageContainer.sprite = activationSwitch.puzzleInfo.uiImage;
    }


    public void IncorrectSelection()
    {
        RandomizePuzzlePiece();
    }

}
