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

    [Header("Intro")]

   // public GameObject LevelIntro;


    [Header("LoadingBetweenScenes")]
    private PhotonView PhotonView;
    private int PlayersInGame = 0;

    private void Awake()
    {
        if (Instance)                                   //If room manager already in scene
        {
            Destroy(gameObject);                        //If yes destroy
            print("Destroying this copy");
            return;

        }

        else
        {
            Instance = this;
            print("I am the only 1");

        }
       
        DontDestroyOnLoad(gameObject);                      //I am the only 1


        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            PhotonView = GetComponent<PhotonView>();
        }



    }


    public void Start()
    {

        DontDestroyOnLoad(gameObject);                      //I am the only 1
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

        //start evtry scene ---- > removed 07/12/2021
        //LevelIntro.gameObject.GetComponent<Level01Cutscene>().StartCoroutine("CutSceneCoRoutine");
        //then spawn in players

         //Added 13/12/2021
         /*
    if (scene.name != "Lobby")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MasterLoadedGame();
            }
            else
            {
                NonMasterLoadedGame();
            }
        }

     */  

        //-----ERROR created when switching scenes moved on 13/12/2021

    if (PhotonNetwork.IsConnected)
        {
            spawnPlayers();
        }

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Not Connected ".Color("red"));
        }


        if (PhotonNetwork.CurrentRoom == null ) 
        {
            Debug.Log("Not Connected to room".Color("red"));
        }

        if (PhotonNetwork.CurrentRoom.Players == null)
        {
            Debug.Log("no current players".Color("red"));
        }
    }

    //running 3 times
    public void spawnPlayers()
    {


        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            //running 3 times
            networkedPlayerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            Debug.Log("Game Is Multiplayer : ".Bold().Color("green"));
            return;
        }

        else
        {
            Debug.Log("Game Is Single Player : ".Bold().Color("white"));
            s_CreatePlayer();
            return;
        }


     //   LevelIntro.gameObject.SetActive(false);


    }



    public void s_CreatePlayer()
    {
   
        print("Creating Player : ".Color("Green"));
        S_PlayerInstance = Instantiate(s_PlayerManager, Vector3.zero, Quaternion.identity);
        // networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false

        //griefBar.CreateDisplay();

    }


    private void MasterLoadedGame()
    {
        print("masterLoaded-Game");
        PlayersInGame = 1;
        PhotonView.RPC("RPC_LoadGameOthers", RpcTarget.Others);
              //  photonView.RPC("RandomizePuzzlePieces", RpcTarget.All, index);
    }
 
    private void NonMasterLoadedGame()
    {
        print("non Master Loaded-Game");
        PhotonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        print("Loading Level from - Level to Load");
        PhotonNetwork.LoadLevel(EndOfLevel.Instance.levelToLoad);
    }



    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        PlayersInGame++;
        print("Players in the scene : " + PlayersInGame);

        if (PlayersInGame == PhotonNetwork.PlayerList.Length) //in number of players in game is the same as the player list 
        {
            print("All players are in the game scene");
        }

    }

}


