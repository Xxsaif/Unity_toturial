using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    public List<GameObject> hotbarItems;
    private int selected_id;
    private bool item_selected;
    [SerializeField] private Animator animator;
    public bool canSwapItem;
    [SerializeField] private Image[] hotbarSlots;
    private Color hotbarSlotActive = new Color(72f / 255f, 72f / 255f, 72f / 255f, 200f / 255f);
    private Color hotbarSlotInactive = new Color(72f / 255f, 72f / 255f, 72f / 255f, 100f / 255f);
    private bool hotbarActive;
    [SerializeField] private GameObject inventoryScreen;
    [HideInInspector] public bool inventoryActive;

    [HideInInspector] public GameObject[,] inventoryItems = new GameObject[3, 6];
    [SerializeField] private GameObject testInvObj;
    [HideInInspector] public GameObject[,] inventorySlots = new GameObject[3, 6];

    void Start()
    {
        item_selected = false;
        canSwapItem = false;
        hotbarActive = false;

        inventoryScreen.SetActive(true);
        for (int y = 0; y < inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < inventorySlots.GetLength(1); x++)
            {
                if (inventoryScreen.transform.GetChild(y).transform.GetChild(x).gameObject != null)
                {
                    inventorySlots[y, x] = inventoryScreen.transform.GetChild(y).transform.GetChild(x).gameObject;
                }
            }
        }
        inventoryScreen.SetActive(false);
        AddItemToInventory(testInvObj, 0, 0);
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

                hotbarSlots[selected_id].color = hotbarSlotInactive;
                selected_id = Input.GetKeyDown(KeyCode.C) ? selected_id + 1 : selected_id - 1;
                selected_id = selected_id > 9 ? 0 : selected_id;
                selected_id = selected_id < 0 ? 9 : selected_id;

                hotbarSlots[selected_id].color = hotbarSlotActive;
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
                if (hotbarItems.Count > 0f)
                {
                    selected_id = 0;
                    hotbarItems[selected_id].SetActive(true);
                    hotbarSlots[selected_id].color = hotbarSlotActive;
                    item_selected = true;
                    canSwapItem = true;
                    UpdateAnimator();
                }
                else
                {
                    hotbarActive = false;
                }
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
                    hotbarSlots[selected_id].color = hotbarSlotInactive;
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
        switch (hotbarItems[selected_id].gameObject.name)
        {
            case "Sword_Parent":
                animator.SetBool("Sword_Equipped", true);
                animator.SetBool("Axe_Equipped", false);
                break;

            case "Axe_Parent":
                animator.SetBool("Axe_Equipped", true);
                animator.SetBool("Sword_Equipped", false);
                break;
        }
    }

    
    private bool ItemSelected() => selected_id <= hotbarItems.Count - 1;

    public void AddItemToInventory(GameObject obj, int xPos = -1, int yPos = -1)
    {
        if ((xPos == -1 && yPos == -1) || xPos < 0 || yPos < 0 || xPos > inventoryItems.GetLength(1) || yPos > inventoryItems.GetLength(0))
        {
            for (int y = 0; y < inventoryItems.GetLength(0); y++)
            {
                for (int x = 0; x < inventoryItems.GetLength(1); x++)
                {
                    if (inventoryItems[x, y] == null)
                    {
                        inventoryItems[x, y] = obj;
                    }
                }
            }
        }
        else
        {
            inventoryItems[yPos, xPos] = obj;
            inventorySlots[yPos, xPos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = obj.name;
           
        }
    }
}
