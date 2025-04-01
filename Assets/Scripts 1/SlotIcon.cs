using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotIcon : MonoBehaviour
{
    private Vector3 startPos;
    [HideInInspector] public bool isBeingDragged;
    private SlotManager slotManager;
    private (int x, int y) slotPos = (-1, -1);
    void Start()
    {
        startPos = transform.localPosition;
        slotManager = GameObject.Find("Inventory").GetComponent<SlotManager>();
        
    }

    public void Drag()
    {
        if (slotPos == (-1, -1))
        {
            slotPos = slotManager.FindSlotPosition(gameObject.transform.parent.gameObject);
        }
        if (slotManager.inventoryScr.inventoryItems[slotPos.y, slotPos.x] != null)
        {
            transform.position = Input.mousePosition;
            isBeingDragged = true;
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = false;
        }
        
    }

    public void EndDrag()
    {
        if (slotManager.inventoryScr.inventoryItems[slotPos.y, slotPos.x] != null)
        {
            transform.localPosition = startPos;
            isBeingDragged = false;
            slotManager.TryMoveItem(gameObject.transform.parent.gameObject);
            gameObject.GetComponent<TextMeshProUGUI>().raycastTarget = true;
        }
    }
    void Update()
    {
        
    }
}
