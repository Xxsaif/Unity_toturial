using UnityEngine;

public class Item
{
    public readonly GameObject gameObject;
    public int quantity;
    public readonly int stackLimit;
    private readonly string itemName;
    public string Name { get { return itemName; } }
    public readonly ItemType itemType;

    public Item(ItemType itemType, int quantity)
    {
        this.itemType = itemType;
        ItemManager.SelectData(itemType, ref gameObject, ref stackLimit, ref itemName);
        this.quantity = Mathf.Clamp(quantity, 1, stackLimit);
    }

    public bool Full() => quantity == stackLimit;
    public enum ItemType
    {
        Sword,
        Axe,
        Medkit,
        Rock,
        Stick,
        String
    }
}
