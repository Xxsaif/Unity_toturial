using TMPro;
using UnityEngine;

public class DroppedItem : MonoBehaviour, InteractableObject
{
    private InventorySave inventory;
    public Item item;
    public Item.ItemType type;
    public int quantity;
    [SerializeField] private TextMeshProUGUI interactionText;
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<InventorySave>();
        item = new Item(type, quantity);
    }

    void Update()
    {
        
    }
    public void InteractRangeEnter()
    {
        interactionText.text = "Press F to\nPick up " + item.Name;
    }

    public void InteractRangeStay()
    {
        interactionText.text = "Press F to\nPick up " + item.Name;
    }

    public void InteractRangeExit()
    {
        interactionText.text = string.Empty;
    }

    public void Interact()
    {
        inventory.AddItem(type, quantity);
        gameObject.SetActive(false);
        interactionText.text = string.Empty;
    }
    
}
