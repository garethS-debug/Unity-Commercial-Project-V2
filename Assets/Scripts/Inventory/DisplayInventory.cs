using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    public PlayerSO playerSO;
    public int xStart;
    public int xSpaceBetweenItems;
    public int yStart;
    public int ySpaceBetweenItems;
    public int numberOfColumns;

    public MouseItem mouseItem = new MouseItem();

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        if (playerSO.PlayerCharacterChoise == 1)
        {
            CreateDisplay(inventoryHuman);
        }

        if (playerSO.PlayerCharacterChoise == 2)
        {
            CreateDisplay(inventoryGhost);
        }
    }

    public void Update()
    {
        if (playerSO.PlayerCharacterChoise == 1)
        {
            UpdateDisplay(inventoryHuman);
        }

        if (playerSO.PlayerCharacterChoise == 2)
        {
            UpdateDisplay(inventoryGhost);
        }
    }

    public void CreateDisplay(InventoryObject inv)
    {
        for (int i = 0; i < inv.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            //Set up the drag and swap for the items
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj, inv); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj, inv); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            //obj.GetComponentInChildren<TextMeshProUGUI>().text = inv.Container.Items[i].amount.ToString("n0");
            itemsDisplayed.Add(obj, inv.Container.Items[i]);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % numberOfColumns)), yStart + (-ySpaceBetweenItems * (i / numberOfColumns)), 0f);
    }

    public void UpdateDisplay(InventoryObject inv)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inv.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount.ToString("n0");
            }

            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        //Set up the button to allow items swapping
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj, InventoryObject inv)
    {
        var mouseObject = new GameObject();
        var rect = mouseObject.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 100);
        mouseObject.transform.SetParent(transform.parent);

        if(itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inv.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }

        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj, InventoryObject inv)
    {
        if (mouseItem.hoverObj)
        {
            inv.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }

        else
        {
            inv.RemoveItem(itemsDisplayed[obj].item);
        }

        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if(mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public class MouseItem
    {
        public GameObject obj;
        public InventorySlot item;
        public GameObject hoverObj;
        public InventorySlot hoverItem;
    }

}
