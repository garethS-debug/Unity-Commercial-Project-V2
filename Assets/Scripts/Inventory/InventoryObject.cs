using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Photon.Pun;

[CreateAssetMenu(fileName = "New inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public PlayerSO playerSO;
    public Inventory Container;


    public bool twoKeysCollected = false;

    public void AddItem(Item _item, int _amount)
    {
        // Check if item is already in inventory
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);

                if (_item.Name == "GoldenKey")
                {
                    twoKeysCollected = true;
                }
                return;
            }
        }

        SetEmptySlot(_item, _amount);
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }

        //Needs to work on solution when inventory is full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == _item)
            {
                //If one item in inventory, remove it completely
                if (Container.Items[i].amount == 1)
                {
                    Container.Items[i].UpdateSlot(-1, null, 0);
                }

                //If more than one item in inventory, remove one of them
                if (Container.Items[i].amount >= 2)
                {
                    int newAmount = Container.Items[i].amount - 1;
                    Container.Items[i].UpdateSlot(Container.Items[i].ID, Container.Items[i].item, newAmount);
                }

                Vector3 positionPrefab = new Vector3();
                Vector3 offsetPrefab = new Vector3(0, 2f, -5f);

                if (playerSO.PlayerCharacterChoise == 1)
                {
                    positionPrefab = GameObject.Find("HumanPlayerCharacter(Clone)").transform.position + offsetPrefab;
                }

                if (playerSO.PlayerCharacterChoise == 2)
                {
                    positionPrefab = GameObject.Find("GhostCharacter(Clone)").transform.position + offsetPrefab;
                }

                //Instantiate an item on the ground
                GameObject prefab = database.GetItem[_item.Id].prefabItem;
                Instantiate(prefab, positionPrefab, Quaternion.identity);

                //Need to work on multiplayer spawn
                //PhotonNetwork.Instantiate(prefab.name, positionPrefab, Quaternion.identity);

            }
        }
    }

    public void PlayerPosition()
    {

    }

    public void SaveInventory()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void LoadInventory()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

}

[Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[4];
}

[Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
