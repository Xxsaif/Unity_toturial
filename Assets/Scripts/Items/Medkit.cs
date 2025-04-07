using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour
{
    private Inventory inventoryScr;
    private PlayerHealth playerHealthScr;
    [SerializeField] private Animator animator;
    private bool isUsing = false;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        inventoryScr = player.GetComponent<Inventory>();
        playerHealthScr = player.GetComponent<PlayerHealth>();
        Debug.Log((player != null) + ", " + inventoryScr.gameObject.name + ", " + playerHealthScr.gameObject.name);
    }


    void Update()
    {
        Debug.Log("trigger");
        if (CanUse())
        {
            StartCoroutine(Use());
            animator.SetTrigger("Use");
        }
    }

    protected IEnumerator Use()
    {
        inventoryScr.canSwapItem = false;
        isUsing = true;
        yield return new WaitForSeconds(1.667f);
        inventoryScr.inventoryData.hotbarItems[inventoryScr.selected_id].quantity--;
        inventoryScr.hotbarSlotQuantity[inventoryScr.selected_id].text = inventoryScr.inventoryData.hotbarItems[inventoryScr.selected_id].quantity.ToString();
        isUsing = false;
        inventoryScr.canSwapItem = true;
    }

    protected bool CanUse() => Input.GetKey(KeyCode.Mouse1) && !isUsing && !inventoryScr.inventoryActive;
}
