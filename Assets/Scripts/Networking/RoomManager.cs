using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager Instance;

    public GameObject networkedPlayerManager;

    public GriefBarDisplay griefBar;


    public bool SinglePLayerMode;


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

        if (SinglePLayerMode == false)
        {
            networkedPlayerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
      
     //   networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false;
      //  }
    }

    public void CreatePlayer()
    {
   
        print("Creating Player : ".Color("Green"));
         networkedPlayerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        // networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false

        //griefBar.CreateDisplay();

    }


}


