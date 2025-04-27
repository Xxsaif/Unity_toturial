using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector] public bool hovered;
    public SlotType type;
    [HideInInspector] public Image slotImg;
    [HideInInspector] public TextMeshProUGUI slotIcon;
    [HideInInspector] public TextMeshProUGUI slotQuantity;
    [HideInInspector] public (int x, int y) slotInventoryPos;
    [HideInInspector] public int slotHotbarPos;
    public Image image;
    void Start()
    {
        slotIcon = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        slotQuantity = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        slotImg = GetComponent<Image>();
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
