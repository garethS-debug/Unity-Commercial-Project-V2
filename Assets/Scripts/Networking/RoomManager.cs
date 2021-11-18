using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Multi PLayer Instnace")]
    public static RoomManager Instance;
    public GameObject networkedPlayerManager;



    [Header("Single PLayer Instnace")]
  [HideInInspector]  public GameObject S_PlayerInstance;
    public GameObject s_PlayerManager;



    [Header("Grief Bar")]
    public GriefBarDisplay griefBar;



    private void Awake()
    {
        if (Instance)                                   //If room manager already in scene
        {
            Destroy(gameObject);                        //If yes destroy
            return;
        }

        DontDestroyOnLoad(gameObject);                      //I am the only 1


        Instance = this;
    }


    public void Start()
    {


        //   CreatePlayer();                                   //Works but not on Claire's version


        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            SceneSettings.Instance.RemoveMultiplayerScript(this.gameObject);
        }

   



    }


    private void OnEnable()
    {
        Debug.Log("OnEnabled Called");
       // base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;      //Works on Claire's but not on 

    }



    private void OnDisable()
    {
        Debug.Log("OnDisabledd Called");
        //  base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode LoadSceneMode)
    {
        // if (scene.buildIndex == 1) // if we are in the game scene
        // {

           if (SceneSettings.Instance.isMultiPlayer == true)
        {
            networkedPlayerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            Debug.Log("Game Is Multiplayer : ".Bold().Color("green"));
        }

           else
        {
            Debug.Log("Game Is Single Player : ".Bold().Color("white"));
            s_CreatePlayer();
        }

        


      
     //   networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false;
      //  }
    }



    public void s_CreatePlayer()
    {
   
        print("Creating Player : ".Color("Green"));
        S_PlayerInstance = Instantiate(s_PlayerManager, Vector3.zero, Quaternion.identity);
        // networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false

        //griefBar.CreateDisplay();

    }


}


