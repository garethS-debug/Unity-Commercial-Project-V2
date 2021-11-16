using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


//Displaying the correct room name for each room item and joining the room on clic 

public class RoomItem : MonoBehaviour
{

    [Header("Rooom Button")]
    public TMP_Text roomName;
    public GameObject roomButton;

    LobbyManager manager;



    [Header("password")]
    public RoomInfo roomInfo;
    public string thePasswordDummy;
    public TMP_InputField password;
    public GameObject PasswordBox;
    private string passwordtext;
    public bool passwordRequired;


    [Header("3D Lobby Settings")]
    public GameObject LobbySpawnPoint;
    public GameObject BonfireGO;
    public GameObject bonfireGhost;
    public GameObject bonfireHuman;
    public bool isHostVersion;

    [Header("RequiredPlayer")]
    public int remainingPlayerID;
    public int playerCurrentlyInRoom;
    public bool DebugMode;


    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();

        if (passwordRequired)
        {
            PasswordBox.gameObject.SetActive(true);

            if (roomInfo.CustomProperties["Password"].ToString() != "")
            {
                thePasswordDummy = roomInfo.CustomProperties["Password"].ToString();
            }
        }

        if (!passwordRequired)
        {
            PasswordBox.gameObject.SetActive(false);
        }


      //  Debug.Log(" ROOM ID = " + roomInfo.CustomProperties["3DLobby"]);

        roomButton.SetActive(false);
        PasswordBox.gameObject.SetActive(false);


        //Room ID required (opposite to joining player)
        //----HUman = 1
        //----Ghost = 2
        if (remainingPlayerID == 2)
        {
            bonfireHuman.SetActive(true);
            bonfireGhost.SetActive(false);
        }

        if (remainingPlayerID == 1)
        {
            bonfireHuman.SetActive(false);
            bonfireGhost.SetActive(true);
        }
    }


    // Start is called before the first frame update
    public void SetRoomName(string _roomName)
    {

        roomName.text = _roomName;

    }

    public void WrittenPassword (string _password)
    {
        PasswordBox.gameObject.SetActive(true);
        passwordtext = _password;

    }
  
    public void OnClickItem()
    {
    


        //Check for remaining player ID
        //get your instance of lobby manager
        LobbyManager lobbymanager = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>();
        print("Your ID is : " + lobbymanager.spawnedLobbyPlayer.GetComponent<CharacterID>().playerSOData.PlayerCharacterChoise + "Remaining ID is " + remainingPlayerID);

        if (DebugMode == false && lobbymanager.spawnedLobbyPlayer.GetComponent<CharacterID>().playerSOData.PlayerCharacterChoise == remainingPlayerID)
        {

        

        if (passwordRequired)
        {
            passwordtext = password.text;

            if (passwordtext == thePasswordDummy)
            {
                Debug.Log("WELL DONE PASSWORD CORRECT");
                manager.JoinRoom(roomName.text);
                    roomButton.SetActive(false);

                    PasswordBox.gameObject.SetActive(false);

                }

            else
            {
                Debug.Log("PASSWORD INCORRECT");
                Debug.Log("You Entered " + passwordtext);
            }
           
        }
        if (!passwordRequired)
        {
            manager.JoinRoom(roomName.text);
                roomButton.SetActive(false);

                PasswordBox.gameObject.SetActive(false);

            }
        }

        if (DebugMode == true) 
        
        
        {
            if (passwordRequired)
            {
                passwordtext = password.text;

                if (passwordtext == thePasswordDummy)
                {
                    Debug.Log("WELL DONE PASSWORD CORRECT");
                    manager.JoinRoom(roomName.text);
                    roomButton.SetActive(false);

                    PasswordBox.gameObject.SetActive(false);

                }

                else
                {
                    Debug.Log("PASSWORD INCORRECT");
                    Debug.Log("You Entered " + passwordtext);
                }

            }
            if (!passwordRequired)
            {
                manager.JoinRoom(roomName.text);
                roomButton.SetActive(false);

                PasswordBox.gameObject.SetActive(false);

            }


        }

    }

    public void onClickLeaveRoom()
    {
        if (isHostVersion == false)
        {
            roomButton.SetActive(false);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (isHostVersion == false)
        {
            if (other.gameObject.tag == "Player")
            {
                roomButton.SetActive(true);

                if (thePasswordDummy != "")
                {
                    PasswordBox.gameObject.SetActive(true);
                }
            }
        }
   
       
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          //  roomButton.SetActive(true);
            
            if (thePasswordDummy != "")
            {
            //    PasswordBox.gameObject.SetActive(true);
            }
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (isHostVersion == false)
        {
            if (other.gameObject.tag == "Player")
            {
                roomButton.SetActive(false);

                if (thePasswordDummy != "")
                {
                    PasswordBox.gameObject.SetActive(false);
                }

            }
        }
    
    }
}
