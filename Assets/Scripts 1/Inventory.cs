using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> hotbar;
    private int selected_id;
    private bool item_selected;
    [SerializeField] private Animator animator;
    public bool canSwapItem;
    [SerializeField] private Image[] hotbarSlots;
    private Color hotbarSlotActive = new Color(72f / 255f, 72f / 255f, 72f / 255f, 200f / 255f);
    private Color hotbarSlotInactive = new Color(72f / 255f, 72f / 255f, 72f / 255f, 100f / 255f);
    private bool hotbarActive;

    void Start()
    {
        item_selected = false;
        canSwapItem = false;
        hotbarActive = false;
    }

    
    void Update()
    {
        
        if (canSwapItem)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.C))
            {

                if (item_selected)
                {
                    hotbar[selected_id].SetActive(false);
                }

                hotbarSlots[selected_id].color = hotbarSlotInactive;
                selected_id = Input.GetKeyDown(KeyCode.C) ? selected_id + 1 : selected_id - 1;
                selected_id = selected_id > 9 ? 0 : selected_id;
                selected_id = selected_id < 0 ? 9 : selected_id;

                hotbarSlots[selected_id].color = hotbarSlotActive;
                if (ItemSelected())
                {
                    hotbar[selected_id].SetActive(true);
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            hotbarActive = !hotbarActive;
            
            if (hotbarActive)
            {
                if (hotbar.Count > 0f)
                {
                    selected_id = 0;
                    hotbar[selected_id].SetActive(true);
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
                        hotbar[selected_id].SetActive(false);
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
        switch (hotbar[selected_id].gameObject.name)
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
    private bool ItemSelected() => selected_id <= hotbar.Count - 1;
}
