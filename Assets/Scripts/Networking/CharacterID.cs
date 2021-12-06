using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterID : MonoBehaviour
{

    public string CharaceterName;

    public bool isGhostCharacer;

    public bool isHumanCharater;

    public bool isSinglePlayer;


    [Header("SO")]
    public PlayerSO playerSOData;


    [Header("Photon Settings")]
    PhotonView PV;


    public void Start()
    {
        GameObject griefBar = GameObject.FindGameObjectWithTag("GriefBar");



        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            //Photon
            PV = GetComponent<PhotonView>();
        }

        if (isGhostCharacer == true)
        {
            SceneSettings.Instance.ghostPlayer = this.gameObject;
        }


        if (isHumanCharater == true)
        {
            SceneSettings.Instance.humanPlayer = this.gameObject;
        }




        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            //check if in lobby

            if (this.gameObject.GetComponent<NetworkedPlayerController>().isInLobby == false)
            {
                if (PV.IsMine)
                {
                    if (isGhostCharacer)
                    {
                        griefBar.GetComponent<GriefBarDisplay>().CreateGhostCandle();
                    }
                    if (isHumanCharater)
                    {
                        griefBar.GetComponent<GriefBarDisplay>().CreateHumanCandle();
                    }

                }
            }

     


        }

        else if (SceneSettings.Instance.isSinglePlayer == true)
        {
            SceneSettings.Instance.RemoveMultiplayerScript(this.gameObject);

            if (isGhostCharacer)
            {
                if (gameObject.GetComponent<NetworkedPlayerController>().isInLobby == false)
                {
                    griefBar.GetComponent<GriefBarDisplay>().CreateGhostCandle();
                }
            }
            if (isHumanCharater)
            {
                if (gameObject.GetComponent<NetworkedPlayerController>().isInLobby == false)
                {
                    griefBar.GetComponent<GriefBarDisplay>().CreateHumanCandle();
                }
              
            }
        }



      


    }

}
