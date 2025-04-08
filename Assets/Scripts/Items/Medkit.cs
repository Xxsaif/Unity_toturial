using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour
{
    private Inventory inventoryScr;
    private PlayerHealth playerHealthScr;
    [SerializeField] private Animator animator;
    private bool isUsing = false;
    private float useTime;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        inventoryScr = player.GetComponent<Inventory>();
        playerHealthScr = player.GetComponent<PlayerHealth>();
        Debug.Log((player != null) + ", " + inventoryScr.gameObject.name + ", " + playerHealthScr.gameObject.name);
    }


    void Update()
    {
        if ( Input.GetKey(KeyCode.Mouse1) )
        {
            Debug.Log(!isUsing);
        }
        if (CanUse())
        {
            StartCoroutine(Use());
            animator.SetTrigger("Use");
        }
        /*
        if( isUsing )
        {
            if(useTime + 1.667F < Time.time )
            {
                isUsing = false;
                inventoryScr.canSwapItem = true;
                playerHealthScr.Heal(50);
                inventoryScr.inventoryData.hotbarItems[inventoryScr.selected_id].quantity--;
                inventoryScr.hotbarSlotQuantity[inventoryScr.selected_id].text = inventoryScr.inventoryData.hotbarItems[inventoryScr.selected_id].quantity.ToString();
            }
        }*/
    }

    protected IEnumerator Use()
    {
        inventoryScr.canSwapItem = false;
        isUsing = true;
        /*
        useTime = Time.time;
        yield return null;*/
        
        yield return new WaitForSeconds(0.833f);
        playerHealthScr.Heal(50);
        yield return new WaitForSeconds(0.833f);
        Debug.Log("trigger2");
        inventoryScr.inventoryData.hotbarItems[inventoryScr.selected_id].quantity--;
        inventoryScr.hotbarSlotQuantity[inventoryScr.selected_id].text = inventoryScr.inventoryData.hotbarItems[inventoryScr.selected_id].quantity.ToString();
        isUsing = false;
        inventoryScr.canSwapItem = true;
    }

    protected bool CanUse() => Input.GetKeyDown(KeyCode.Mouse1) && !isUsing && !inventoryScr.inventoryActive;
}
