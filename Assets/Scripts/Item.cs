using UnityEngine;
using static UnityEditor.Progress;

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
        SelectData(itemType, ref gameObject, ref stackLimit, ref itemName);
        this.quantity = Mathf.Clamp(quantity, 1, stackLimit);
    }

    private static void SelectData(ItemType itemType, ref GameObject gameObject, ref int stackLimit, ref string itemName)
    {
        switch (itemType)
        {
            case ItemType.Sword:
                gameObject = GetGameObject("Sword");
                stackLimit = 32;
                itemName = "Sword";
                break;

            case ItemType.Axe:
                gameObject = GetGameObject("Axe");
                stackLimit = 32;
                itemName = "Axe";
                break;
        }
    }

    private static GameObject GetGameObject(string name)
    {
        for (int i = 0; i < GameObject.Find("hand.r").transform.childCount; i++)
        {
            if (GameObject.Find("hand.r").transform.GetChild(i).gameObject.name == name)
            {
                return GameObject.Find("hand.r").transform.GetChild(i).gameObject;
            }
        }
        return null;
    }
    public bool Full() => quantity == stackLimit;
    public enum ItemType
    {
        Sword,
        Axe
    }
}
