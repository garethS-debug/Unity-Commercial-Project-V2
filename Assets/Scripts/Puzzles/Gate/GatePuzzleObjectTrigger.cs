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

    [Header("Material Settings")]
    public Material activeRune;
    public Material correctRune;
    public Material wrongRune;
    public Material originalMaterial;
    //public GameObject rune;






    public void Start()
    {
        state = PuzzleObjectState.Inactive;

    
        //  rune.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("Intensity", 1);
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
                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 0)
                    {
                        state = PuzzleObjectState.Active;

                        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                       if (GetComponent<MeshRenderer>()!= null)
                        {
                            GetComponent<MeshRenderer>().material = activeRune;
                        }
                    

                        print("Performing human Action");
                        return;
                    }

                    if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1 )
                    {
                        state = PuzzleObjectState.Active;
                        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                        GetComponent<MeshRenderer>().material = activeRune;
                        print("Performing ghost Action");
                        return;
                    }

                }
            }

            if (SceneSettings.Instance.isSinglePlayer == true)
            {
                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 0)
                {
                    state = PuzzleObjectState.Active;
                    MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                    GetComponent<MeshRenderer>().material = activeRune;
                    print("Performing human Action");
                    return;
                }

                if (SceneSettings.Instance.playerSOData.PlayerCharacterChoise == 1 )
                {
                    state = PuzzleObjectState.Active;
                    MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                    GetComponent<MeshRenderer>().material = activeRune;
                    print("Performing ghost Action");
                    return;
                }
            }

        }
    }


 



    IEnumerator RevertToExistingMat()
    {
      
        yield return new WaitForSeconds(3f);
        // MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material = originalMaterial;
        print("I hate coroutines");

    }


    public void incorrectRune()
    {
        GetComponent<MeshRenderer>().material = wrongRune;
        StartCoroutine("RevertToExistingMat");
    }



    public void CorrectRune()
    {
        GetComponent<MeshRenderer>().material = correctRune;
        StartCoroutine("RevertToExistingMat");
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
