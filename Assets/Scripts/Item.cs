using UnityEngine;

/// <summary>
/// Something that fits in the inventory
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName="Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public int cost;
    public Sprite sprite;
    public GameObject itemPickupPrefab;
}
