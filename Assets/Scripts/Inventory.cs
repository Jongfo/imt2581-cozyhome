using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int maxItems = 30;
    public List<Item> items;
    Text inventoryList;

    public bool belongsToPlayer = false;

    int markedItemIndex;

    void Awake()
    {
        items = new List<Item>();
        inventoryList = GameObject.Find("InventoryList").GetComponent<Text>();
    }

    public void AddItem(Item item)
    {
        if (items.Count == 0)
        {
            markedItemIndex = 0;
        }

        items.Add(item);
        Debug.Log("Added" + item.itemName);

        if (belongsToPlayer)
        {
            UpdateInventoryList();
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        UpdateInventoryList();
    }

    public void RemoveItem(string itemName)
    {
        Item toRemove = null;
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                toRemove = item;
                break;
            }
        }
        if (toRemove != null)
        {
            items.Remove(toRemove);
        }
        UpdateInventoryList();
    }

    public void OnEndDay()
    {
        // Remove all items (for now)
        items.Clear();
        if (belongsToPlayer)
        {
            UpdateInventoryList();
        }
    }

    /// <summary>
    /// Display an X next to the next item to be marked in the inventory
    /// </summary>
    public void MarkNextItem()
    {
        if (items.Count > 0)
        {
            markedItemIndex++;
            if (markedItemIndex >= items.Count)
            {
                markedItemIndex = 0;
            }
            UpdateInventoryList();
        }
    }

    public void DropMarkedItem()
    {
        int oldCount = items.Count;
        if (oldCount > 0)
        {
            Instantiate(
                items[markedItemIndex].itemPickupPrefab,
                transform.position,
                Quaternion.identity,
                null
            );
            items.Remove(items[markedItemIndex]);
            if (markedItemIndex == oldCount - 1)
            {
                markedItemIndex--;
            }
            UpdateInventoryList();
        }
    }


    void UpdateInventoryList()
    {
        if (belongsToPlayer)
        {
            Debug.Log("Updating inventory list");
            inventoryList.text = "Inventory\n";
            for (int i = 0; i < items.Count; i++)
            {
                if (i == markedItemIndex)
                {
                    inventoryList.text += "X ";
                }
                inventoryList.text += items[i].itemName + "\n";
            }
        }
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                count++;
            }
        }
        return count;
    }
}
