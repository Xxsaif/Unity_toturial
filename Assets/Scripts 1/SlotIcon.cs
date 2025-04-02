using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotIcon : MonoBehaviour
{
    private Vector3 startPos;
    [HideInInspector] public bool isBeingDragged;
    private SlotManager slotManager;
    private (int x, int y) inventorySlotPos = (-1, -1);
    private int hotbarSlotPos = -1;
    [HideInInspector] public Slot.SlotType type;
    
    void Start()
    {
        startPos = transform.localPosition;
        slotManager = GameObject.Find("Inventory").GetComponent<SlotManager>();
        type = gameObject.transform.parent.gameObject.GetComponent<Slot>().type;
        
    }

    public void Drag()
    {
        if (inventorySlotPos == (-1, -1) && type == Slot.SlotType.Inventory)
        {
            inventorySlotPos = slotManager.FindInventorySlotPosition(gameObject.transform.parent.gameObject);
        }
        else if (hotbarSlotPos == -1 && type == Slot.SlotType.Hotbar)
        {
            hotbarSlotPos = slotManager.FindHotbarSlotPosition(gameObject.transform.parent.gameObject);
        }

        if (type == Slot.SlotType.Inventory && slotManager.inventoryScr.inventoryItems[inventorySlotPos.y, inventorySlotPos.x] != null)
        {
            transform.position = Input.mousePosition;
            isBeingDragged = true;
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = false;
        }
        else if (type == Slot.SlotType.Hotbar && slotManager.inventoryScr.hotbarItems[hotbarSlotPos] != null)
        {
            transform.position = Input.mousePosition;
            isBeingDragged = true;
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = false;
        }
        
    }

    public void EndDrag()
    {
        if (type == Slot.SlotType.Inventory && slotManager.inventoryScr.inventoryItems[inventorySlotPos.y, inventorySlotPos.x] != null)
        {
            transform.localPosition = startPos;
            isBeingDragged = false;
            slotManager.TryMoveItem(gameObject.transform.parent.gameObject, type);
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = true;
        }
        else if (type == Slot.SlotType.Hotbar && slotManager.inventoryScr.hotbarItems[hotbarSlotPos] != null)
        {
            transform.localPosition = startPos;
            isBeingDragged = false;
            slotManager.TryMoveItem(gameObject.transform.parent.gameObject, type);
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = true;
        }
    }
    void Update()
    {
        
    }

    
}
