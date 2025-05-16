using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Created by Herman Bergström
public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryScreen;
    [HideInInspector] public Item[] inventoryItems;
    public Slot[] inventorySlots;
    [HideInInspector] public bool inventoryActive;


    void Start()
    {
        inventoryItems = new Item[inventorySlots.Length];
        for (int i = 0;  i < inventorySlots.Length; i++)
        {
            inventorySlots[i].inventory = this;
            inventorySlots[i].slotInventoryPos = i;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Player")
        {
            ChangeActiveState();
        }
    }

    public void ChangeActiveState()
    {
        if (inventoryActive == InventorySystem.inventoryActive && !Weapon.playerAttacking && !PauseMenu.paused)
        {
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
            inventoryActive = inventoryScreen.activeSelf;
            InventorySystem.inventoryActive = inventoryActive;
            if (inventoryScreen.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            else if (!inventoryScreen.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public virtual int AddItem(Item.ItemType itemType, int q)
    {
        int quantity = q;
        
        for (int y = 0; y < inventoryItems.Length; y++)
        {

            if (inventoryItems[y] != null && inventoryItems[y].itemType == itemType)
            {
                int originalQuantity = inventoryItems[y].quantity;
                inventoryItems[y].quantity = Mathf.Clamp(inventoryItems[y].quantity + quantity, 1, inventoryItems[y].stackLimit);
                quantity = originalQuantity + quantity - Mathf.Clamp(originalQuantity + quantity, 1, inventoryItems[y].stackLimit);
                inventorySlots[y].slotQuantity.text = inventoryItems[y].quantity.ToString();
                if (quantity == 0)
                {
                    return 0;
                }
            }
            if (inventoryItems[y] == null)
            {
                inventoryItems[y] = new Item(itemType, quantity);
                inventorySlots[y].slotIcon.text = inventoryItems[y].Name;
                quantity -= Mathf.Clamp(quantity, 1, inventoryItems[y].stackLimit);
                inventorySlots[y].slotQuantity.text = inventoryItems[y].quantity.ToString();
                if (quantity == 0)
                {
                    return 0;
                }
            }
            
        }

        return quantity;
    }

    public virtual int AddItemTo(Item.ItemType fromType, int toPos, int q)
    {
        int quantity = q;
        if (inventoryItems[toPos] == null)
        {
            inventoryItems[toPos] = new Item(fromType, quantity);
            inventorySlots[toPos].slotIcon.text = inventoryItems[toPos].Name;
            inventorySlots[toPos].slotQuantity.text = inventoryItems[toPos].quantity.ToString();
        }
        else if (fromType == inventoryItems[toPos].itemType)
        {
            if (inventoryItems[toPos].quantity + quantity >= inventoryItems[toPos].stackLimit)
            {
                quantity = inventoryItems[toPos].stackLimit - inventoryItems[toPos].quantity;
            }
            inventoryItems[toPos].quantity += quantity;
            inventorySlots[toPos].slotQuantity.text = inventoryItems[toPos].quantity.ToString();
        }
        return quantity;
    }

    public virtual void RemoveFrom(int fromPos, int quantity)
    {
        inventoryItems[fromPos].quantity -= quantity;
        if (inventoryItems[fromPos].quantity == 0)
        {
            inventoryItems[fromPos] = null;
            inventorySlots[fromPos].slotIcon.text = string.Empty;
            inventorySlots[fromPos].slotQuantity.text = string.Empty;
        }
        else
        {
            inventorySlots[fromPos].slotQuantity.text = inventoryItems[fromPos].quantity.ToString();
        }
    }

    public bool ContainsType(Item.ItemType type)
    {
        foreach (Item item in inventoryItems)
        {
            if (item != null && item.itemType == type)
            {
                return true;
            }
        }
        return false;
    }
}
