using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    public int xStart;
    public int xSpaceBetweenItems;
    public int yStart;
    public int ySpaceBetweenItems;
    public int numberOfColumns;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    //Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateDisplay(inventoryGhost);
        CreateDisplay(inventoryHuman);
    }

    public void Update()
    {
        UpdateDisplay(inventoryGhost);
        UpdateDisplay(inventoryHuman);
    }


    public void CreateDisplay(InventoryObject inv)
    {
        for (int i = 0; i < inv.Container.Items.Count; i++)
        {
            InventorySlot slot = inv.Container.Items[i];

            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inv.database.GetItem[slot.item.Id].uiDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemsDisplayed.Add(inv.Container.Items[i], obj);
        }
    }

    //public void CreateDisplay(InventoryObject inv)//
    //{
    //    for (int i = 0; i < inv.Container.Items.Length; i++)
    //    {
    //        var obj = Instantiate(inv.Container.Items[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
    //        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //        obj.GetComponentInChildren<TextMeshProUGUI>().text = inv.Container.Items[i].amount.ToString("n0");
    //        itemsDisplayed.Add(obj, inv.Container.Items[i]);
    //    }
    //}

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % numberOfColumns)), yStart + (-ySpaceBetweenItems * (i / numberOfColumns)), 0f);
    }

    public void UpdateDisplay(InventoryObject inv)
    {
        for (int i = 0; i < inv.Container.Items.Count; i++)
        {
            InventorySlot slot = inv.Container.Items[i];

            if (itemsDisplayed.ContainsKey(inv.Container.Items[i]))
            {
                itemsDisplayed[inv.Container.Items[i]].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inv.database.GetItem[slot.item.Id].uiDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                itemsDisplayed.Add(inv.Container.Items[i], obj);
            }
        }
    }
}
