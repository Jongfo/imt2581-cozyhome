using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] float timePerDay; // the time the player has to return home
    float timePassedToday = 0.0f;

    int day = 0;
    public int Day
    {
        get { return day; }
    }
    [SerializeField] int lastDay = 5;

    RectTransform cozynessBar;
    float cozynessBarMaxWidth;

    RectTransform endOfDayCozynessBar;
    float endOfDayCozynessBarMaxWidth;

    RectTransform warmnessBar;
    float warmnessBarMaxWidth;

    RectTransform timeBar;
    float timeBarMaxWidth;

    const int maxCozyness = 100;
    int cozyness;

    // Daily requirement
    [SerializeField] int[] logsNeeded;
    [SerializeField] int[] meatNeeded;
    [SerializeField] int[] toysNeeded;

    Animator dayTransitionPanel;
    Text dayTransitionText;
    Text dayTransitionBlurb;

    [SerializeField] string[] dayTransitionTexts;
    [SerializeField] string[] dayTransitionBlurbs;
    [SerializeField] float[] temperatures; // celsius

    public NonPlayerCharacter characterCurrentlyBeingSpokenTo;

    public GameObject conversationPanel;
    public GameObject tradePanelPrefab;

    GameObject gameOverPanel;

    GameObject endOfDayPanel;

    GameObject settingsPanel;

    Player player;

    public int Cozyness
    {
        set { cozyness = Mathf.Clamp(value, 0, maxCozyness); UpdateCozynessBar(); }
        get { return cozyness; }
    }

    public int nextItemId = 0;

    [SerializeField] Vector2[] wind;

    public Item logPrefab;
    public Item meatPrefab;
    public Item toyPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(GameObject.Find("Canvas"));
        }
        else
        {
            Destroy(gameObject);
        }
 
    }

    // Start is called before the first frame update
    void Start()
    {
        cozynessBar = GameObject.Find("CozynessBar").GetComponent<RectTransform>();
        cozynessBarMaxWidth = GameObject.Find("CozynessBarBG").GetComponent<RectTransform>().sizeDelta.x - 10;
        Cozyness = 50;

        endOfDayCozynessBar = GameObject.Find("EndOfDayCozynessBar").GetComponent<RectTransform>();
        endOfDayCozynessBarMaxWidth = GameObject.Find("EndOfDayCozynessBarBG").GetComponent<RectTransform>().sizeDelta.x - 10;

        warmnessBar = GameObject.Find("WarmnessBar").GetComponent<RectTransform>();
        warmnessBarMaxWidth = GameObject.Find("WarmnessBarBG").GetComponent<RectTransform>().sizeDelta.x - 10;

        timeBar = GameObject.Find("TimeBar").GetComponent<RectTransform>();
        timeBarMaxWidth = GameObject.Find("TimeBarBG").GetComponent<RectTransform>().sizeDelta.x - 10;

        conversationPanel = GameObject.Find("ConversationPanel");
        conversationPanel.SetActive(false);

        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);

        endOfDayPanel = GameObject.Find("EndOfDayPanel");
        endOfDayPanel.SetActive(false);

        settingsPanel = GameObject.Find("SettingsPanel");
        settingsPanel.SetActive(false);

        dayTransitionPanel = GameObject.Find("DayTransitionPanel").GetComponent<Animator>();
        dayTransitionText = GameObject.Find("DayTransitionText").GetComponent<Text>();
        dayTransitionBlurb = GameObject.Find("DayTransitionBlurb").GetComponent<Text>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        StartDayTransition();
    }

    // Update is called once per frame
    void Update()
    {
        timePassedToday += Time.deltaTime;
        UpdateTimeBar();
        if (timePassedToday >= timePerDay)
        {
            Lose();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
                player.canMove = true;
            }
            else
            {
                settingsPanel.SetActive(true);
                player.canMove = false;
            }
        }
    }

    public float GetCurrentTemperature()
    {
        if (day > 0)
        {
            return temperatures[day - 1];
        }
        else
        {
            return 5.0f;
        }
    }

    public Vector3 GetWindDirection()
    {
        return new Vector3(wind[day - 1].x, wind[day - 1].y, 0.0f);
    }

    public void StartNewDay()
    {
        if (day != 1) SceneManager.LoadScene("Day" + day);
        GameObject.Find("DayCounterText").GetComponent<Text>().text = "Day " + day;
        // Play intro cutscene
        // Load the scene for that day
        timePassedToday = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
    }

    public void OnEndDay(List<Item> items)
    {
        // Day 0 is just for initialisation purposes
        if (day != 0)
        {
            endOfDayPanel.SetActive(true);
            // See if daily requirements are fulfilled.
            int logs = 0;
            int meat = 0;
            int toys = 0;
            foreach (Item item in items)
            {
                if (item.itemName == "Log")
                {
                    logs++;
                }
            }
            foreach (Item item in items)
            {
                if (item.itemName == "Rabbit Meat")
                {
                    meat++;
                }
            }
            foreach (Item item in items)
            {
                if (item.itemName == "Toy")
                {
                    toys++;
                }
            }

            Cozyness += (logs - logsNeeded[day - 1]) * 4;
            Cozyness += (meat - meatNeeded[day - 1]) * 3;
            Cozyness += toys * 15;

            GameObject.Find("EndOfDayTitleText").GetComponent<Text>().text = "End of day " + day;
            GameObject.Find("EndOfDayText").GetComponent<Text>().text =
                "Logs " + logs + "/" + logsNeeded[day - 1] + "\n" +
                "Meat " + meat + "/" + meatNeeded[day - 1] + "\n" +
                "Toys " + toys + "/" + toysNeeded[day - 1];

            if (Cozyness <= 0)
            {
                Lose();
            }

            UpdateEndOfDayCozynessBar();
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().OnEndDay();

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = false;

        if (day >= lastDay)
        {
            GameObject.Find("EndOfDayText").GetComponent<Text>().text += "\nYou win";
        }
    }
    public void StartDayTransition()
    {
        // Set up the day transition animation
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = false;
        day++;
        dayTransitionPanel.SetInteger("day", day);
        dayTransitionText.text = dayTransitionTexts[day - 1];
        dayTransitionBlurb.text = dayTransitionBlurbs[day - 1];
        dayTransitionPanel.SetTrigger("animate");
        endOfDayPanel.SetActive(false);
    }

    void UpdateCozynessBar()
    {
        cozynessBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            (float)Cozyness / maxCozyness * cozynessBarMaxWidth
        );
    }
    void UpdateEndOfDayCozynessBar()
    {
        endOfDayCozynessBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            (float)Cozyness / maxCozyness * endOfDayCozynessBarMaxWidth
        );
    }
    public void UpdateWarmnessBar()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        warmnessBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            player.currentWarmness / 60.0f * warmnessBarMaxWidth
        );
    }
    void UpdateTimeBar()
    {
        timeBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            (timePerDay - timePassedToday) / timePerDay * timeBarMaxWidth
        );
    }

    public void TradeWithCharacterCurrentlyBeingSpokenTo()
    {
        GameObject tpGameObject = GameObject.Find("TradePanel");
        if (tpGameObject == null)
        {
            // setup trade
            characterCurrentlyBeingSpokenTo.Trade();
        }
        else
        {
            TradePanel tp = tpGameObject.GetComponent<TradePanel>();
            if (tp.playerCost <= tp.npcCost)
            {
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // perform trade
                foreach (Item item in tp.playerItems)
                {
                    player.inventory.RemoveItem(item);
                    characterCurrentlyBeingSpokenTo.inventory.AddItem(item);
                }
                foreach (Item item in tp.npcItems)
                {
                    characterCurrentlyBeingSpokenTo.inventory.RemoveItem(item);
                    player.inventory.AddItem(item);
                }
                GameObject.Find("ConversationText").GetComponent<Text>().text = "That's a good trade.";
            }
            else
            {
                GameObject.Find("ConversationText").GetComponent<Text>().text = "This is a bad trade.";
            }
        }
        Destroy(tpGameObject);
    }
    void Lose()
    {
        gameOverPanel.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = false;
    }
}
