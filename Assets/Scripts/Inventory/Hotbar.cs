using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : Inventory
{
    [HideInInspector] public int selected_id;
    private bool item_selected;
    [SerializeField] private Animator animator;
    [HideInInspector] public bool canSwapItem;
    private readonly Color hotbarSlotActive = new(72f / 255f, 72f / 255f, 72f / 255f, 200f / 255f);
    private readonly Color hotbarSlotInactive = new(72f / 255f, 72f / 255f, 72f / 255f, 100f / 255f);
    private bool hotbarActive;
    private bool canScroll;
    private readonly KeyCode[] numberKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
    void Start()
    {
        item_selected = false;
        canSwapItem = false;
        hotbarActive = false;
        canScroll = true;
    }


    private void Update()
    {
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
                if (inventoryItems[selected_id] != null)
                {
                    inventoryItems[selected_id].gameObject.SetActive(true);
                    item_selected = true;
                }
                inventorySlots[selected_id].slotImg.color = hotbarSlotActive;
                canSwapItem = true;
                UpdateAnimator();
            }
            else if (!hotbarActive)
            {
                if (canSwapItem)
                {
                    if (item_selected)
                    {
                        inventoryItems[selected_id].gameObject.SetActive(false);
                        item_selected = false;
                        animator.SetBool("Item_Equipped", false);
                    }
                    inventorySlots[selected_id].slotImg.color = hotbarSlotInactive;
                    canSwapItem = false;
                }
                else
                {
                    hotbarActive = true;
                }
            }
        }
    }
    public override int AddItem(Item.ItemType itemType, int q)
    {
        int quantity = q;

        for (int y = 0; y < inventoryItems.GetLength(0); y++)
        {

            if (inventoryItems[y] != null && inventoryItems[y].itemType == itemType)
            {
                int originalQuantity = inventoryItems[y].quantity;
                inventoryItems[y].quantity = Mathf.Clamp(inventoryItems[y].quantity + quantity, 1, inventoryItems[y].stackLimit);
                quantity = originalQuantity + quantity - Mathf.Clamp(originalQuantity + quantity, 1, inventoryItems[y].stackLimit);
                inventorySlots[y].slotQuantity.text = inventoryItems[y].quantity.ToString();
                if (quantity == 0)
                {
                    return 0;
                }
            }
            if (inventoryItems[y] == null)
            {
                inventoryItems[y] = new Item(itemType, quantity);
                inventorySlots[y].slotIcon.text = inventoryItems[y].Name;
                quantity -= Mathf.Clamp(quantity, 1, inventoryItems[y].stackLimit);
                inventorySlots[y].slotQuantity.text = inventoryItems[y].quantity.ToString();
                if (hotbarActive && inventoryItems[selected_id] != null)
                {
                    inventoryItems[selected_id].gameObject.SetActive(true);
                    item_selected = true;
                    UpdateAnimator();
                }
                if (quantity == 0)
                {
                    return 0;
                }
            }

        }

        return quantity;
    }
    public override int AddItemTo(Item.ItemType fromType, int toPos, int q)
    {
        int quantity = q;
        if (inventoryItems[toPos] == null)
        {
            inventoryItems[toPos] = new Item(fromType, quantity);
            inventorySlots[toPos].slotIcon.text = inventoryItems[toPos].Name;
            inventorySlots[toPos].slotQuantity.text = inventoryItems[toPos].quantity.ToString();
            if (hotbarActive && toPos == selected_id && inventoryItems[selected_id] != null)
            {
                inventoryItems[selected_id].gameObject.SetActive(true);
                item_selected = true;
                UpdateAnimator();
            }
        }
        else if (fromType == inventoryItems[toPos].itemType)
        {
            if (inventoryItems[toPos].quantity + quantity >= inventoryItems[toPos].stackLimit)
            {
                quantity = inventoryItems[toPos].stackLimit - inventoryItems[toPos].quantity;
            }
            inventoryItems[toPos].quantity += quantity;
            inventorySlots[toPos].slotQuantity.text = inventoryItems[toPos].quantity.ToString();
        }
        return quantity;
    }

    public override void RemoveFrom(int fromPos, int quantity)
    {
        inventoryItems[fromPos].quantity -= quantity;
        if (inventoryItems[fromPos].quantity == 0)
        {
            RemoveHotbarItem(fromPos);
        }
        else
        {
            inventorySlots[fromPos].slotQuantity.text = inventoryItems[fromPos].quantity.ToString();
        }
    }

    private void UpdateAnimator()
    {
        if (inventoryItems[selected_id] != null)
        {
            animator.SetBool("Item_Equipped", true);
            animator.SetInteger("Item_Type_Id", (int)inventoryItems[selected_id].itemType);
        }

        else
        {
            animator.SetBool("Item_Equipped", false);
        }
    }

    private void RemoveHotbarItem(int fromPos)
    {
        if (fromPos == selected_id)
        {
            inventoryItems[selected_id].gameObject.SetActive(false);
        }
        inventoryItems[fromPos] = null;
        inventorySlots[fromPos].slotIcon.text = string.Empty;
        inventorySlots[fromPos].slotQuantity.text = string.Empty;
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
            inventoryItems[selected_id].gameObject.SetActive(false);
        }
        inventorySlots[selected_id].slotImg.color = hotbarSlotInactive;

        selected_id -= direction;
        selected_id = selected_id > 9 ? selected_id - 10 : selected_id;
        selected_id = selected_id < 0 ? selected_id + 10 : selected_id;

        inventorySlots[selected_id].slotImg.color = hotbarSlotActive;
        if (inventoryItems[selected_id] != null)
        {
            inventoryItems[selected_id].gameObject.SetActive(true);
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
