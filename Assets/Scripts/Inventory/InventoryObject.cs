using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

    public bool twoKeysCollected = false;

    public void AddItem(Item _item, int _amount)
    {
        // Check if item is already in inventory
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if (Container.Items[i].item.Id == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);

                if (_item.Name == "GoldenKey")
                {
                    twoKeysCollected = true;
                }

                return;
            }
        }

        // If item not in inventory, create a new slot
        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
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
    public List<InventorySlot> Items = new List<InventorySlot>();
    //public InventorySlot[] Items = new InventorySlot[4];//
}


[Serializable]
public class InventorySlot
{
    public int Id;
    public Item item;
    public int amount;

    public InventorySlot(int _id, Item _item, int _amount)
    {
        Id = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
