using TMPro;
using UnityEngine;

public class DroppedItem : MonoBehaviour, InteractableObject
{
    private Inventory inventory;
    public Item item;
    public Item.ItemType type;
    public int quantity;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject[] objects;
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        item = new Item(type, quantity);
        objects[(int)type].SetActive(true);
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
        interactionText.text = string.Empty;
        gameObject.SetActive(false);
    }
    
}
