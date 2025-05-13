using System.Xml.Serialization;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [HideInInspector] public InventorySystem inventorySystem;
    [HideInInspector] public static GameObject hoveredSlot;
    
    void Start()
    {
        inventorySystem = GameObject.Find("Player").GetComponent<InventorySystem>();
        
    }

    
    void Update()
    {
        
    }

    public void TryMoveItem(GameObject caller, int quantity)
    {
        if (hoveredSlot != null)
        {
            Slot hoveredSlotScr = hoveredSlot.GetComponent<Slot>();
            Slot callerSlotScr = caller.GetComponent<Slot>();
            //Debug.Log("hovered: " + (hoveredSlotScr == null) + ", caller: " + (callerSlotScr == null));
            if (hoveredSlotScr.hovered && hoveredSlot != caller && (hoveredSlotScr.inventory.inventoryItems[hoveredSlotScr.slotInventoryPos] == null || hoveredSlotScr.inventory.inventoryItems[hoveredSlotScr.slotInventoryPos].itemType == callerSlotScr.inventory.inventoryItems[callerSlotScr.slotInventoryPos].itemType))
            {
                //Debug.Log(callerSlotScr.inventory.gameObject.name + " -> " + hoveredSlotScr.inventory.gameObject.name + " | " + quantity + " | " + callerSlotScr.slotInventoryPos + " -> " + hoveredSlotScr.slotInventoryPos);
                inventorySystem.MoveItem(callerSlotScr.inventory, hoveredSlotScr.inventory, callerSlotScr.slotInventoryPos, hoveredSlotScr.slotInventoryPos, quantity);
            }
        }
    }
}
