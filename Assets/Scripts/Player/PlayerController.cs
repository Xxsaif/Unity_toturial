using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
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
    private float baseMaxStamina = 5f;
    private float staminaRegenRate = 1f;
    private float staminaDrainRate = 1f;
    private float exhausted = 0f;
    public bool isExhausted;
    [SerializeField] private Slider staminabarSlider;

    private float timer;
    [SerializeField] private TextMeshProUGUI timerText;

    public static bool inventoryActive;

    private PlayerLevelSystem playerLevelSystem;
    private readonly Color staminaNormalColor = new Color(95f, 95f, 95f, 170f) / 255f;
    private readonly Color staminaExhaustedColor = new Color(255f, 0f, 0f, 170f) / 255f;
    [SerializeField] private Image staminaBorder;
    void Start()
    {
        inventoryActive = false;
        controller = GetComponent<CharacterController>();
        grounded = true;
        moveSpeed = walkSpeed;
        playerLevelSystem = GetComponent<PlayerLevelSystem>();
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

        if (!inventoryActive)
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
                    UpdateStaminaUI();

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
        else if (inventoryActive)
        {
            moveSpeed = walkSpeed;
        }
        if (moveSpeed == walkSpeed && (exhausted > 0f || stamina < maxStamina))
        {
            if (stamina < maxStamina)
            {
                stamina += Time.deltaTime * staminaRegenRate;
                stamina = Mathf.Clamp(stamina, 0f, maxStamina);
                UpdateStaminaUI();
            }

            if (exhausted > 0f)
            {
                exhausted -= Time.deltaTime * staminaRegenRate;
                exhausted = Mathf.Clamp(exhausted, 0f, 0.6f * maxStamina);
                
            }
        }
        
        staminaBorder.color = exhausted > 0f ? staminaExhaustedColor : staminaNormalColor;
        falling = velocity.y < -3f && !grounded;
        isExhausted = IsExhausted();
    }

    private void UpdateStaminaUI()
    {
        staminabarSlider.value = stamina / maxStamina;
    }
    public void UpdateMaxStamina()
    {
        maxStamina = baseMaxStamina * playerLevelSystem.PlayerStaminaMultiplier;
    }
    private bool IsExhausted() => exhausted > 0f;
}
