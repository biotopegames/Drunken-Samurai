using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
        public int id;

    public Sprite icon;
    public ItemType type;
    public int maxStack;
    public bool isStackable;
    public bool isEquipable;
    public bool isCraftable;
    public int sellPrice;
    public GameObject prefab;
    public bool isEquipped; // Flag to indicate if the item is equipped

    // Add any other properties or methods relevant to your game
}

public enum ItemType
{
    Equipment,
    Material
}
