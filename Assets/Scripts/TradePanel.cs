using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePanel : MonoBehaviour
{
    [SerializeField] GameObject itemButtonPrefab;
    [SerializeField] GameObject playerTradePanel;
    [SerializeField] GameObject npcTradePanel;

    // the items that are selected for trade
    public List<Item> playerItems;
    public List<Item> npcItems;

    public int playerCost;
    public int npcCost;


    public void SetUp(NonPlayerCharacter npc)
    {
        // Set up player's items
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        foreach (Item item in player.inventory.items)
        {
            GameObject button = Instantiate(itemButtonPrefab, playerTradePanel.transform);
            button.GetComponent<ItemButton>().SetUp(item, true);
        }

        // Set up NPC's items
        foreach (Item item in npc.inventory.items)
        {
            GameObject button = Instantiate(itemButtonPrefab, npcTradePanel.transform);
            button.GetComponent<ItemButton>().SetUp(item, false);
        }

    }

    public void TearDown()
    {
        GameObject[] itemButton = GameObject.FindGameObjectsWithTag("ItemButton");
        for (int i = 0; i < itemButton.Length; i++)
        {
            Destroy(itemButton[i]);
        }
    }
}
