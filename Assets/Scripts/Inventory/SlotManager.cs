using System.Xml.Serialization;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [HideInInspector] public InventorySave inventoryScr;
    [HideInInspector] public static GameObject hoveredSlot;
    
    void Start()
    {
        inventoryScr = GameObject.Find("Player").GetComponent<InventorySave>();
        
    }

    
    void Update()
    {
        
    }

    public void TryMoveItem(GameObject caller, Slot.SlotType type, int quantity)
    {

        if (hoveredSlot != null)
        {
            Slot hoveredSlotScr = hoveredSlot.GetComponent<Slot>();
            Slot callerSlotScr = caller.GetComponent<Slot>();
            if (hoveredSlotScr.hovered && hoveredSlot != caller)
            {
                switch (type)
                {
                    case Slot.SlotType.Inventory:
                        switch (hoveredSlotScr.type)
                        {
                            case Slot.SlotType.Inventory:
                                if ((inventoryScr.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x] == null || inventoryScr.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x].itemType == inventoryScr.inventoryItems[callerSlotScr.slotInventoryPos.y, callerSlotScr.slotInventoryPos.x].itemType))
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotInventoryPos, hoveredSlotScr.slotInventoryPos, quantity);
                                }
                                break;

                            case Slot.SlotType.Hotbar:
                                if ((inventoryScr.hotbarItems[hoveredSlotScr.slotHotbarPos] == null || inventoryScr.hotbarItems[hoveredSlotScr.slotHotbarPos].itemType == inventoryScr.inventoryItems[callerSlotScr.slotInventoryPos.y, callerSlotScr.slotInventoryPos.x].itemType))
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotInventoryPos, hoveredSlotScr.slotHotbarPos, quantity);
                                }
                                break;
                        }
                        break;

                    case Slot.SlotType.Hotbar:
                        switch (hoveredSlotScr.type)
                        {
                            case Slot.SlotType.Inventory:
                                if ((inventoryScr.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x] == null || inventoryScr.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x].itemType == inventoryScr.hotbarItems[callerSlotScr.slotHotbarPos].itemType))
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotHotbarPos, hoveredSlotScr.slotInventoryPos, quantity);
                                }
                                break;

                            case Slot.SlotType.Hotbar:
                                if ((inventoryScr.hotbarItems[hoveredSlotScr.slotHotbarPos] == null || inventoryScr.hotbarItems[hoveredSlotScr.slotHotbarPos].itemType == inventoryScr.hotbarItems[callerSlotScr.slotHotbarPos].itemType))
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotHotbarPos, hoveredSlotScr.slotHotbarPos, quantity);
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }
}
