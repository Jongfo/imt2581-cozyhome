using UnityEngine;

public enum ClothingSlot
{
    Head, Neck, Torso, Hands, Legs, Feet
}

[CreateAssetMenu(fileName = "New Clothing", menuName="Inventory/Clothing")]
public class Clothing : Item
{
    public int warmness;
    public ClothingSlot slot;
    int durability = 100;
}
