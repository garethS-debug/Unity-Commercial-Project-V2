using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private MeshRenderer gateRend;
    private static Gate instance;
    private GameObject player;
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    private Animator anim;

    public RoomManager roomManager;


    private void Awake()
    {
        instance = this;
        gateRend = GetComponent<MeshRenderer>();
       // player = GameObject.FindGameObjectWithTag("Player");                //Players dont spawn on load, so this might miss the players. 
        anim = GetComponent<Animator>();
    }


    public void Start()
    {
        if (roomManager.networkedPlayer != null)
        {
            player = roomManager.networkedPlayer;                   //This gets the player from the Room Manager, which spawns the player
        }

        else
        {
            print("Error!");
        }
    
    }



    private void Update()
    {
        if (inventoryHuman.twoKeysCollected || inventoryGhost.twoKeysCollected)
        {
            ChangeColor();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((inventoryHuman.twoKeysCollected || inventoryGhost.twoKeysCollected) && other.gameObject.tag == "Player")
        {
            print("Door Open");
            anim.SetTrigger("OpenDoor");
        }
    }

    public void pauseAnimationEvent()
    {
        anim.enabled = false;
    }

    public void ChangeColor()
    {
        gateRend.material.color = Color.yellow;
    }

}
