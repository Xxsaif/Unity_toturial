using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class InventorySave : MonoBehaviour
{

    [HideInInspector] public int selected_id;
    private bool item_selected;
    [SerializeField] private Animator animator;
    public bool canSwapItem;
    private readonly Color hotbarSlotActive = new(72f / 255f, 72f / 255f, 72f / 255f, 200f / 255f);
    private readonly Color hotbarSlotInactive = new(72f / 255f, 72f / 255f, 72f / 255f, 100f / 255f);
    private bool hotbarActive;
    public GameObject inventoryScreen;
    [HideInInspector] public bool inventoryActive;
    [HideInInspector] public Item[,] inventoryItems = new Item[3, 6];
    [HideInInspector] public Item[] hotbarItems = new Item[10];

    [HideInInspector] public TextMeshProUGUI[,] inventorySlotIcons = new TextMeshProUGUI[3, 6];
    [HideInInspector] public TextMeshProUGUI[,] inventorySlotQuantity = new TextMeshProUGUI[3, 6];

    [HideInInspector] public Image[] hotbarSlotImg;
    [HideInInspector] public TextMeshProUGUI[] hotbarSlotIcons;
    [HideInInspector] public TextMeshProUGUI[] hotbarSlotQuantity;

    private bool canScroll;

    private readonly KeyCode[] numberKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
    void Start()
    {
        hotbarItems = new Item[10];
        hotbarSlotQuantity = new TextMeshProUGUI[10];
        hotbarSlotIcons = new TextMeshProUGUI[10];
        hotbarSlotImg = new Image[10];
        item_selected = false;
        canSwapItem = false;
        hotbarActive = false;
        canScroll = true;
        GameObject hotbar = GameObject.Find("Hotbar");
        for (int i = 0; i < hotbarSlotIcons.Length; i++)
        {
            GameObject slot = hotbar.transform.GetChild(i).gameObject;
            slot.GetComponent<Slot>().slotHotbarPos = i;

            hotbarSlotImg[i] = slot.GetComponent<Image>();
            hotbarSlotIcons[i] = slot.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            hotbarSlotQuantity[i] = slot.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        }
        inventoryScreen.SetActive(true);
        for (int y = 0; y < inventorySlotIcons.GetLength(0); y++)
        {
            for (int x = 0; x < inventorySlotIcons.GetLength(1); x++)
            {
                if (inventoryScreen.transform.GetChild(y).transform.GetChild(x).gameObject != null)
                {
                    GameObject slot = inventoryScreen.transform.GetChild(y).transform.GetChild(x).gameObject;
                    slot.GetComponent<Slot>().slotInventoryPos = (x, y);
                    if (slot.transform.GetChild(0).gameObject != null)
                    {
                        inventorySlotIcons[y, x] = slot.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                    }
                    if (slot.transform.GetChild(1).gameObject != null)
                    {
                        inventorySlotQuantity[y, x] = slot.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }
        inventoryScreen.SetActive(false);
        AddItem(Item.ItemType.Axe, 2);
        AddItem(Item.ItemType.Sword, 2);
        AddItem(Item.ItemType.Medkit, 7);
        AddItem(Item.ItemType.Rock, 24);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
            inventoryActive = inventoryScreen.activeSelf;
            if (inventoryScreen.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            else if (!inventoryScreen.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (canSwapItem && !inventoryActive)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(Scroll(Input.GetKeyDown(KeyCode.Z) ? 1 : -1));
            }

            else if (Input.mouseScrollDelta.y != 0 && canScroll)
            {
                canScroll = false;
                StartCoroutine(Scroll((int)Input.mouseScrollDelta.y));
            }
            else if (Input.anyKeyDown)
            {
                for (int i = 0; i < numberKeys.Length; i++)
                {
                    if (Input.GetKeyDown(numberKeys[i]))
                    {
                        StartCoroutine(Scroll(selected_id - i));
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && !inventoryActive)
        {
            hotbarActive = !hotbarActive;

            if (hotbarActive)
            {
                if (hotbarItems[selected_id] != null)
                {
                    hotbarItems[selected_id].gameObject.SetActive(true);
                    item_selected = true;
                }
                hotbarSlotImg[selected_id].GetComponent<Slot>().image.color = hotbarSlotActive;
                canSwapItem = true;
                UpdateAnimator();
            }
            else if (!hotbarActive)
            {
                if (canSwapItem)
                {
                    if (item_selected)
                    {
                        hotbarItems[selected_id].gameObject.SetActive(false);
                        item_selected = false;
                        animator.SetBool("Item_Equipped", false);
                    }
                    hotbarSlotImg[selected_id].GetComponent<Slot>().image.color = hotbarSlotInactive;
                    canSwapItem = false;
                }
                else
                {
                    hotbarActive = true;
                }
            }
        }



    }

    
    private void UpdateAnimator()
    {
        if (hotbarItems[selected_id] != null)
        {
            animator.SetBool("Item_Equipped", true);
            animator.SetInteger("Item_Type_Id", (int)hotbarItems[selected_id].itemType);
        }

        else
        {
            animator.SetBool("Item_Equipped", false);
        }
    }

    public int AddItem(Item.ItemType itemType, int q)
    {
        int quantity = q;
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] != null && hotbarItems[i].itemType == itemType)
            {
                int originalQuantity = hotbarItems[i].quantity;
                hotbarItems[i].quantity = Mathf.Clamp(hotbarItems[i].quantity + quantity, 1, hotbarItems[i].stackLimit);
                quantity = originalQuantity + quantity - Mathf.Clamp(originalQuantity + quantity, 1, hotbarItems[i].stackLimit);
                hotbarSlotQuantity[i].text = hotbarItems[i].quantity.ToString();
                if (quantity == 0)
                {
                    return 0;
                }
            }
            else if (hotbarItems[i] == null)
            {
                hotbarItems[i] = new Item(itemType, quantity);
                hotbarSlotIcons[i].text = hotbarItems[i].Name;
                quantity -= Mathf.Clamp(quantity, 0, hotbarItems[i].stackLimit);
                hotbarSlotQuantity[i].text = hotbarItems[i].quantity.ToString();
                if (hotbarActive && hotbarItems[selected_id] != null)
                {
                    hotbarItems[selected_id].gameObject.SetActive(true);
                    item_selected = true;
                    UpdateAnimator();
                }
                if (quantity == 0)
                {
                    return 0;
                }
            }
        }

        for (int y = 0; y < inventoryItems.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryItems.GetLength(1); x++)
            {
                if (inventoryItems[y, x] != null && inventoryItems[y, x].itemType == itemType)
                {
                    int originalQuantity = inventoryItems[y, x].quantity;
                    inventoryItems[y, x].quantity = Mathf.Clamp(inventoryItems[y, x].quantity + quantity, 1, inventoryItems[y, x].stackLimit);
                    quantity = originalQuantity + quantity - Mathf.Clamp(originalQuantity + quantity, 1, inventoryItems[y, x].stackLimit);
                    inventorySlotQuantity[y, x].text = inventoryItems[y, x].quantity.ToString();
                    if (quantity == 0)
                    {
                        return 0;
                    }
                }
                if (inventoryItems[y, x] == null)
                {
                    inventoryItems[y, x] = new Item(itemType, quantity);
                    inventorySlotIcons[y, x].text = inventoryItems[y, x].Name;
                    quantity -= Mathf.Clamp(quantity, 1, inventoryItems[y, x].stackLimit);
                    inventorySlotQuantity[y, x].text = inventoryItems[y, x].quantity.ToString();
                    if (quantity == 0)
                    {
                        return 0;
                    }
                }
            }
        }

        return quantity;
    }

    public void MoveItem((int x, int y) fromPos, (int x, int y) toPos, int q)
    {
        int quantity = q;
        quantity = AddingToInventory(inventoryItems[fromPos.y, fromPos.x].itemType, toPos, quantity);
        RemovingFromInventory(fromPos, quantity);
    }

    public void MoveItem(int fromPos, int toPos, int q)
    {
        int quantity = q;
        quantity = AddingToHotbar(hotbarItems[fromPos].itemType, toPos, quantity);
        RemovingFromHotbar(fromPos, quantity);

        if (hotbarActive && toPos == selected_id && hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].gameObject.SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
    }

    public void MoveItem((int x, int y) fromPos, int toPos, int q)
    {
        int quantity = q;
        quantity = AddingToHotbar(inventoryItems[fromPos.y, fromPos.x].itemType, toPos, quantity); ;
        RemovingFromInventory(fromPos, quantity);

        if (hotbarActive && toPos == selected_id && hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].gameObject.SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }

    }

    public void MoveItem(int fromPos, (int x, int y) toPos, int q)
    {
        int quantity = q;
        quantity = AddingToInventory(hotbarItems[fromPos].itemType, toPos, quantity);
        RemovingFromHotbar(fromPos, quantity);

        if (hotbarActive && fromPos == selected_id && hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].gameObject.SetActive(false);
            item_selected = false;
            UpdateAnimator();
        }
    }
    private int AddingToInventory(Item.ItemType fromType, (int x, int y) toPos, int q)
    {
        int quantity = q;
        if (inventoryItems[toPos.y, toPos.x] == null)
        {
            inventoryItems[toPos.y, toPos.x] = new Item(fromType, quantity);
            inventorySlotIcons[toPos.y, toPos.x].text = inventoryItems[toPos.y, toPos.x].Name;
            inventorySlotQuantity[toPos.y, toPos.x].text = inventoryItems[toPos.y, toPos.x].quantity.ToString();
        }
        else if (fromType == inventoryItems[toPos.y, toPos.x].itemType)
        {
            if (inventoryItems[toPos.y, toPos.x].quantity + quantity >= inventoryItems[toPos.y, toPos.x].stackLimit)
            {
                quantity = inventoryItems[toPos.y, toPos.x].stackLimit - inventoryItems[toPos.y, toPos.x].quantity;
            }
            inventoryItems[toPos.y, toPos.x].quantity += quantity;
            inventorySlotQuantity[toPos.y, toPos.x].text = inventoryItems[toPos.y, toPos.x].quantity.ToString();
        }
        return quantity;
    }

    private void RemovingFromInventory((int x, int y) fromPos, int quantity)
    {
        inventoryItems[fromPos.y, fromPos.x].quantity -= quantity;
        if (inventoryItems[fromPos.y, fromPos.x].quantity == 0)
        {
            inventoryItems[fromPos.y, fromPos.x] = null;
            inventorySlotIcons[fromPos.y, fromPos.x].text = string.Empty;
            inventorySlotQuantity[fromPos.y, fromPos.x].text = string.Empty;
        }
        else
        {
            inventorySlotQuantity[fromPos.y, fromPos.x].text = inventoryItems[fromPos.y, fromPos.x].quantity.ToString();
        }
    }
    public int AddingToHotbar(Item.ItemType fromType, int toPos, int q)
    {
        int quantity = q;
        if (hotbarItems[toPos] == null)
        {
            hotbarItems[toPos] = new Item(fromType, quantity);
            hotbarSlotIcons[toPos].text = hotbarItems[toPos].Name;
            hotbarSlotQuantity[toPos].text = hotbarItems[toPos].quantity.ToString();
        }
        else if (fromType == hotbarItems[toPos].itemType)
        {
            if (hotbarItems[toPos].quantity + quantity >= hotbarItems[toPos].stackLimit)
            {
                quantity = hotbarItems[toPos].stackLimit - hotbarItems[toPos].quantity;
            }
            hotbarItems[toPos].quantity += quantity;
            hotbarSlotQuantity[toPos].text = hotbarItems[toPos].quantity.ToString();
        }
        return quantity;
    }

    public void RemovingFromHotbar(int fromPos, int quantity)
    {
        hotbarItems[fromPos].quantity -= quantity;
        if (hotbarItems[fromPos].quantity == 0)
        {
            RemoveHotbarItem(fromPos);
        }
        else
        {
            hotbarSlotQuantity[fromPos].text = hotbarItems[fromPos].quantity.ToString();
        }
    }
    private void RemoveHotbarItem(int fromPos)
    {
        if (fromPos == selected_id)
        {
            hotbarItems[selected_id].gameObject.SetActive(false);
        }
        hotbarItems[fromPos] = null;
        hotbarSlotIcons[fromPos].text = string.Empty;
        hotbarSlotQuantity[fromPos].text = string.Empty;
        if (hotbarActive && fromPos == selected_id)
        {
            item_selected = false;
            UpdateAnimator();
        }
    }
    IEnumerator Scroll(int direction)
    {
        if (item_selected)
        {
            hotbarItems[selected_id].gameObject.SetActive(false);
        }
        hotbarSlotImg[selected_id].color = hotbarSlotInactive;

        selected_id -= direction;
        selected_id = selected_id > 9 ? selected_id - 10 : selected_id;
        selected_id = selected_id < 0 ? selected_id + 10 : selected_id;

        hotbarSlotImg[selected_id].color = hotbarSlotActive;
        if (hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].gameObject.SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
        else
        {
            item_selected = false;
            animator.SetBool("Item_Equipped", false);
        }

        yield return new WaitForSeconds(0.025f);
        canScroll = true;
    }
}
