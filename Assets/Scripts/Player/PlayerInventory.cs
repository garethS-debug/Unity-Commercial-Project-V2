using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;


    [Header("Photon Settings")]
    PhotonView PV;                                              //Setting up photon view 

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            if (this.gameObject.GetComponent<PhotonView>() != null)
            {
                PV = this.gameObject.GetComponent<PhotonView>();        //Get the photonview on the player
            }

        }

        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            if (PV.IsMine)                                          //Only run this script on the owning player who triggered the event
            {
                if (item)
                {
                    inventory.AddItem(item.item, 1);                    //Photon 


                    item.DestroyItem();
                }
            }
        }

        else if (SceneSettings.Instance.isSinglePlayer == true)
        {
            if (item)
            {
                inventory.AddItem(item.item, 1);                    //singlePlayer


                item.DestroyItem();
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Clear the player's inventory when they quit
        inventory.Container.Clear();
    }


}
