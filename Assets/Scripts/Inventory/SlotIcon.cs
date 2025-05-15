using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Created by Herman Bergström
public class SlotIcon : MonoBehaviour
{
    private Vector3 startPos;
    [HideInInspector] public bool isBeingDragged;
    private SlotManager slotManager;
    private int inventorySlotPos = -1;
    private KeyCode mousekey;
    
    void Start()
    {
        slotManager = GameObject.Find("Player").GetComponent<SlotManager>();
        startPos = transform.localPosition;
        
    }

    public void Click()
    {
        mousekey = Input.GetKey(KeyCode.Mouse0) ? KeyCode.Mouse0 : Input.GetKey(KeyCode.Mouse1) ? KeyCode.Mouse1 : KeyCode.None;

    }
    public void Drag()
    {

        if (inventorySlotPos == -1)
        {
            inventorySlotPos = gameObject.transform.parent.gameObject.GetComponent<Slot>().slotInventoryPos;
        }

        if (gameObject.transform.parent.gameObject.GetComponent<Slot>().inventory.inventoryItems[inventorySlotPos] != null)
        {
            transform.position = Input.mousePosition;
            isBeingDragged = true;
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = false;
        }
    }

    public void EndDrag()
    {
        if (gameObject.transform.parent.gameObject.GetComponent<Slot>().inventory.inventoryItems[inventorySlotPos] != null)
        {
            transform.localPosition = startPos;
            isBeingDragged = false;
            
            slotManager.TryMoveItem(gameObject.transform.parent.gameObject, mousekey == KeyCode.Mouse0 ? 1 : mousekey == KeyCode.Mouse1 ? gameObject.transform.parent.gameObject.GetComponent<Slot>().inventory.inventoryItems[inventorySlotPos].quantity : 0);
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = true;
        }
        
    }
    void Update()
    {
        
    }

    
}
