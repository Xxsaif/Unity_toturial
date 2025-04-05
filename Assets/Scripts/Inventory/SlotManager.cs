using System.Xml.Serialization;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [HideInInspector] public Inventory inventoryScr;
    [HideInInspector] public static GameObject hoveredSlot;
    
    void Start()
    {
        inventoryScr = GameObject.Find("Player").GetComponent<Inventory>();
        
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
                                if ((inventoryScr.inventoryData.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x] == null || inventoryScr.inventoryData.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x].itemType == inventoryScr.inventoryData.inventoryItems[callerSlotScr.slotInventoryPos.y, callerSlotScr.slotInventoryPos.x].itemType))
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotInventoryPos, hoveredSlotScr.slotInventoryPos, quantity);
                                }
                                break;

                            case Slot.SlotType.Hotbar:
                                if ((inventoryScr.inventoryData.hotbarItems[hoveredSlotScr.slotHotbarPos] == null || inventoryScr.inventoryData.hotbarItems[hoveredSlotScr.slotHotbarPos].itemType == inventoryScr.inventoryData.inventoryItems[callerSlotScr.slotInventoryPos.y, callerSlotScr.slotInventoryPos.x].itemType))
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
                                if ((inventoryScr.inventoryData.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x] == null || inventoryScr.inventoryData.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x].itemType == inventoryScr.inventoryData.hotbarItems[callerSlotScr.slotHotbarPos].itemType))
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotHotbarPos, hoveredSlotScr.slotInventoryPos, quantity);
                                }
                                break;

                            case Slot.SlotType.Hotbar:
                                if ((inventoryScr.inventoryData.hotbarItems[hoveredSlotScr.slotHotbarPos] == null || inventoryScr.inventoryData.hotbarItems[hoveredSlotScr.slotHotbarPos].itemType == inventoryScr.inventoryData.hotbarItems[callerSlotScr.slotHotbarPos].itemType))
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
