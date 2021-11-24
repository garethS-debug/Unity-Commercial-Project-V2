using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class GatePuzzleObjectTrigger : MonoBehaviour
{

    bool isActive;

    [Header("PuzzleState")]
    public PuzzleObjectState state;

    [Header("PuzzleInformationContainer")]
    public PuzzleItemInfo objectInfo;

    [Header("Bool")]
    bool PuzzleGuideShowing, withinLeverZone;

    [Header("Photon Settings")]
    PhotonView PV;
    NetworkedPlayerController controller;

    public void Start()
    {
        state = PuzzleObjectState.Inactive;
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
                   // InteractionUI.gameObject.SetActive(true);
                }


            }

            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                controller = other.gameObject.GetComponent<NetworkedPlayerController>();
               // InteractionUI.gameObject.SetActive(true);
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
            print("next to rune object");


            if (SceneSettings.Instance.isMultiPlayer == true)
            {

                if (PV.IsMine)
                {
                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1)
                    {
                        state = PuzzleObjectState.Active;
                        print("Performing human Action");
                        return;
                    }

                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 )
                    {
                        state = PuzzleObjectState.Active;
                        print("Performing ghost Action");
                        return;
                    }

                }
            }

            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1)
                {
                    state = PuzzleObjectState.Active;
                    print("Performing human Action");
                    return;
                }

                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 2 )
                {
                    state = PuzzleObjectState.Active;
                    print("Performing ghost Action");
                    return;
                }
            }

        }
    }




    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          //  InteractionUI.gameObject.SetActive(false);  //Close UI
          //  PuzzleGuide.gameObject.SetActive(false); // close puzzle UI
            withinLeverZone = false;
        }
    }




    public enum PuzzleObjectState
    {
        Active,
        Inactive,


    }






}
