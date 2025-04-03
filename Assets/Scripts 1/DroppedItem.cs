using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemType Type;
    private Inventory inventory;
    private GameObject item;
    [HideInInspector] public string itemName;
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        switch (Type)
        {
            case ItemType.Sword:
                for (int i = 0; i < GameObject.Find("hand.r").transform.childCount; i++)
                {
                    if (GameObject.Find("hand.r").transform.GetChild(i).gameObject.name == "Sword")
                    {
                        item = GameObject.Find("hand.r").transform.GetChild(i).gameObject;
                    }
                }
                itemName = item.name;
                break;

            case ItemType.Axe:
                for (int i = 0; i < GameObject.Find("hand.r").transform.childCount; i++)
                {
                    if (GameObject.Find("hand.r").transform.GetChild(i).gameObject.name == "Axe")
                    {
                        item = GameObject.Find("hand.r").transform.GetChild(i).gameObject;
                    }
                }
                itemName = item.name;
                break;
        }
    }

    void Update()
    {
        
    }

    public void PickUp()
    {
        inventory.AddItem(item, 1);
        gameObject.SetActive(false);
    }

    public enum ItemType
    {
        Sword,
        Axe
    }
}
