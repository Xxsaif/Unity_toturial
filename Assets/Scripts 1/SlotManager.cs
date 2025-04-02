using System.Xml.Serialization;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [HideInInspector] public Inventory inventoryScr;
    
    void Start()
    {
        inventoryScr = GameObject.Find("Player").GetComponent<Inventory>();
        
    }

    
    void Update()
    {
        
    }

    public void TryMoveItem(GameObject caller, Slot.SlotType type)
    {
        switch (type)
        {
            case Slot.SlotType.Inventory:
                for (int y = 0; y < inventoryScr.inventorySlots.GetLength(0); y++)
                {
                    for (int x = 0; x < inventoryScr.inventorySlots.GetLength(1); x++)
                    {
                        if (inventoryScr.inventorySlots[y, x].GetComponent<Slot>().hovered && inventoryScr.inventorySlots[y, x] != caller && inventoryScr.inventoryItems[y, x] == null)
                        {
                            (int x, int y) fromPos = FindInventorySlotPosition(caller);
                            inventoryScr.MoveItem(fromPos, (x, y));
                            return;
                        }
                    }
                }

                for (int i = 0; i < inventoryScr.hotbarSlots.GetLength(0); i++)
                {

                    if (inventoryScr.hotbarSlots[i].GetComponent<Slot>().hovered && inventoryScr.hotbarSlots[i] != caller && inventoryScr.hotbarItems[i] == null)
                    {
                        (int x, int y) fromPos = FindInventorySlotPosition(caller);
                        inventoryScr.MoveItem(fromPos, i);
                        return;
                    }

                }
                break;

            case Slot.SlotType.Hotbar:
                for (int i = 0; i < inventoryScr.hotbarSlots.GetLength(0); i++)
                {
                    
                    if (inventoryScr.hotbarSlots[i].GetComponent<Slot>().hovered && inventoryScr.hotbarSlots[i] != caller && inventoryScr.hotbarItems[i] == null)
                    {
                        int fromPos = FindHotbarSlotPosition(caller);
                        inventoryScr.MoveItem(fromPos, i);
                        return;
                    }
                    
                }

                for (int y = 0; y < inventoryScr.inventorySlots.GetLength(0); y++)
                {
                    for (int x = 0; x < inventoryScr.inventorySlots.GetLength(1); x++)
                    {
                        if (inventoryScr.inventorySlots[y, x].GetComponent<Slot>().hovered && inventoryScr.inventorySlots[y, x] != caller && inventoryScr.inventoryItems[y, x] == null)
                        {
                            int fromPos = FindHotbarSlotPosition(caller);
                            inventoryScr.MoveItem(fromPos, (x, y));
                            return;
                        }
                    }
                }
                break;
        }
    }

    

    public (int x, int y) FindInventorySlotPosition(GameObject obj)
    {
        for (int y = 0; y < inventoryScr.inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryScr.inventorySlots.GetLength(1); x++)
            {
                if (inventoryScr.inventorySlots[y, x] == obj)
                {
                    return (x, y);
                }
            }
        }
        return (0, 0);
    }

    public int FindHotbarSlotPosition(GameObject obj)
    {
        for (int i = 0; i < inventoryScr.hotbarSlots.GetLength(0); i++)
        {
            if (inventoryScr.hotbarSlots[i] == obj)
            {
                return i;
            }
        }
       
        return 0;
    }
}
