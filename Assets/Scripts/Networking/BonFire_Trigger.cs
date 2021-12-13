using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFire_Trigger : MonoBehaviour
{

    public LobbyManager lobbyManager;

    public Camera BookCamTrigger;


    private void Start()
    {
        BookCamTrigger.gameObject.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
         //   other.gameObject.GetComponent<NetworkedPlayerController>().

            BookCamTrigger.gameObject.SetActive(true);
            lobbyManager.bookCamera = BookCamTrigger;
           // lobbyManager.exitUI.gameObject.GetComponent<Animator>().SetBool("UIAppear", true);
            StartCoroutine(CutSceneCoRoutine());
            //Open UI
            //------ Characcter ID check
            /*
            CharacterID charID = other.gameObject.GetComponent<CharacterID>();

            if (charID != null)
            {
           
                if (charID.isHumanCharater == true || charID.isGhostCharacer == true)
                {
             */
       
              //  }
              //  else
              //  {
                  //  lobbyManager.lobbyUI.gameObject.SetActive(false);
               // }
            }
  
       // }

    }


    IEnumerator CutSceneCoRoutine()
    {


        yield return new WaitForSeconds(3.5f);

        lobbyManager.lobbyUI.gameObject.SetActive(true);

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Close UI
           // lobbyManager.lobbyUI.gameObject.SetActive(false);
        }
    }


}
