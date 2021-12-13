using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTemple : MonoBehaviour
{
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    private Animator doorAnim;


    void Start()
    {
        doorAnim = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            if (other.gameObject.tag == "Player" && inventoryHuman.oneKeyCollected && inventoryGhost.oneKeyCollected)
            {
                doorAnim.SetBool("IsOpening", true);
            }
        }

        else if (SceneSettings.Instance.isSinglePlayer == true)
        {
             if (other.gameObject.tag == "Player" && inventoryHuman.twoKeysCollected || inventoryGhost.twoKeysCollected)
             {
                doorAnim.SetBool("IsOpening", true);
             }           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            if (other.gameObject.tag == "Player" && doorAnim)
            {
                doorAnim.SetBool("IsOpening", false);
            }
        }

        else if (SceneSettings.Instance.isSinglePlayer == true)
        {
            if (other.gameObject.tag == "Player" && doorAnim)
            {
                doorAnim.SetBool("IsOpening", false);
            }
        }
    }
}

