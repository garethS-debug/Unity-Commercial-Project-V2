using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterID : MonoBehaviour
{

    public string CharaceterName;

    public bool isGhostCharacer;

    public bool isHumanCharater;


    [Header("SO")]
    public PlayerSO playerSOData;


    [Header("Photon Settings")]
    PhotonView PV;


    public void Start()
    {
        GameObject griefBar = GameObject.FindGameObjectWithTag("GriefBar");

        //Photon
        PV = GetComponent<PhotonView>();


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

}
