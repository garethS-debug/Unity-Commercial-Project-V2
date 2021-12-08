using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyhole1 : MonoBehaviour
{
    public Material[] keyholeMat;
    private MeshRenderer keyholeRend;
    public InventoryObject humanInventory;
    public InventoryObject ghostInventory;

    private void Awake()
    {
        keyholeRend = GetComponent<MeshRenderer>();

        //No key collected, red keyhole
        keyholeRend.material = keyholeMat[0];
    }

    void Update()
    {
        //One or two key collected, green keyhole
        if (humanInventory.oneKeyCollected || ghostInventory.oneKeyCollected || humanInventory.twoKeysCollected || ghostInventory.twoKeysCollected)
        {
            keyholeRend.material = keyholeMat[1];
        }
    }
}
