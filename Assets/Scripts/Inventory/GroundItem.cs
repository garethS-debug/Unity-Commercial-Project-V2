using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GroundItem : MonoBehaviour
{
    public ItemObject item;

    private void Start()
    {
        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            SceneSettings.Instance.RemoveMultiplayerScript(this.gameObject);
        }
    }

    public void DestroyItem()
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            //Get PhotonView on this gameobject
            PhotonView photonView = PhotonView.Get(this);
            //Send an RPC call to everyone
            photonView.RPC("m_DestroyObject", RpcTarget.All);
        }

        if (SceneSettings.Instance.isSinglePlayer == true)
        {
            this.gameObject.SetActive(false);
        }

    }

    [PunRPC]
    void m_DestroyObject()                                      
    {
        //Making sure the object is destroyed on everyones copy of the game
        this.gameObject.SetActive(false);
    }
}
