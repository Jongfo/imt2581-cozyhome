using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;

    public float InteractionDistance = 7.25f;
    public float InteractionWidth = 7.2f;
    public LayerMask InteractionLayer;

    public Inventory inventory;

    public bool canMove = true;

    struct Clothes
    {
        public Clothing head;
        public Clothing neck;
        public Clothing torso;
        public Clothing hands;
        public Clothing legs;
        public Clothing feet;
    }

    Clothes clothes; // The currently worn clothes

    public float currentWarmness = 30.0f;

    //datatype som sier hvilken retning spilleren peker
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
    //variabel som inneholder datatypen ovenfor
    public Direction Dir;

    private void Awake()
    {
        inventory = gameObject.AddComponent<Inventory>();
        inventory.belongsToPlayer = true;
        GetComponent<BowAndArrow>().equipped = true;
        GameObject.Find("BowAndArrowEquipmentButton").GetComponent<Image>().color = Color.green;
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Vector3 wind = GameManager.instance.GetWindDirection();
            Vector3 movementDirection = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"),
                0.0f
            ).normalized;
            Vector3 movementVector = movementDirection;
            if (Mathf.Sign(movementDirection.x) == -Mathf.Sign(wind.x))
            {
                movementVector.x += wind.x;
            }
            else
            {
                movementVector.x += wind.x / 4.0f;
            }
            if (Mathf.Sign(movementDirection.y) == -Mathf.Sign(wind.y))
            {
                movementVector.y += wind.y;
            }
            else
            {
                movementVector.y += wind.y / 4.0f;
            }

            transform.position += movementVector * speed * Time.deltaTime;

            // If we are talking to someone and we move far enough away,
            // we should stop talking
            if (GameManager.instance.characterCurrentlyBeingSpokenTo != null)
            {
                if (Vector3.Distance(
                        GameManager.instance.characterCurrentlyBeingSpokenTo.transform.position,
                        transform.position
                    ) > 2.0f)
                {
                    GameManager.instance.characterCurrentlyBeingSpokenTo = null;
                    GameManager.instance.conversationPanel.SetActive(false);
                    Destroy(GameObject.Find("TradePanel"));
                }
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                //GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                Dir = Direction.Up;
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                //GetComponent<Rigidbody2D>().AddForce(-Vector2.up * speed);
                Dir = Direction.Down;
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                //GetComponent<Rigidbody2D>().AddForce(-Vector2.right * speed);
                Dir = Direction.Left;
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                //GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
                Dir = Direction.Right;
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Submit"))
            {
                CheckForInteractions();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwapEquipment();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                inventory.MarkNextItem();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                inventory.DropMarkedItem();
            }

            // Effects of temperature
            float temperature = GameManager.instance.GetCurrentTemperature();
            int clothesWarmness = 0;
            if (clothes.head  != null) clothesWarmness += clothes.head.warmness;
            if (clothes.neck  != null) clothesWarmness += clothes.neck.warmness;
            if (clothes.torso != null) clothesWarmness += clothes.torso.warmness;
            if (clothes.hands != null) clothesWarmness += clothes.hands.warmness;
            if (clothes.legs  != null) clothesWarmness += clothes.legs.warmness;
            if (clothes.feet  != null) clothesWarmness += clothes.feet.warmness;

            //currentWarmness -= (Time.deltaTime * (5 - temperature) - (clothesWarmness / 30.0f)) / 60.0f;
            //GameManager.instance.UpdateWarmnessBar();
        }
    }

    void SwapEquipment()
    {
        if (GetComponent<BowAndArrow>().equipped)
        {
            GetComponent<Axe>().equipped = true;
            GetComponent<BowAndArrow>().equipped = false;
            GameObject.Find("AxeEquipmentButton").GetComponent<Image>().color = Color.green;
            GameObject.Find("BowAndArrowEquipmentButton").GetComponent<Image>().color = Color.white;
        }
        else if (GetComponent<Axe>().equipped)
        {
            GetComponent<BowAndArrow>().equipped = true;
            GetComponent<Axe>().equipped = false;
            GameObject.Find("AxeEquipmentButton").GetComponent<Image>().color = Color.white;
            GameObject.Find("BowAndArrowEquipmentButton").GetComponent<Image>().color = Color.green;
        }
    }
    
    /// <summary>
    /// Puts on an article of clothing
    /// </summary>
    void PutOnClothing(Clothing clothing)
    {
        switch (clothing.slot)
        {
            case ClothingSlot.Head:
                clothes.head = clothing;
                break;
                
            case ClothingSlot.Neck:
                clothes.neck = clothing;
                break;
            case ClothingSlot.Torso:
                clothes.torso= clothing;
                break;
            case ClothingSlot.Hands:
                clothes.hands= clothing;
                break;
            case ClothingSlot.Legs:
                clothes.legs = clothing;
                break;
            case ClothingSlot.Feet:
                clothes.feet= clothing;
                break;
        }
    }

    //funksjon som leter etter colliders forran spilleren basert på retning man peker
    void CheckForInteractions()
    {
        //Lager to Vector2 objekter som skal si hvor "hjørenene" på firkanten er
        //Firkanten brukes til å overlappe colliders, og dermed finne det som overlappes
        Vector2 p1 = new Vector2();
        Vector2 p2 = new Vector2();
        Vector2 pos = transform.position;
        //switch kjører forskjellige caser basert på verdien til "Dir"
        switch (Dir)
        {
            //hvis "Dir" = Down
            case Direction.Down:
                p1.Set(pos.x - InteractionWidth, pos.y);
                p2.Set(pos.x + InteractionWidth, pos.y - InteractionDistance);
                break;
            //osv..
            case Direction.Left:
                p1.Set(pos.x - InteractionDistance, pos.y + InteractionWidth);
                p2.Set(pos.x, pos.y - InteractionWidth);
                break;
            case Direction.Right:
                p1.Set(pos.x, pos.y + InteractionWidth);
                p2.Set(pos.x + InteractionDistance, pos.y - InteractionWidth);
                break;
            case Direction.Up:
                p1.Set(pos.x - InteractionWidth, pos.y + InteractionDistance);
                p2.Set(pos.x + InteractionWidth, pos.y);
                break;

        }

        //Overlapper colliders med firkanten vår, og lagrer det i en Collider2D[] liste hvis de er i "InteractionLayer"
        Collider2D[] cols = Physics2D.OverlapAreaAll(p1, p2, InteractionLayer);
        //går gjennom listen og sjekker om colliderene har en komponent av typen IInteractable
        // hvis den har det så kan vi Interacte.
        for (int i = 0; i < cols.Length; i++)
        {
            IInteractable ir = cols[i].GetComponent(typeof(IInteractable)) as IInteractable;
            ItemPickup item = cols[i].GetComponent<ItemPickup>();
            NonPlayerCharacter nonPlayerCharacter = cols[i].GetComponent<NonPlayerCharacter>();
            Debug.Log("Found collider" + cols[i].name);
            if (ir != null)
            {
                ir.Interact(this);
            }

            if (item != null)
            {
                Debug.Log("Found item");
                if (inventory.items.Count < inventory.maxItems)
                {
                    inventory.AddItem(item.item);
                    Destroy(cols[i].gameObject);
                }
            }

            if (cols[i].tag == "Home")
            {
                Debug.Log("Home");
                GameManager.instance.OnEndDay(inventory.items);
            }

            if (nonPlayerCharacter != null)
            {
                nonPlayerCharacter.Talk();
            }
        }

    }
}
