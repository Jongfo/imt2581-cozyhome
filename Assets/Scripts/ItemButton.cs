using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    Item item;
    [SerializeField] Text itemNameText;
    [SerializeField] Image itemSprite;
    [SerializeField] Text moneyText;

    TradePanel tradePanel;

    bool belongsToPlayer;
    bool selected = false;

    public void SetUp(Item i, bool player)
    {
        item = i;
        itemNameText.text = item.itemName;
        itemSprite.sprite = item.sprite;
        moneyText.text = item.cost + "";

        belongsToPlayer = player;
        tradePanel = GameObject.Find("TradePanel").GetComponent<TradePanel>();
    }

    public void OnClick()
    {
        if (selected)
        {
            // Unselect it
            selected = false;
            GetComponent<Image>().color = Color.white;
            if (belongsToPlayer)
            {
                tradePanel.playerItems.Remove(item);
                tradePanel.npcCost -= item.cost;
            }
            else
            {
                tradePanel.npcItems.Remove(item);
                tradePanel.playerCost -= item.cost;
            }
        }
        else
        {
            // Highlight it
            selected = true;
            GetComponent<Image>().color = Color.green;
            // Add cost of item to total for that side of the trade
            if (belongsToPlayer)
            {
                tradePanel.playerItems.Add(item);
                tradePanel.npcCost += item.cost;
            }
            else
            {
                tradePanel.npcItems.Add(item);
                tradePanel.playerCost += item.cost;
            }
        }
    }
}
