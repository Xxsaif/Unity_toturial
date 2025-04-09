using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Inventory : MonoBehaviour
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

    [HideInInspector] public TextMeshProUGUI[,] inventorySlotIcons = new TextMeshProUGUI[3, 6];
    [HideInInspector] public TextMeshProUGUI[,] inventorySlotQuantity = new TextMeshProUGUI[3, 6];

    [HideInInspector] public Image[] hotbarSlotImg;
    [HideInInspector] public TextMeshProUGUI[] hotbarSlotIcons;
    [HideInInspector] public TextMeshProUGUI[] hotbarSlotQuantity;

    public InventoryData inventoryData;

    [SerializeField] private TextMeshProUGUI interactionText;
    private bool canScroll;

    private readonly KeyCode[] numberKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
    void Start()
    {
        inventoryData.hotbarItems = new Item[10];
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
        AddItem(Item.ItemType.Axe, 48);
        AddItem(Item.ItemType.Sword, 48);
        AddItem(Item.ItemType.Medkit, 48);
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
                if (inventoryData.hotbarItems[selected_id] != null)
                {
                    inventoryData.hotbarItems[selected_id].gameObject.SetActive(true);
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
                        inventoryData.hotbarItems[selected_id].gameObject.SetActive(false);
                        item_selected = false;
                        animator.SetBool("Sword_Equipped", false);
                        animator.SetBool("Axe_Equipped", false);
                        animator.SetBool("Medkit_Equipped", false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DroppedItem>(out _))
        {
            interactionText.text = "Press F to\nPick up " + other.GetComponent<DroppedItem>().item.Name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<DroppedItem>(out _))
        {
            interactionText.text = string.Empty;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<DroppedItem>(out _))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.GetComponent<DroppedItem>().PickUp();
                interactionText.text = string.Empty;
            }
        }
    }
    private void UpdateAnimator()
    {
        if (inventoryData.hotbarItems[selected_id] != null)
        {
            switch (inventoryData.hotbarItems[selected_id].itemType)
            {
                case Item.ItemType.Sword:
                    animator.SetBool("Sword_Equipped", true);
                    animator.SetBool("Axe_Equipped", false);
                    animator.SetBool("Medkit_Equipped", false);
                    break;

                case Item.ItemType.Axe:
                    animator.SetBool("Axe_Equipped", true);
                    animator.SetBool("Sword_Equipped", false);
                    animator.SetBool("Medkit_Equipped", false);
                    break;

                case Item.ItemType.Medkit:
                    animator.SetBool("Medkit_Equipped", true);
                    animator.SetBool("Axe_Equipped", false);
                    animator.SetBool("Sword_Equipped", false);
                    break;
            }
        }
        
        else
        {
            animator.SetBool("Axe_Equipped", false);
            animator.SetBool("Sword_Equipped", false);
            animator.SetBool("Medkit_Equipped", false);
        }
    }

    public void AddItem(Item.ItemType itemType, int q)
    {
        int quantity = q;
        for (int i = 0; i < inventoryData.hotbarItems.Length; i++)
        {
            if (inventoryData.hotbarItems[i] != null && inventoryData.hotbarItems[i].itemType == itemType)
            {
                int originalQuantity = inventoryData.hotbarItems[i].quantity;
                inventoryData.hotbarItems[i].quantity = Mathf.Clamp(inventoryData.hotbarItems[i].quantity + quantity, 1, inventoryData.hotbarItems[i].stackLimit);
                quantity = originalQuantity + quantity - Mathf.Clamp(originalQuantity + quantity, 1, inventoryData.hotbarItems[i].stackLimit);
                hotbarSlotQuantity[i].text = inventoryData.hotbarItems[i].quantity.ToString();
                if (quantity == 0)
                {
                    return;
                }
            }
            else if (inventoryData.hotbarItems[i] == null)
            {
                inventoryData.hotbarItems[i] = new Item(itemType, quantity);
                hotbarSlotIcons[i].text = inventoryData.hotbarItems[i].Name;
                quantity -= Mathf.Clamp(quantity, 0, inventoryData.hotbarItems[i].stackLimit);
                hotbarSlotQuantity[i].text = inventoryData.hotbarItems[i].quantity.ToString();
                if (hotbarActive && inventoryData.hotbarItems[selected_id] != null)
                {
                    inventoryData.hotbarItems[selected_id].gameObject.SetActive(true);
                    item_selected = true;
                    UpdateAnimator();
                }
                if (quantity == 0)
                {
                    return;
                }
            }
        }

        for (int y = 0; y < inventoryData.inventoryItems.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryData.inventoryItems.GetLength(1); x++)
            {
                if (inventoryData.inventoryItems[y, x] != null && inventoryData.inventoryItems[y, x].itemType == itemType)
                {
                    int originalQuantity = inventoryData.inventoryItems[y, x].quantity;
                    inventoryData.inventoryItems[y, x].quantity = Mathf.Clamp(inventoryData.inventoryItems[y, x].quantity + quantity, 1, inventoryData.inventoryItems[y, x].stackLimit);
                    quantity = originalQuantity + quantity - Mathf.Clamp(originalQuantity + quantity, 1, inventoryData.inventoryItems[y, x].stackLimit);
                    inventorySlotQuantity[y, x].text = inventoryData.inventoryItems[y, x].quantity.ToString();
                    if (quantity == 0)
                    {
                        return;
                    }
                }
                if (inventoryData.inventoryItems[y, x] == null)
                {
                    inventoryData.inventoryItems[y, x] = new Item(itemType, quantity);
                    inventorySlotIcons[y, x].text = inventoryData.inventoryItems[y, x].Name;
                    quantity -= Mathf.Clamp(quantity, 1, inventoryData.inventoryItems[y, x].stackLimit);
                    inventorySlotQuantity[y, x].text = inventoryData.inventoryItems[y, x].quantity.ToString();
                    if (quantity == 0)
                    {
                        return;
                    }
                }
            }
        }


    }

    public void MoveItem((int x, int y) fromPos, (int x, int y) toPos, int q)
    {
        int quantity = q;
        if (inventoryData.inventoryItems[toPos.y, toPos.x] == null)
        {
            AddingToInventory(inventoryData.inventoryItems[fromPos.y, fromPos.x].itemType, toPos, quantity);
        }

        else if (inventoryData.inventoryItems[fromPos.y, fromPos.x].itemType == inventoryData.inventoryItems[toPos.y, toPos.x].itemType)
        {
            quantity = AddingToInventory(inventoryData.inventoryItems[fromPos.y, fromPos.x].itemType, toPos, quantity);
        }
        RemovingFromInventory(fromPos, quantity);
    }

    public void MoveItem(int fromPos, int toPos, int q)
    {
        int quantity = q;
        if (inventoryData.hotbarItems[toPos] == null)
        {
            AddingToHotbar(inventoryData.hotbarItems[fromPos].itemType, toPos, quantity);
        }

        else if (inventoryData.hotbarItems[fromPos].itemType == inventoryData.hotbarItems[toPos].itemType)
        {
            quantity = AddingToHotbar(inventoryData.hotbarItems[fromPos].itemType, toPos, quantity);
        }
        RemovingFromHotbar(fromPos, quantity);
        
        if (hotbarActive && toPos == selected_id && inventoryData.hotbarItems[selected_id] != null)
        {
            inventoryData.hotbarItems[selected_id].gameObject.SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
    }

    public void MoveItem((int x, int y) fromPos, int toPos, int q)
    {
        int quantity = q;
        if (inventoryData.hotbarItems[toPos] == null)
        {
            AddingToHotbar(inventoryData.inventoryItems[fromPos.y, fromPos.x].itemType, toPos, quantity);
        }

        else if (inventoryData.inventoryItems[fromPos.y, fromPos.x].itemType == inventoryData.hotbarItems[toPos].itemType)
        {
            quantity = AddingToHotbar(inventoryData.inventoryItems[fromPos.y, fromPos.x].itemType, toPos, quantity); ;
        }
        RemovingFromInventory(fromPos, quantity);

        if (hotbarActive && toPos == selected_id && inventoryData.hotbarItems[selected_id] != null)
        {
            inventoryData.hotbarItems[selected_id].gameObject.SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }

    }

    public void MoveItem(int fromPos, (int x, int y) toPos, int q)
    {
        int quantity = q;
        if (inventoryData.inventoryItems[toPos.y, toPos.x] == null)
        {
            AddingToInventory(inventoryData.hotbarItems[fromPos].itemType, toPos, quantity);
        }

        else if (inventoryData.hotbarItems[fromPos].itemType == inventoryData.inventoryItems[toPos.y, toPos.x].itemType)
        {
            quantity = AddingToInventory(inventoryData.hotbarItems[fromPos].itemType, toPos, quantity);
        }
        RemovingFromHotbar(fromPos, quantity);
    }
    private int AddingToInventory(Item.ItemType fromType, (int x, int y) toPos, int q)
    {
        int quantity = q;
        if (inventoryData.inventoryItems[toPos.y, toPos.x] == null)
        {
            inventoryData.inventoryItems[toPos.y, toPos.x] = new Item(fromType, quantity);
            inventorySlotIcons[toPos.y, toPos.x].text = inventoryData.inventoryItems[toPos.y, toPos.x].Name;
            inventorySlotQuantity[toPos.y, toPos.x].text = inventoryData.inventoryItems[toPos.y, toPos.x].quantity.ToString();
        }
        else if (fromType == inventoryData.inventoryItems[toPos.y, toPos.x].itemType)
        {
            if (inventoryData.inventoryItems[toPos.y, toPos.x].quantity + quantity >= inventoryData.inventoryItems[toPos.y, toPos.x].stackLimit)
            {
                inventoryData.hotbarItems[selected_id].gameObject.SetActive(false);
                quantity = inventoryData.inventoryItems[toPos.y, toPos.x].stackLimit - inventoryData.inventoryItems[toPos.y, toPos.x].quantity;
            }
            inventoryData.inventoryItems[toPos.y, toPos.x].quantity += quantity;
            inventorySlotQuantity[toPos.y, toPos.x].text = inventoryData.inventoryItems[toPos.y, toPos.x].quantity.ToString();
        }
        return quantity;
    }

    private void RemovingFromInventory((int x, int y) fromPos, int quantity)
    {
        inventoryData.inventoryItems[fromPos.y, fromPos.x].quantity -= quantity;
        if (inventoryData.inventoryItems[fromPos.y, fromPos.x].quantity == 0)
        {
            inventoryData.inventoryItems[fromPos.y, fromPos.x] = null;
            inventorySlotIcons[fromPos.y, fromPos.x].text = string.Empty;
            inventorySlotQuantity[fromPos.y, fromPos.x].text = string.Empty;
        }
        else
        {
            inventorySlotQuantity[fromPos.y, fromPos.x].text = inventoryData.inventoryItems[fromPos.y, fromPos.x].quantity.ToString();
        }
    }
    public int AddingToHotbar(Item.ItemType fromType, int toPos, int q)
    {
        int quantity = q;
        if (inventoryData.hotbarItems[toPos] == null)
        {
            inventoryData.hotbarItems[toPos] = new Item(fromType, quantity);
            hotbarSlotIcons[toPos].text = inventoryData.hotbarItems[toPos].Name;
            hotbarSlotQuantity[toPos].text = inventoryData.hotbarItems[toPos].quantity.ToString();
        }
        else if (fromType == inventoryData.hotbarItems[toPos].itemType)
        {
            if (inventoryData.hotbarItems[toPos].quantity + quantity >= inventoryData.hotbarItems[toPos].stackLimit)
            {
                quantity = inventoryData.hotbarItems[toPos].stackLimit - inventoryData.hotbarItems[toPos].quantity;
            }
            inventoryData.hotbarItems[toPos].quantity += quantity;
            hotbarSlotQuantity[toPos].text = inventoryData.hotbarItems[toPos].quantity.ToString();
        }
        return quantity;
    }

    public void RemovingFromHotbar(int fromPos, int quantity)
    {
        inventoryData.hotbarItems[fromPos].quantity -= quantity;
        if (inventoryData.hotbarItems[fromPos].quantity == 0)
        {
            RemoveHotbarItem(fromPos);
        }
        else
        {
            hotbarSlotQuantity[fromPos].text = inventoryData.hotbarItems[fromPos].quantity.ToString();
        }
    }
    private void RemoveHotbarItem(int fromPos)
    {
        inventoryData.hotbarItems[selected_id].gameObject.SetActive(false);
        inventoryData.hotbarItems[fromPos] = null;
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
            inventoryData.hotbarItems[selected_id].gameObject.SetActive(false);
        }
        hotbarSlotImg[selected_id].color = hotbarSlotInactive;

        selected_id -= direction;
        selected_id = selected_id > 9 ? selected_id - 10 : selected_id;
        selected_id = selected_id < 0 ? selected_id + 10 : selected_id;

        hotbarSlotImg[selected_id].color = hotbarSlotActive;
        if (inventoryData.hotbarItems[selected_id] != null)
        {
            inventoryData.hotbarItems[selected_id].gameObject.SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
        else
        {
            item_selected = false;
            animator.SetBool("Sword_Equipped", false);
            animator.SetBool("Axe_Equipped", false);
            animator.SetBool("Medkit_Equipped", false);
        }

        yield return new WaitForSeconds(0.025f);
        canScroll = true;
    }
}
