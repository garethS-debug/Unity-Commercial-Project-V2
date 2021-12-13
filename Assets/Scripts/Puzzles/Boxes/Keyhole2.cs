using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyhole2 : MonoBehaviour
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
        //Two key collected, green keyhole
        if (humanInventory.twoKeysCollected || ghostInventory.twoKeysCollected)
        {
            keyholeRend.material = keyholeMat[1];
        }
    }
}
