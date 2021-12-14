using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;

    //Setting up photon view
    [Header("Photon Settings")]
    PhotonView PV;                                               

    private void Start()
    {
        inventory.ResetKeys();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            inventory.SaveInventory();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.LoadInventory();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "item")
        {
        var item = other.GetComponent<GroundItem>();

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                if (this.gameObject.GetComponent<PhotonView>() != null)
                {
                    //Get the photonview on the player
                    PV = this.gameObject.GetComponent<PhotonView>();
                }
            }

            if (SceneSettings.Instance.isMultiPlayer == true)
            {
                //Only run this script on the owning player who triggered the event
                if (PV.IsMine)                                          
                {
                    if (item)
                    {
                        inventory.AddItem(new Item(item.item), 1);
                        item.DestroyItem();
                    }
                }
            }

            else if (SceneSettings.Instance.isSinglePlayer == true)
            {
                if (item)
                {
                    inventory.AddItem(new Item(item.item), 1);
                    item.DestroyItem();
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Clear the player's inventory when they quit
        inventory.Container.Items = new InventorySlot[4];//
    }
}
