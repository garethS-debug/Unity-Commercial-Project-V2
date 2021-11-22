using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    public int xStart;
    public int xSpaceBetweenItems;
    public int yStart;
    public int ySpaceBetweenItems;
    public int numberOfColumns;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

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
        for (int i = 0; i < inv.Container.Count; i++)
        {
            var obj = Instantiate(inv.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inv.Container[i].amount.ToString("n0");
            itemsDisplayed.Add(inv.Container[i], obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % numberOfColumns)), yStart + (-ySpaceBetweenItems * (i / numberOfColumns)), 0f);
    }

    public void UpdateDisplay(InventoryObject inv)
    {
        for (int i = 0; i < inv.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inv.Container[i]))
            {
                itemsDisplayed[inv.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inv.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inv.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inv.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inv.Container[i], obj);
            }
        }
    }
}
