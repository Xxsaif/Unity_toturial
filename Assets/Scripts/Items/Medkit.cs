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
    }


    void Update()
    {
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
        yield return new WaitForSeconds(0.833f);
        playerHealthScr.Heal(50);
        yield return new WaitForSeconds(0.833f);
        inventoryScr.RemovingFromHotbar(inventoryScr.selected_id, 1);
        isUsing = false;
        inventoryScr.canSwapItem = true;
    }

    protected bool CanUse() => Input.GetKeyDown(KeyCode.Mouse1) && !isUsing && !inventoryScr.inventoryActive;
}
