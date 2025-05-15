using UnityEngine;
using System.Collections;
// Created by Herman Bergström
public class Medkit : MonoBehaviour
{
    private PlayerHealth playerHealthScr;
    [SerializeField] private Animator animator;
    private bool isUsing = false;
    private float useTime;
    protected Hotbar hotbar;
    protected Inventory inventory;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerHealthScr = player.GetComponent<PlayerHealth>();
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        hotbar = GameObject.FindWithTag("Player").GetComponent<Hotbar>();
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
        hotbar.canSwapItem = false;
        isUsing = true;
        yield return new WaitForSeconds(0.833f);
        playerHealthScr.Heal(50);
        yield return new WaitForSeconds(0.833f);
        hotbar.RemoveFrom(hotbar.selected_id, 1);
        isUsing = false;
        hotbar.canSwapItem = true;
    }

    protected bool CanUse() => Input.GetKeyDown(KeyCode.Mouse1) && !isUsing && !inventory.inventoryActive;
}
