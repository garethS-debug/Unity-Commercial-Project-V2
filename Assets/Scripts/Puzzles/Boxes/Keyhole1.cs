using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyhole1 : MonoBehaviour
{
    private MeshRenderer keyholeRend;
    public InventoryObject humanInventory;
    public InventoryObject ghostInventory;

    private void Awake()
    {
        keyholeRend = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (humanInventory.oneKeyCollected || ghostInventory.oneKeyCollected || humanInventory.twoKeysCollected || ghostInventory.twoKeysCollected)
        {
            keyholeRend.material.color = Color.green;
        }
    }
}
