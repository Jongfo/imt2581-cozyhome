using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    static QuestManager instance;
    [SerializeField] Quest[] quests;
    Dictionary<Quest, int> questStages;

    bool questTriggered = false;
    Quest currentQuest = null;

    Player player;
    GameManager gm;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        questStages = new Dictionary<Quest, int>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gm = GameManager.instance;
        foreach (Quest quest in quests)
        {
            currentQuest = quest;
            questStages.Add(quest, 0);
            quest.action.Invoke();
        }
    }

    private void Update()
    {
        foreach (Quest quest in quests)
        {
            currentQuest = quest;
            questTriggered = false;
            quest.trigger.Invoke();
            if (questTriggered)
            {
                Debug.Log("Quest triggered");
                questStages[quest]++;
                quest.action.Invoke();
            }
        }
    }

    public void SampleQuestTrigger()
    {
        if (GameManager.instance.Day == 1)
        {
            if (instance.questStages[instance.currentQuest] == 0)
            {
                instance.questTriggered = true;
            }
            if (instance.questStages[instance.currentQuest] == 1)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character != null && character.characterName == "Peter")
                {
                    instance.questTriggered = true;
                }
            }
            else if (instance.questStages[instance.currentQuest] == 2)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character == null)
                {
                    instance.questTriggered = true;
                }
            }
            else if (instance.questStages[instance.currentQuest] == 3)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character != null && character.characterName == "Peter")
                {
                    if (instance.player.inventory.GetItemCount("Log") >= 2)
                    {
                        instance.questTriggered = true;
                    }
                }
            }
        }
    }

    public void SampleQuestAction()
    {
        // This quest is only valid on day 1
        if (instance.gm.Day == 1)
        {
            Debug.Log("action triggered");
            if (instance.questStages[instance.currentQuest] == 1)
            {
                NonPlayerCharacter.GetCharacterWithName("Peter").prioritizedPhrase =
                    "I need two logs. Could you get them for me?";
            }
            else if (instance.questStages[instance.currentQuest] == 2)
            {
                Debug.Log(instance.currentQuest.description);
            }
            else if (instance.questStages[instance.currentQuest] == 3)
            {
                Debug.Log("Stopped talking");
            }
            else if (instance.questStages[instance.currentQuest] == 4)
            {
                NonPlayerCharacter peter = NonPlayerCharacter.GetCharacterWithName("Peter");
                peter.prioritizedPhrase = "Thanks for the logs. Here, have this toy.";
                peter.Talk();

                instance.player.inventory.RemoveItem("Log");
                instance.player.inventory.RemoveItem("Log");
                instance.player.inventory.AddItem(GameManager.instance.toyPrefab);
            }
        }
    }

    public void HugryPeterTrigger()
    {
        if (instance.gm.Day == 2)
        {
            Debug.Log("Quest2 Trigger");
            if (instance.questStages[instance.currentQuest] == 0)
            {
                instance.questTriggered = true;
            }
            if (instance.questStages[instance.currentQuest] == 1)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character != null && character.characterName == "Peter")
                {
                    instance.questTriggered = true;
                }
            }
            else if (instance.questStages[instance.currentQuest] == 2)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character == null)
                {
                    instance.questTriggered = true;
                }
            }
            else if (instance.questStages[instance.currentQuest] == 3)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character != null && character.characterName == "Peter")
                {
                    if (instance.player.inventory.GetItemCount("Rabbit Meat") >= 2)
                    {
                        instance.questTriggered = true;
                    }
                }
            }
        }
    }
    public void HugryPeterAction()
    {
        if (instance.gm.Day == 2)
        {
            Debug.Log("Quest2 Action");
            if (instance.questStages[instance.currentQuest] == 1)
            {
                NonPlayerCharacter.GetCharacterWithName("Peter").prioritizedPhrase =
                    "I'm low on Rabbit Meat. Could you get two for me?";
            }
            else if (instance.questStages[instance.currentQuest] == 2)
            {
                Debug.Log(instance.currentQuest.description);
            }
            else if (instance.questStages[instance.currentQuest] == 3)
            {
                Debug.Log("Stopped talking");
            }
            else if (instance.questStages[instance.currentQuest] == 4)
            {
                NonPlayerCharacter peter = NonPlayerCharacter.GetCharacterWithName("Peter");
                peter.prioritizedPhrase = "Thanks for the food. Here, have this toy.";
                peter.Talk();

                instance.player.inventory.RemoveItem("Rabbit Meat");
                instance.player.inventory.RemoveItem("Rabbit Meat");
                instance.player.inventory.AddItem(GameManager.instance.toyPrefab);
            }
        }
    }

    public void JogeirDiesTrigger()
    {
        if (instance.gm.Day == 3)
        {
            if (instance.questStages[instance.currentQuest] == 0)
            {
                instance.questTriggered = true;
            }
            if (instance.questStages[instance.currentQuest] == 1)
            {
                NonPlayerCharacter character = GameManager.instance.characterCurrentlyBeingSpokenTo;
                if (character != null && character.characterName == "Dead Jogeir")
                {
                    instance.questTriggered = true;
                }
            }
        }
    }

    public void JogeirDiesAction()
    {
        if (instance.gm.Day == 3)
        {
            if (instance.questStages[instance.currentQuest] == 1)
            {
                NonPlayerCharacter.GetCharacterWithName("Peter").prioritizedPhrase =
                    "Jogeir took a hike up north. He hasn't returned for a while though...";
            }
            if (instance.questStages[instance.currentQuest] == 2)
            {
                instance.player.inventory.AddItem(instance.gm.toyPrefab);
            }
        }
    }
}
