using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Created by Herman Bergström
public class InventorySystem : MonoBehaviour
{
    public void MoveItem(Inventory fromInv, Inventory toInv, int fromPos, int toPos, int q)
    {
        int quantity = q;
        quantity = toInv.AddItemTo(fromInv.inventoryItems[fromPos].itemType, toPos, quantity);
        fromInv.RemoveFrom(fromPos, quantity);
    }

}
