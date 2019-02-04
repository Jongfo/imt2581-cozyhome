using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonPlayerCharacter : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] string description;
    [SerializeField] string[] randomPhrases;
    [SerializeField] Item[] startingItems;

    public string prioritizedPhrase = null;

    public Inventory inventory;

    private void Awake()
    {
        inventory = gameObject.AddComponent<Inventory>();
        foreach (Item item in startingItems)
        {
            inventory.AddItem(item);
        }
    }

    private void Update()
    {
        // TODO: Move around randomly
    }

    public void Trade()
    {
        // Opens trade panel with available items
        Debug.Log("Trading with " + characterName);
        GameObject tradePanel = Instantiate(GameManager.instance.tradePanelPrefab, GameObject.Find("Canvas").transform);
        tradePanel.name = "TradePanel";
        tradePanel.GetComponent<TradePanel>().SetUp(this);
    }

    public void Talk()
    {
        GameManager.instance.characterCurrentlyBeingSpokenTo = this;
        GameManager.instance.conversationPanel.SetActive(true);
        // Say something random unless there is something important to say
        if (prioritizedPhrase == null || prioritizedPhrase == "")
        {
            int phraseIndex = Random.Range(0, randomPhrases.Length);
            GameObject.Find("ConversationText").GetComponent<Text>().text = randomPhrases[phraseIndex];
        }
        else
        {
            GameObject.Find("ConversationText").GetComponent<Text>().text = prioritizedPhrase;
            prioritizedPhrase = null;
        }
        GameObject.Find("ConversationCharacterImage").GetComponent<Image>().sprite =
            GetComponent<SpriteRenderer>().sprite;
        GameObject.Find("ConversationCharacterImage").GetComponent<Image>().color =
            GetComponent<SpriteRenderer>().color;
    }

    public static NonPlayerCharacter GetCharacterWithName(string name)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject o in objects)
        {
            NonPlayerCharacter c = o.GetComponent<NonPlayerCharacter>();
            if (c.characterName == name)
            {
                return c;
            }
        }

        return null;
    }
}
