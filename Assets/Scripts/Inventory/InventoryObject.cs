using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    //public Inventory Container;//
    public List<InventorySlot> Container = new List<InventorySlot>();

    public bool twoKeysCollected = false;

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/InventoryDatabase.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("InventoryDatabase");
#endif
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            Container[i].item = database.GetItem[Container[i].Id];
        }
    }

    public void OnBeforeSerialize()
    {

    }

    public void AddItem(ItemObject _item, int _amount)
    {
        // Check if item is already in inventory
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);

                if (_item.name == "GoldenKey")
                {
                    twoKeysCollected = true;
                }

                return;
            }
        }

        // If item not in inventory, create a new slot
        Container.Add(new InventorySlot(database.GetId[_item], _item, _amount));
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

//[Serializable]
//public class Inventory//
//{
//    public InventorySlot[] Items = new InventorySlot[4];//
//}


[Serializable]
public class InventorySlot
{
    public int Id;
    public ItemObject item;
    public int amount;

    public InventorySlot(int _id, ItemObject _item, int _amount)
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
