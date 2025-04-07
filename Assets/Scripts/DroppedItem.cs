using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private Inventory inventory;
    public Item item;
    public Item.ItemType type;
    public int quantity;
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        item = new Item(type, quantity);
    }

    void Update()
    {
        
    }

    public void PickUp()
    {
        inventory.AddItem(type, quantity);
        gameObject.SetActive(false);
    }
}
