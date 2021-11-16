using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePuzzle_CheckForPiece : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Missing Item")]
    public Item missingItem;

    [Header("Fixed Bridge")]
    public Mesh fixedBridgeMesh;
    public GameObject missingPieceBoxCollder;

    [Header("Broken Bridge")]
    public GameObject brokenBridge;
    //public GameObject brokenBridgeMesh;

    public bool DebugOutSideOfNetwork = false;

    public void Start()
    {

        DebugOutSideOfNetwork = false;
        missingPieceBoxCollder.SetActive(false);
    }

    private void Update()
    {
        if (DebugOutSideOfNetwork == true)
        {
            brokenBridge.GetComponent<MeshFilter>().mesh = fixedBridgeMesh;
            Debug.Log("2 golden keys");
            missingPieceBoxCollder.SetActive(true);
        }

    }

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
                    brokenBridge.GetComponent<MeshFilter>().mesh = fixedBridgeMesh;
                        Debug.Log("2 golden keys");
                    missingPieceBoxCollder.SetActive(true);
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
