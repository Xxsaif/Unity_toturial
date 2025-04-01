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

    public void TryMoveItem(GameObject caller)
    {
        for (int y = 0; y < inventoryScr.inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryScr.inventorySlots.GetLength(1); x++)
            {
                if (inventoryScr.inventorySlots[y, x].GetComponent<Slot>().hovered && inventoryScr.inventorySlots[y, x] != caller)
                {
                    (int x, int y) fromPos = FindSlotPosition(caller);
                    inventoryScr.MoveItem((fromPos.x, fromPos.y), (x, y));
                    return;
                }
            }
        }
    }

    public (int x, int y) FindSlotPosition(GameObject obj)
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
}
