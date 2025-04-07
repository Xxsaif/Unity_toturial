using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/InventoryData")]
public class InventoryData : ScriptableObject
{
    [HideInInspector] public Item[,] inventoryItems = new Item[3, 6];
    [HideInInspector] public Item[] hotbarItems = new Item[10];
}
