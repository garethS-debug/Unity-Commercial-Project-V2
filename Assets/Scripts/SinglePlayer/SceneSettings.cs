using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SceneSettings : MonoBehaviour
{
    private static SceneSettings _instance;

    public static SceneSettings Instance { get { return _instance; } }

    [Header("PlayerData")]
    public PlayerSO playerSOData;

    [Header("Game Mode ")]
    public bool isSinglePlayer;

    public bool isMultiPlayer;


    [Header("my player ")]
    //  PhotonView PV;
  //  private GameObject[] players;
   // public GameObject myPlayer;

    public bool DebugMode;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;


            if (playerSOData.SingleOrMultiPlayer == 1)
            {
                // 1 = multiplayer
                //2 = single player
             isMultiPlayer = true;
                isSinglePlayer = false;

            }

            else if (playerSOData.SingleOrMultiPlayer == 2)
            {
              isSinglePlayer = true;
                isMultiPlayer = false;
            }



            if (isSinglePlayer == false && isMultiPlayer == false)
            {
                Debug.LogError("SceneSettings : Set bool to either single or multiplayer");
            }
        }
    }


    //public void Start()
    //{
    //    if (isMultiPlayer == true)
    //    {
    //        players = GameObject.FindGameObjectsWithTag("Player");

    //        for (int i = 0; i < players.Length; i++)
    //        {
    //            // Clear the player's inventory when they quit
    //            if (players[i].gameObject.GetComponent<PhotonView>().IsMine)
    //            {
    //                myPlayer = players[i];
    //                print("My Player is " + myPlayer.name);
    //            }

    //            else
    //            {
    //                Debug.Log("Player found but not mine " + players[i].gameObject.name);
    //            }

    //        }
    //    }
     
    //}


    public void RemoveMultiplayerScript(GameObject subject)
    {
        if (subject.gameObject.GetComponent<PhotonTransformView>() == true)
        {
            Destroy(subject.gameObject.GetComponent<PhotonTransformView>());
        }

        if (subject.gameObject.GetComponent<PhotonView>() == true)
        {
            Destroy(subject.gameObject.GetComponent<PhotonView>());
        }
    }


    private void OnApplicationQuit()
    {

        //for (int i = 0; i < players.Length; i++)
        //{
        //    // Clear the player's inventory when they quit
        //    players[i].gameObject.GetComponent<PlayerInventory>().inventory.Container.Clear();
        //}


    }

}

