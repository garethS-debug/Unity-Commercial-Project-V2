using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;
using ExitGames.Client.Photon;
using System.Linq;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("Room")]
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;

    [Header("MaxPlayerCount")]
    public TMP_Text MaxPlayerUI;
    public int maxPlayerCount = 1;

    [Header("UI Room Button - in Lobby")]
    public RoomItem RoomItemPrefab; //Storing roomitem prefeb
    List<RoomItem> roomItems = new List<RoomItem>();   //List of room items

    [Header("TIME BETWEEN UPDATES")]
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;
   

    public Transform contentObject; //Scrool view parent


    [Header("Player Item Cards")]
    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefeb;
    public Transform playerItemParent;

    [Header("Button")]
    public GameObject playButton;

    [Header("SceneSelect")]
    public GameObject levelSelectPanel;


    [Header("Room Info")]
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, RoomItem> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;
    private Dictionary<int, GameObject> bonfireEntries;

    [Header("KickPlayer")]
    [SerializeField]
    private TMP_InputField nicknameInputField;
    private const byte KICK_PLAYER_EVENT = 0;

    [Header("Password")]
    public GameObject passwordBox;
    [SerializeField]
    Toggle public_Toggle;
    [SerializeField]
    Toggle private_Toggle;
    [SerializeField]
    private TMP_InputField passwordField;

    ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();

    [Header("Bonfires")]
    public GameObject bonfireTrigger;
   // public GameObject s_bonfireTrigger;
  //  public GameObject s_bonfirePrefab;
  //  public GameObject s_bonfirePrefab;
    public GameObject Lobby_PLayer;
    public GameObject[] Bonfire_GameObjects;
    int Bonfire_Index, Bonfire_SpawnIndex;
    public GameObject[] Lobby_Room_Bonfire_SpawnPoints;
    List<GameObject> bonfiresItems = new List<GameObject>();   //List of room items
    public GameObject tempBonfire;





    [Header("Player Avatars")]
    public GameObject[] playerPrefabs;         //stored player prefabs
    public ExitGames.Client.Photon.Hashtable playerproperties = new ExitGames.Client.Photon.Hashtable();   //Custom property - Hashtable (a list with a name instead of #)
    public PlayerSO playerSOData;
    public GameObject[] Lobby_Room_Player_SpawnPoints;
    public GameObject spawnedLobbyPlayer;
    List<int> characterSelectedID = new List<int>();   //List of charactersSelected
    bool uniqueList;

    [Header("Debugging")]
    public TMP_Text regionTextbox;

    [Header("UI")]
    public GameObject lobbyUI;
    [HideInInspector] public Camera bookCamera;
    public GameObject ghosthumanWarning;
   // public GameObject exitUI;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, RoomItem>();
        //Bonfire
        bonfireEntries = new Dictionary<int, GameObject>();

        
    }

 

    //When lobby scene loads up we need a lobby to create a room
    public void Start()
    {

       if( SceneSettings.Instance.isMultiPlayer == true)
        {
            bonfireTrigger.SetActive(true);
        lobbyPanel.SetActive(false);
        playerproperties["playerAvatar"] = 0; //ensures it exists

        if (playerSOData.AutoConnect == true)
        {
            playerproperties["playerAvatar"] = playerSOData.PlayerCharacterChoise;
            //print("PLayer ID = " + (int)playerproperties["playerAvatar"]);
        }
            ghosthumanWarning.gameObject.SetActive(false);
            roomPanel.SetActive(false); //set to false in case

        // if (!PhotonNetwork.IsConnected)
        // {
        PhotonNetwork.JoinLobby();
        // }
        maxPlayerCount = 1;

        DebuggingFunction();
        }


        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            Destroy(bonfireTrigger);

         //   Instantiate(s_bonfirePrefab, s_bonfireTrigger.transform.position, s_bonfireTrigger.transform.rotation);

        }



        }

    
    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)                                    //If there is not a blank room name
        {


 
            //--------------------------3D Lobby Settings
            /*
            Bonfire_SpawnIndex = UnityEngine.Random.Range(0, Lobby_Room_Bonfire_SpawnPoints.Length);      //Choosing a Random Spawn point
            Bonfire_Index = UnityEngine.Random.Range(0, Bonfire_GameObjects.Length);              //Choosing a Random Fireplace
            GameObject BonfirePosition = Lobby_Room_Bonfire_SpawnPoints[Bonfire_SpawnIndex];
            spawnedLobbyPlayer.transform.position = BonfirePosition.transform.position;
            GameObject bonfireSpawn =   spawnedLobbyPlayer.gameObject.GetComponent<NetworkedPlayerController>().bonfireSpawn;
            tempBonfire =  Instantiate(Bonfire_GameObjects[Bonfire_Index], bonfireSpawn.transform.position, Quaternion.identity);
            RoomItem bonfireRoomitem = tempBonfire.gameObject.GetComponent<RoomItem>();
            bonfireRoomitem.roomButton.SetActive(false);
            bonfireRoomitem.bonfireGhost.SetActive(false);
            bonfireRoomitem.isHostVersion = true;
            */


           // Access players save date


            //Max player count is hardcoded at 2 instead of maxPlayerCount
            CreateRoom(roomInputField.text, /*maxPlayerCount*/ 2, /*Password*/passwordField.text/*, //3D Lobby -- Bonfire_SpawnIndex, Bonfire_Index, (int)playerproperties["playerAvatar"]*/);
            
            
 
        }




    }

    public void CreateRoom(string _name, int _maxplayers, string _password/*, //3D Lobby -- int _lobbySpawnIndex, int _bonfire, int _playerType */)
    {


        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = System.Convert.ToByte(_maxplayers), IsOpen = true, IsVisible = true };

        ExitGames.Client.Photon.Hashtable custProps = new ExitGames.Client.Photon.Hashtable();
        
        //pasword
        custProps.Add("Password", _password);
        
        //----------------------Bonfire 
        /*
        custProps.Add("BonfireSpawnPoint", _lobbySpawnIndex);
        custProps.Add("Bonfire", _bonfire);
        */
        //------------------- player type
        /*
        custProps.Add("playerType", _playerType);
        */

        roomOptions.CustomRoomProperties = custProps;


        //-------------------Set up custom room properties
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "Password", "BonfireSpawnPoint", "Bonfire", "playerType" };
      //  Debug.Log(" Room Option = new password ");



      

        PhotonNetwork.CreateRoom(_name, roomOptions, TypedLobby.Default);



    }




    public void IncreaseMaxPlayerCount()
    {
        maxPlayerCount++;
        MaxPlayerUI.text = maxPlayerCount.ToString();
    }

    public void DecreaseMaxPlayerCount()
    {
        if (maxPlayerCount > 1)
        {
            maxPlayerCount--;
            MaxPlayerUI.text = maxPlayerCount.ToString();
        }
    
    }

    /// <summary>
    /// from asteriod Demo
    /// </summary>
    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    //called when joining a room
    public override void OnJoinedRoom()
    {


        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        //INSERT LOGIC HERE FOR 3D Lobby
        foreach ( GameObject bonfire in bonfiresItems)
        {
           // Destroy(bonfire);
        }

        ClearRoomListView();

        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerList();
    }






    //Called automaticlaly when there is a change in room list
    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        //when a room is created, modified or destroyed
     //   print("OnRoomList Update");
        /*
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(_roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
        */

        ClearRoomListView();
        UpdateCachedRoomList(_roomList);
        UpdateRoomListView();




   
    





    }


    /// <summary>
    /// Logic for 3D Lobby
    /// </summary>
    /// 
    public void SetUp3DLobby()
    {



        //Host will be in the room 
        //Waiting players will be outside of the room. 
        //Setting up room
        Bonfire_Index = UnityEngine.Random.Range(0, Bonfire_GameObjects.Length);
        Bonfire_SpawnIndex = UnityEngine.Random.Range(0, Lobby_Room_Bonfire_SpawnPoints.Length);


        GameObject LobbyRoom = Bonfire_GameObjects[Bonfire_Index];
        GameObject LobbySpawnPoints = Lobby_Room_Bonfire_SpawnPoints[Bonfire_SpawnIndex];

        Instantiate(LobbyRoom, LobbySpawnPoints.transform.position, Quaternion.identity);



    }


    /// <summary>
    /// From Asteriods Demo
    /// </summary>
    private void ClearRoomListView()
    {
        foreach (RoomItem item in roomItems)
        {
            Destroy(item.gameObject);
        }

        foreach (GameObject bonefire in bonfiresItems)
        {
            Destroy(bonefire.gameObject);
        }


        roomItems.Clear();
        bonfiresItems.Clear();
    }



    /// <summary>
    /// From Asteriods Demo
    /// </summary>
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }


    /// <summary>
    /// From ASteriod demo
    /// </summary>
    /// <param name="_list"></param>

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            

            //---------3D Lobby Settings----------------
            //Bonfire Settings
            /*
            GameObject BonfireGO = Bonfire_GameObjects[(int)info.CustomProperties["Bonfire"]];                              //Getting the bonfire GO from the room information 
            GameObject BonfireSpawnPoint = Lobby_Room_Bonfire_SpawnPoints[(int)info.CustomProperties["BonfireSpawnPoint"]];         //Getting the bonfire spawn point from the room information 
            GameObject bonfireINS = Instantiate(BonfireGO, BonfireSpawnPoint.transform.position, Quaternion.identity);
            RoomItem newRoom = bonfireINS.gameObject.GetComponent<RoomItem>();                                              //Get Room Component of bonfire
            bonfiresItems.Add(bonfireINS);                                                                                  // Instantiate Bonfire            
            newRoom.BonfireGO = BonfireGO;
            newRoom.LobbySpawnPoint = BonfireSpawnPoint;
            */
            //---------------------------------------------



            //--------- Room Requirements------------------
            //Room ID required (opposite to joining player)
            //----HUman = 1
            //----Ghost = 2
            // --- When updating room it will spawn the opposite 
            /*
            if ((int)info.CustomProperties["playerType"] == 1)
            {
                print("Ghost Needed");
                newRoom.remainingPlayerID = 2;                  //opposite player required

            }
            else if ((int)info.CustomProperties["playerType"] == 2)
            {
                print("Human Needed");
                newRoom.remainingPlayerID = 1;   //opposite player required
            }
            */
            //---------------------------------------


            //Room Information
            /* -- remove when 3D lobby is in use */  RoomItem newRoom = Instantiate(RoomItemPrefab, contentObject);         //Instantiate the UI element
            newRoom.SetRoomName(info.Name);                                         //Set toom name in the item
            newRoom.roomInfo = info;
            roomItems.Add(newRoom);                                                 //AddComponentMenu new to list




            //----------------------------------Password
            Debug.Log(info.ToStringFull());
            Debug.Log("------");

            if (info.CustomProperties["Password"] != null)
            {
                newRoom.passwordRequired = true;
            }

            if (info.CustomProperties["Password"].ToString() == "")
            {
                newRoom.passwordRequired = false;
            }
            //  roomListEntries.Add(info.Name, newRoom);
        }
    }

    /*
    private void UpdateRoomList(List<RoomInfo> _list)
    {
        print("DestroyRooms");
        //destroy room items and re-popualte with new
        foreach (RoomItem item in roomItems)
        {
            Destroy(item.gameObject);
        }

        roomItems.Clear();

        //INstantiate room items for each room and add our new list
        print("Populate with new rooms");
        foreach (RoomInfo room in _list)
        {
            print("Room Name = " + room.Name);
            RoomItem newRoom =  Instantiate(RoomItemPrefab, contentObject);         //Instantiate the object
            newRoom.SetRoomName(room.Name);                                         //Set toom name in the item
            roomItems.Add(newRoom);                                                 //AddComponentMenu new to list
            print("Add Rooms");
        }
    }
    */


    public void  JoinRoom(string roomName)              //Join room
    {

        //UpdateRoomList(_list);
        //UpdateCachedRoomList(_list);

        PhotonNetwork.JoinRoom(roomName);
    }



    public void OnClickLeaveRoom()                      //leave room button
    {
        PhotonNetwork.LeaveRoom();
        Destroy(tempBonfire);
        
        //refresh rooms
    }

    //Runs autoamtically when leaving room

    public override void OnLeftRoom()

    {
        if (roomPanel != null)
        {
            roomPanel.SetActive(false);
        }

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
        }
     
    }

    public override void OnConnectedToMaster()      //Join master server when leaving a lobby. 
    {
        //Added 13/12/2021
        print("Connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;


        PhotonNetwork.JoinLobby();
    }


    void UpdatePlayerList()
    {
        //divided into 2 steps, 1st delete all items and delete, then populate for each player in the room
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
            
        }

        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)      //If the room is clear, exist out of code
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) //looping through the current room players, retrieving the curret players in the room. It comes inthe form of a KVP (dictionary)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefeb, playerItemParent);
            newPlayerItem.Setplayerinfo(player.Value);

            //buttons
          // newPlayerItem.GetComponent<PlayerItem>().leftArrowButton.SetActive(false);
         //   newPlayerItem.GetComponent<PlayerItem>().rightArrowButton.SetActive(false);


            if (player.Value == PhotonNetwork.LocalPlayer) //checking if this is the local player in question
            {
                newPlayerItem.ApplyLocalChanges();
       
            }

            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)              //when player enters room
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)           //when player leaves room
    {
        UpdatePlayerList();
    }



    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayerCount) //is Host? less than room limit
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        //PhotonNetwork.GetCustomRoomList

       

        if (public_Toggle.isOn)
        {
            passwordBox.gameObject.SetActive(false);
            passwordField.text = "";
        }


         if (private_Toggle.isOn == true)
        {
            passwordBox.gameObject.SetActive(true);

        }


        }

   

    //-------THIS NEEDS TO BE MADE EXPANDABLE
    //--This should take into account locked levels - based on whoever is the host
    //--A UI interface should be created to select levels in the lobby
    //--Save data

    public void OnClickPlayButton()
    {
       // PhotonNetwork.LoadLevel("Game"); //Load the 'Game' scene when master cliet clicks
    }

    public void OnClickLevelSelect()
    {

        Debug.Log("Checking Character".Bold());

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            print( " name: " + player.NickName);
            int characterSelected = ((int)player.CustomProperties["playerAvatar"]);
            print(" ID: " + characterSelected);

            characterSelectedID.Add(characterSelected);

           

        }

        if (characterSelectedID.Count == characterSelectedID.Distinct().Count())
        {
            // Duplicates exist
            uniqueList = true;
            Debug.Log("Unique List".Bold());

            lobbyPanel.SetActive(false);
            roomPanel.SetActive(false);
            ghosthumanWarning.gameObject.SetActive(false);
            levelSelectPanel.SetActive(true);
        }

        else
        {
            uniqueList = false;

            Debug.Log("not unique Unique List".Bold());
            ghosthumanWarning.gameObject.SetActive(true);
        }

        characterSelectedID.Clear();



    }

    public void OnClickLeaveLevelSelect()
    {

        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);

            levelSelectPanel.SetActive(false);
        }


        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            levelSelectPanel.SetActive(false);
        }

        }


    /*

    public void OnClick_CallKick()
    {
        Kick(nicknameInputField);
      //  print("Kick Button Clicked");
    }

    private void Kick(TMP_InputField inputField)
    {
        if (inputField == null)
        {
            return; // log error?
        }
        string nickname = inputField.text;
        Kick(nickname);
    }

    private void Kick(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            return; // log error?
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) //looping through the current room players, retrieving the curret players in the room. It comes inthe form of a KVP (dictionary)
        {
            if (player.Value != PhotonNetwork.LocalPlayer) //checking if this is the local player in question
            {
              
                if (nickname.Equals(player.Value.NickName))
                {
                //    print("Kicking " + player.Value);
                    KickingPlayer(player.Value);
                    return;
                }


            }
 


        }
        // log error? player is local or not found?
    }

    private void KickingPlayer(Player playerToKick)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return; // log error?
        }
        print("Closing connection for " + playerToKick.NickName);




        //not working
        PhotonNetwork.EnableCloseConnection = true;
         PhotonNetwork.CloseConnection(playerToKick);

  
    }
    */

   public void SpawnLobbyPlayer ()
    {


        if (SceneSettings.Instance.isSinglePlayer == true)
        {



            int randomNumber = UnityEngine.Random.Range(0, Lobby_Room_Player_SpawnPoints.Length);
            Transform spawnPoint = Lobby_Room_Player_SpawnPoints[randomNumber].transform;

            //Access Save File 
            GameObject playerToSpawn = playerPrefabs[0]; //Choosing Human

            spawnedLobbyPlayer = Instantiate(playerToSpawn, spawnPoint.position, spawnPoint.rotation);
            spawnedLobbyPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = true;


        }


        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            int randomNumber = UnityEngine.Random.Range(0, Lobby_Room_Player_SpawnPoints.Length);
            Transform spawnPoint = Lobby_Room_Player_SpawnPoints[randomNumber].transform;

            GameObject playerToSpawn = playerPrefabs[(int)playerproperties["playerAvatar"]];


            //    Debug.Log("Cretated Player Controller " + playerPrefabs[(int)playerproperties["playerAvatar"]]);

            //   Debug.Log("Im located on " + this.gameObject);
            //  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);


            print("Spawned Player ID = " + (int)playerproperties["playerAvatar"]);

            spawnedLobbyPlayer = Instantiate(playerToSpawn, spawnPoint.position, spawnPoint.rotation);
            spawnedLobbyPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = true;
        }

        
    }

 public void DebuggingFunction()
    {
        //   print("Region " + PhotonNetwork.CloudRegion);
        regionTextbox.text = PhotonNetwork.CloudRegion;
    }




    /// <summary>
    ///CUT SCENE DELAY
    /// </summary>
    /// <returns></returns>

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);


        //Instantiate Chosen Character
        
    }

    public void OnTriggerSpawnPlayers ()
    {
        SpawnLobbyPlayer();
    }


    public void select_ExitShrine()
    {
       // exitUI.gameObject.GetComponent<Animator>().SetBool("UIAppear", false);
      //  exitUI.gameObject.GetComponent<Animator>().SetBool("UIDisappear", true);

        bookCamera.gameObject.SetActive(false);
        lobbyUI.gameObject.SetActive(false);
    }

}

    
