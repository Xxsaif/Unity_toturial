using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float walkSpeed = 5f;
    private float runSpeed = 8f;
    [SerializeField] private LayerMask groundMask;
    private CharacterController controller;
    private bool grounded;
    [HideInInspector] public bool falling;
    private float gravity = -30f;
    private float jumpHeight = 1.5f;
    private float groundDistance = 0.3f;
    [SerializeField] private Transform groundCheck;
    private Vector3 velocity;

    private float stamina = 5f; 
    private float maxStamina = 5f;
    private float staminaRegenRate = 1f;
    private float staminaDrainRate = 1f;
    private float exhausted = 0f;
    public bool isExhausted;
    [SerializeField] private TextMeshProUGUI staminaText; // Temporary stamina text, could be replaced with stamina bar

    private float timer;
    [SerializeField] private TextMeshProUGUI timerText;

    private Inventory inventoryScr;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        grounded = true;
        moveSpeed = walkSpeed;
        inventoryScr = GetComponent<Inventory>();
    }

    
    void Update()
    {
        /* Skapar sfär vid positionen av groundcheck som är placerad vid spelarens fot. GroundDistance är radiusen av sfären. 
         * Den kollar om det finns något objekt på lagret ground (groundMask) som kolliderar med sfären och returnerar en bool. 
         */
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        timer += Time.deltaTime;
        timer %= 4.033f;
        timerText.text = timer.ToString() + "s";

        if (grounded && velocity.y < 0f)
        {
            velocity.y = -3f;
        }

        if (!inventoryScr.inventoryActive)
        {
            float hInput = Input.GetAxisRaw("Horizontal");
            float vInput = Input.GetAxisRaw("Vertical");

            Vector3 move = transform.right * hInput + transform.forward * vInput;
            move.Normalize();
        
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && exhausted == 0f)
            {
                if (stamina > 0f)
                {
                    moveSpeed = runSpeed;
                    stamina -= Time.deltaTime * staminaDrainRate;
                    stamina = Mathf.Clamp(stamina, 0f, maxStamina);
                    staminaText.text = Math.Round(stamina, 1).ToString();
                
                }
                if (Math.Round(stamina, 1) == 0f)
                {
                    moveSpeed = walkSpeed;
                    exhausted = 3f;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKey(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
            {
                moveSpeed = walkSpeed;
            }

            controller.Move(moveSpeed * Time.deltaTime * move);
        

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                velocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else if (inventoryScr.inventoryActive)
        {
            moveSpeed = walkSpeed;
        }
        if (moveSpeed == walkSpeed && (exhausted > 0f || stamina < maxStamina))
        {
            if (stamina < maxStamina)
            {
                stamina += Time.deltaTime * staminaRegenRate;
                stamina = Mathf.Clamp(stamina, 0f, maxStamina);
                staminaText.text = Math.Round(stamina, 1).ToString();
            }

            if (exhausted > 0f)
            {
                exhausted -= Time.deltaTime * staminaRegenRate;
                exhausted = Mathf.Clamp(exhausted, 0f, 3f);
            }
        }
        
        


        falling = velocity.y < -3f && !grounded;
        isExhausted = IsExhausted();
    }

    private bool IsExhausted() => exhausted > 0f;
}
