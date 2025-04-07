using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector] public bool hovered;
    public SlotType type;
    [SerializeField] private Inventory inventoryScr;
    [HideInInspector] public (int x, int y) slotInventoryPos;
    [HideInInspector] public int slotHotbarPos;
    public Image image;
    void Start()
    {

    }

    
    void Update()
    {
        
    }

    public void Hover()
    {
        hovered = true;
        SlotManager.hoveredSlot = gameObject;
    }

    public void UnHover()
    {
        hovered = false;
        SlotManager.hoveredSlot = null;
    }
    
    
    public enum SlotType
    {
        Inventory,
        Hotbar
    }
}
