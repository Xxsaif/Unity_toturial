using JetBrains.Annotations;
using Mono.Cecil;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Inventory : MonoBehaviour
{
    
    private int selected_id;
    private bool item_selected;
    [SerializeField] private Animator animator;
    public bool canSwapItem;
    private readonly Color hotbarSlotActive = new(72f / 255f, 72f / 255f, 72f / 255f, 200f / 255f);
    private readonly Color hotbarSlotInactive = new(72f / 255f, 72f / 255f, 72f / 255f, 100f / 255f);
    private bool hotbarActive;
    public GameObject inventoryScreen;
    [HideInInspector] public bool inventoryActive;

    [HideInInspector] public GameObject[,] inventoryItems = new GameObject[3, 6];
    [HideInInspector] public int[,] inventoryItemQuantity = new int[3, 6];
    [SerializeField] private GameObject testInvObj;
    [SerializeField] private GameObject testHotbarObj;
    [HideInInspector] public GameObject[,] inventorySlots = new GameObject[3, 6];
    [HideInInspector] public GameObject[,] inventorySlotIcons = new GameObject[3, 6];
    [HideInInspector] public TextMeshProUGUI[,] inventorySlotQuantity = new TextMeshProUGUI[3, 6];

    [HideInInspector] public GameObject[] hotbarItems;
    [HideInInspector] public int[] hotbarItemQuantity;
    [HideInInspector] public GameObject[] hotbarSlots;
    [HideInInspector] public TextMeshProUGUI[] hotbarSlotQuantity;

    [SerializeField] private TextMeshProUGUI interactionText;
    private bool canScroll;

    private readonly KeyCode[] numberKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

    public int itemStackLimit;
    void Start()
    {
        hotbarItems = new GameObject[10];
        hotbarItemQuantity = new int[10];
        hotbarSlotQuantity = new TextMeshProUGUI[10];
        hotbarSlots = new GameObject[10];
        item_selected = false;
        canSwapItem = false;
        hotbarActive = false;
        canScroll = true;
        GameObject hotbar = GameObject.Find("Hotbar");
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbar.transform.GetChild(i).gameObject;
            hotbarSlots[i].GetComponent<Slot>().slotHotbarPos = i;
            hotbarSlotQuantity[i] = hotbarSlots[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        }
        inventoryScreen.SetActive(true);
        for (int y = 0; y < inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < inventorySlots.GetLength(1); x++)
            {
                if (inventoryScreen.transform.GetChild(y).transform.GetChild(x).gameObject != null)
                {
                    inventorySlots[y, x] = inventoryScreen.transform.GetChild(y).transform.GetChild(x).gameObject;
                    inventorySlots[y, x].GetComponent<Slot>().slotInventoryPos = (x, y);
                    if (inventorySlots[y, x].transform.GetChild(0).gameObject != null)
                    {
                        inventorySlotIcons[y, x] = inventorySlots[y, x].transform.GetChild(0).gameObject;
                    }
                    if (inventorySlots[y, x].transform.GetChild(1).gameObject != null)
                    {
                        inventorySlotQuantity[y, x] = inventorySlots[y, x].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }
        inventoryScreen.SetActive(false);
        AddItem(testInvObj, 2);
        AddItem(testInvObj, 500);
        AddItem(testHotbarObj, 2);
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

            if (Input.mouseScrollDelta.y != 0 && canScroll)
            {
                canScroll = false;
                StartCoroutine(Scroll((int)Input.mouseScrollDelta.y));
            }
            if (Input.anyKeyDown)
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
                    hotbarItems[selected_id].SetActive(true);
                    item_selected = true;
                }
                hotbarSlots[selected_id].GetComponent<Slot>().image.color = hotbarSlotActive;
                canSwapItem = true;
                UpdateAnimator();
            }
            else if (!hotbarActive)
            {
                if (canSwapItem)
                {
                    if (item_selected)
                    {
                        hotbarItems[selected_id].SetActive(false);
                        item_selected = false;
                        animator.SetBool("Sword_Equipped", false);
                        animator.SetBool("Axe_Equipped", false);
                    }
                    hotbarSlots[selected_id].GetComponent<Slot>().image.color = hotbarSlotInactive;
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
            interactionText.text = "Press F to\nPick up " + other.GetComponent<DroppedItem>().itemName;
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
        if (hotbarItems[selected_id] != null)
        {
            switch (hotbarItems[selected_id].name)
            {
                case "Sword":
                    animator.SetBool("Sword_Equipped", true);
                    animator.SetBool("Axe_Equipped", false);
                    break;

                case "Axe":
                    animator.SetBool("Axe_Equipped", true);
                    animator.SetBool("Sword_Equipped", false);
                    break;
            }
        }
        
        else
        {
            animator.SetBool("Axe_Equipped", false);
            animator.SetBool("Sword_Equipped", false);
        }
    }

    


    public int AddItemToInventory((int x, int y) fromPos, (int x, int y) toPos, int quantity)
    {
        inventoryItems[toPos.y, toPos.x] = inventoryItems[fromPos.y, fromPos.x];
        inventorySlots[toPos.y, toPos.x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = inventoryItems[fromPos.y, fromPos.x].name;
        int originalQuantity = inventoryItemQuantity[toPos.y, toPos.x];
        inventoryItemQuantity[toPos.y, toPos.x] = Mathf.Clamp(quantity + inventoryItemQuantity[toPos.y, toPos.x], 0, itemStackLimit);
        inventorySlotQuantity[toPos.y, toPos.x].text = inventoryItemQuantity[toPos.y, toPos.x].ToString();
        return inventoryItemQuantity[toPos.y, toPos.x] < itemStackLimit ? quantity : itemStackLimit - originalQuantity;
    }

    public int AddItemToInventory(int fromPos, (int x, int y) toPos, int quantity)
    {
        inventoryItems[toPos.y, toPos.x] = hotbarItems[fromPos];
        inventorySlots[toPos.y, toPos.x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = hotbarItems[fromPos].name;
        int originalQuantity = inventoryItemQuantity[toPos.y, toPos.x];
        inventoryItemQuantity[toPos.y, toPos.x] = Mathf.Clamp(quantity + inventoryItemQuantity[toPos.y, toPos.x], 0, itemStackLimit);
        inventorySlotQuantity[toPos.y, toPos.x].text = inventoryItemQuantity[toPos.y, toPos.x].ToString();
        return inventoryItemQuantity[toPos.y, toPos.x] < itemStackLimit ? quantity : itemStackLimit - originalQuantity;
    }

    public int AddItemToHotbar(int fromPos, int toPos, int quantity)
    {
        hotbarItems[toPos] = hotbarItems[fromPos];
        hotbarSlots[toPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = hotbarItems[fromPos].name;
        int originalQuantity = hotbarItemQuantity[toPos];
        hotbarItemQuantity[toPos] = Mathf.Clamp(quantity + hotbarItemQuantity[toPos], 0, itemStackLimit);
        hotbarSlotQuantity[toPos].text = hotbarItemQuantity[toPos].ToString();
        if (hotbarActive && toPos == selected_id && hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
        return hotbarItemQuantity[toPos] < itemStackLimit ? quantity : itemStackLimit - originalQuantity;
    }
    public int AddItemToHotbar((int x, int y) fromPos, int toPos, int quantity)
    {
        hotbarItems[toPos] = inventoryItems[fromPos.y, fromPos.x];
        hotbarSlots[toPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = inventoryItems[fromPos.y, fromPos.x].name;
        int originalQuantity = hotbarItemQuantity[toPos];
        hotbarItemQuantity[toPos] = Mathf.Clamp(quantity + hotbarItemQuantity[toPos], 0, itemStackLimit);
        hotbarSlotQuantity[toPos].text = hotbarItemQuantity[toPos].ToString();
        if (hotbarActive && toPos == selected_id && hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
        return hotbarItemQuantity[toPos] < itemStackLimit ? quantity : itemStackLimit - originalQuantity;
    }

    public void AddItem(GameObject obj, int q)
    {
        int quantity = q;
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == obj)
            {
                int originalQuantity = hotbarItemQuantity[i];
                hotbarItemQuantity[i] = Mathf.Clamp(hotbarItemQuantity[i]+quantity, 0, itemStackLimit);
                quantity = originalQuantity + quantity - Mathf.Clamp(hotbarItemQuantity[i] + quantity, 0, itemStackLimit);
                hotbarSlotQuantity[i].text = hotbarItemQuantity[i].ToString();
                if (quantity == 0)
                {
                    return;
                }
            }
            else if (hotbarItems[i] == null)
            {
                hotbarItems[i] = obj;
                hotbarSlots[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
                hotbarItemQuantity[i] += Mathf.Clamp(quantity, 0, itemStackLimit);
                quantity -= Mathf.Clamp(quantity, 0, itemStackLimit);
                hotbarSlotQuantity[i].text = hotbarItemQuantity[i].ToString();
                if (hotbarActive && hotbarItems[selected_id] != null)
                {
                    hotbarItems[selected_id].SetActive(true);
                    item_selected = true;
                    UpdateAnimator();
                }
                if (quantity == 0)
                {
                    return;
                }
            }
        }

        for (int y = 0; y < inventoryItems.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryItems.GetLength(1); x++)
            {
                if (inventoryItems[y, x] == obj)
                {
                    int originalQuantity = inventoryItemQuantity[y, x];
                    inventoryItemQuantity[y, x] = Mathf.Clamp(inventoryItemQuantity[y, x] + quantity, 0, itemStackLimit);
                    quantity = originalQuantity + quantity - Mathf.Clamp(inventoryItemQuantity[y, x] + quantity, 0, itemStackLimit);
                    inventorySlotQuantity[y, x].text = inventoryItemQuantity[y, x].ToString();
                    if (quantity == 0)
                    {
                        return;
                    }
                }
                if (inventoryItems[y, x] == null)
                {
                    inventoryItems[y, x] = obj;
                    inventorySlots[y, x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
                    inventoryItemQuantity[y, x] += Mathf.Clamp(quantity, 0, itemStackLimit);
                    quantity -= Mathf.Clamp(quantity, 0, itemStackLimit);
                    inventorySlotQuantity[y, x].text = inventoryItemQuantity[y, x].ToString();
                    if (quantity == 0)
                    {
                        return;
                    }
                }
            }
        }

        
    }

    public void RemoveItemFromInventory((int x, int y) fromPos, int quantity)
    {

        inventoryItemQuantity[fromPos.y, fromPos.x] -= quantity;
        if (inventoryItemQuantity[fromPos.y, fromPos.x] == 0)
        {
            inventoryItems[fromPos.y, fromPos.x] = null;
            inventorySlots[fromPos.y, fromPos.x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
            inventorySlotQuantity[fromPos.y, fromPos.x].text = string.Empty;
        }
        else
        {
            inventorySlotQuantity[fromPos.y, fromPos.x].text = inventoryItemQuantity[fromPos.y, fromPos.x].ToString();
        }
    }
    public void RemoveItemFromHotbar(int pos, int quantity)
    {

        hotbarItemQuantity[pos] -= quantity;
        if (hotbarItemQuantity[pos] == 0)
        {
            if (pos == selected_id)
            {
                hotbarItems[pos].SetActive(false);
            }
            hotbarItems[pos] = null;
            hotbarSlots[pos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
            hotbarSlotQuantity[pos].text = string.Empty;
            if (hotbarActive && pos == selected_id)
            {
                item_selected = false;
                UpdateAnimator();
            }
        }
        else
        {
            hotbarSlotQuantity[pos].text = hotbarItemQuantity[pos].ToString(); 
        }
    }

    public void MoveItem((int x, int y) fromPos, (int x, int y) toPos, int q)
    {
        int quantity = q;
        quantity = AddItemToInventory(fromPos, toPos, quantity);
        RemoveItemFromInventory(fromPos, quantity);
    }
    public void MoveItem(int fromPos, (int x, int y) toPos, int q)
    {
        int quantity = q;
        quantity = AddItemToInventory(fromPos, toPos, quantity);
        RemoveItemFromHotbar(fromPos, quantity);
    }
    public void MoveItem(int fromPos, int toPos, int q)
    {
        int quantity = q;
        quantity = AddItemToHotbar(fromPos, toPos, quantity);
        RemoveItemFromHotbar(fromPos, quantity);
    }

    public void MoveItem((int x, int y) fromPos, int toPos, int q)
    {
        int quantity = q;
        quantity = AddItemToHotbar(fromPos, toPos, quantity);
        RemoveItemFromInventory(fromPos, quantity);
    }

    IEnumerator Scroll(int direction)
    {
        if (item_selected)
        {
            hotbarItems[selected_id].SetActive(false);
        }
        hotbarSlots[selected_id].GetComponent<Slot>().image.color = hotbarSlotInactive;

        selected_id -= direction;
        selected_id = selected_id > 9 ? selected_id - 10 : selected_id;
        selected_id = selected_id < 0 ? selected_id + 10 : selected_id;

        hotbarSlots[selected_id].GetComponent<Slot>().image.color = hotbarSlotActive;
        if (hotbarItems[selected_id] != null)
        {
            hotbarItems[selected_id].SetActive(true);
            item_selected = true;
            UpdateAnimator();
        }
        else
        {
            item_selected = false;
            animator.SetBool("Sword_Equipped", false);
            animator.SetBool("Axe_Equipped", false);
        }
        yield return new WaitForSeconds(0.025f);
        canScroll = true;
    }
}
