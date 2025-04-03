using JetBrains.Annotations;
using NUnit.Framework;
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
    private Color hotbarSlotActive = new Color(72f / 255f, 72f / 255f, 72f / 255f, 200f / 255f);
    private Color hotbarSlotInactive = new Color(72f / 255f, 72f / 255f, 72f / 255f, 100f / 255f);
    private bool hotbarActive;
    private bool hotbarWasActive;
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

    void Start()
    {

        hotbarItems = new GameObject[10];
        hotbarItemQuantity = new int[10];
        hotbarSlotQuantity = new TextMeshProUGUI[10];
        hotbarSlots = new GameObject[10];
        item_selected = false;
        canSwapItem = false;
        hotbarActive = false;
        hotbarWasActive = false;
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
        AddItemToInventory(testInvObj, 3, 0, 0);
        AddItemToHotbar(testHotbarObj, 2, 0);
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

                if (item_selected)
                {
                    hotbarItems[selected_id].SetActive(false);
                }

                hotbarSlots[selected_id].GetComponent<Slot>().image.color = hotbarSlotInactive;
                selected_id = Input.GetKeyDown(KeyCode.C) ? selected_id + 1 : selected_id - 1;
                selected_id = selected_id > 9 ? 0 : selected_id;
                selected_id = selected_id < 0 ? 9 : selected_id;

                hotbarSlots[selected_id].GetComponent<Slot>().image.color = hotbarSlotActive;
                if (ItemSelected())
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

    
    private bool ItemSelected() => hotbarItems[selected_id] != null;

    public void AddItemToInventory(GameObject obj, int quantity, int xPos, int yPos)
    {
        if (xPos >= 0 && yPos >= 0 && xPos < inventoryItems.GetLength(1) && yPos < inventoryItems.GetLength(0) && inventoryItems[yPos, xPos] == null)
        {
            inventoryItems[yPos, xPos] = obj;
            inventorySlots[yPos, xPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
            inventoryItemQuantity[yPos, xPos] = quantity;
            inventorySlotQuantity[yPos, xPos].text = quantity.ToString();
        }
    }

    public void AddItemToHotbar(GameObject obj, int quantity, int pos)
    {
        if (pos >= 0 && pos < hotbarSlots.Length && hotbarItems[pos] == null)
        {
            hotbarItems[pos] = obj;
            hotbarSlots[pos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
            hotbarItemQuantity[pos] = quantity;
            hotbarSlotQuantity[pos].text = quantity.ToString();
            if (hotbarActive && pos == selected_id && hotbarItems[selected_id] != null)
            {
                hotbarItems[selected_id].SetActive(true);
                item_selected = true;
                UpdateAnimator();
            }
        }
    }

    public void AddItem(GameObject obj, int quantity)
    {
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = obj;
                hotbarSlots[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
                hotbarItemQuantity[i] = quantity;
                hotbarSlotQuantity[i].text = quantity.ToString();
                if (hotbarActive && hotbarItems[selected_id] != null)
                {
                    hotbarItems[selected_id].SetActive(true);
                    item_selected = true;
                    UpdateAnimator();
                }
                return;
            }
        }

        for (int y = 0; y < inventoryItems.GetLength(0); y++)
        {
            for (int x = 0; x < inventoryItems.GetLength(1); x++)
            {
                if (inventoryItems[y, x] == null)
                {
                    inventoryItems[y, x] = obj;
                    inventorySlots[y, x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
                    inventoryItemQuantity[y, x] = quantity;
                    inventorySlotQuantity[y, x].text = quantity.ToString();
                    return;
                }
            }
        }

        
    }

    public void RemoveItemFromInventory(int xPos, int yPos)
    {
        inventoryItems[yPos, xPos] = null;
        inventorySlots[yPos, xPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
        inventoryItemQuantity[yPos, xPos] = 0;
        inventorySlotQuantity[yPos, xPos].text = string.Empty;
    }
    public void RemoveItemFromHotbar(int pos)
    {

        if (pos == selected_id)
        {
            hotbarItems[pos].SetActive(false);
        }
        hotbarItems[pos] = null;
        hotbarSlots[pos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
        hotbarItemQuantity[pos] = 0;
        hotbarSlotQuantity[pos].text = string.Empty;
        if (hotbarActive && pos == selected_id)
        {
            item_selected = false;
            UpdateAnimator();
        }
    }

    public void MoveItem((int x, int y) fromPos, (int x, int y) toPos)
    {
        AddItemToInventory(inventoryItems[fromPos.y, fromPos.x], inventoryItemQuantity[fromPos.y, fromPos.x], toPos.x, toPos.y);
        RemoveItemFromInventory(fromPos.x, fromPos.y);
    }
    public void MoveItem(int fromPos, (int x, int y) toPos)
    {
        AddItemToInventory(hotbarItems[fromPos], hotbarItemQuantity[fromPos], toPos.x, toPos.y);
        RemoveItemFromHotbar(fromPos);
    }
    public void MoveItem(int fromPos, int toPos)
    {
        AddItemToHotbar(hotbarItems[fromPos], hotbarItemQuantity[fromPos], toPos);
        RemoveItemFromHotbar(fromPos);
    }

    public void MoveItem((int x, int y) fromPos, int toPos)
    {
        AddItemToHotbar(inventoryItems[fromPos.y, fromPos.x], inventoryItemQuantity[fromPos.y, fromPos.x], toPos);
        RemoveItemFromInventory(fromPos.x, fromPos.y);
    }
}
