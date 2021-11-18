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
                griefBar.GetComponent<GriefBarDisplay>().CreateGhostCandle();
            }
            if (isHumanCharater)
            {
                griefBar.GetComponent<GriefBarDisplay>().CreateHumanCandle();
            }
        }



      


    }

}
