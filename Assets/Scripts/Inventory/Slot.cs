using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Created by Herman Bergström
public class Slot : MonoBehaviour
{
    [HideInInspector] public bool hovered;
    public SlotType type;
    [HideInInspector] public Image slotImg;
    public TextMeshProUGUI slotIcon;
    public TextMeshProUGUI slotQuantity;
    [HideInInspector] public int slotInventoryPos;
    [HideInInspector] public Inventory inventory;
    public Image image;
    void Start()
    {
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
