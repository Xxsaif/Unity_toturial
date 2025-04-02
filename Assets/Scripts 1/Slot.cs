using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector] public bool hovered;
    public SlotType type;
    [SerializeField] private Inventory inventoryScr;
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
    }

    public void UnHover()
    {
        hovered = false;
    }
    
    
    public enum SlotType
    {
        Inventory,
        Hotbar
    }
}
