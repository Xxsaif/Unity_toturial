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

    public void TryMoveItem(GameObject caller, Slot.SlotType type)
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
                                if (inventoryScr.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x] == null)
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotInventoryPos, hoveredSlotScr.slotInventoryPos);
                                }
                                break;

                            case Slot.SlotType.Hotbar:
                                if (inventoryScr.hotbarItems[hoveredSlotScr.slotHotbarPos] == null)
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotInventoryPos, hoveredSlotScr.slotHotbarPos);
                                }
                                break;
                        }
                        break;

                    case Slot.SlotType.Hotbar:
                        switch (hoveredSlotScr.type)
                        {
                            case Slot.SlotType.Inventory:
                                if (inventoryScr.inventoryItems[hoveredSlotScr.slotInventoryPos.y, hoveredSlotScr.slotInventoryPos.x] == null)
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotHotbarPos, hoveredSlotScr.slotInventoryPos);
                                }
                                break;

                            case Slot.SlotType.Hotbar:
                                if (inventoryScr.hotbarItems[hoveredSlotScr.slotHotbarPos] == null)
                                {
                                    inventoryScr.MoveItem(callerSlotScr.slotHotbarPos, hoveredSlotScr.slotHotbarPos);
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }
}
