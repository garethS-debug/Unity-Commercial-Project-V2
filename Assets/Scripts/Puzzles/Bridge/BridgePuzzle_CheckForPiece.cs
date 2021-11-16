using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePuzzle_CheckForPiece : MonoBehaviour
{
    // Start is called before the first frame update
    public Item missingItem;




    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInventory inventory = other.gameObject.GetComponent<PlayerInventory>();

            for (int i = 0; i < inventory.inventory.Container.Count; i++)
            {
               if (inventory.inventory.Container[i].item == missingItem.item)
                {

                    
                    //Container[i].AddAmount(_amount);
                  //  hasItem = true;

                   // if (_item.name == "GoldenKey")
                   // {
                        Debug.Log("2 golden keys");
                   // }
               }

               else
                {
                    Debug.Log("No Luck");
                    Debug.Log("----------");
                    Debug.Log("inventory Item = " + inventory.inventory.Container[i].item);
                }
            }



        //    inventory.inventory
            
        }


    }

    public void OnTriggerStay(Collider other)
    {

    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }
}
