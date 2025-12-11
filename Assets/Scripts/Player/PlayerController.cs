using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
// Created by Herman Bergstr�m
public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float walkSpeed = 5f;
    private float runSpeed = 8f;
    [SerializeField] private LayerMask groundMask;
    private CharacterController controller;
    private bool grounded;
    [HideInInspector] public bool falling;

    private readonly float landGravity = -30f;
    private readonly float waterGravity = -5f;
    private float gravity = -30f;
    private float jumpHeight = 1.5f;
    private float groundDistance = 0.3f;
    [SerializeField] private Transform groundCheck;
    private Vector3 velocity;

    private float stamina; 
    private float maxStamina;
    private float baseMaxStamina = 5f;
    private float staminaRegenRate = 1f;
    private float staminaDrainRate = 1f;
    private float exhausted = 0f;
    public bool isExhausted;
    [SerializeField] private Slider staminabarSlider;

    private float timer;
    [SerializeField] private TextMeshProUGUI timerText;

    private PlayerLevelSystem playerLevelSystem;
    private readonly Color staminaNormalColor = new Color(95f, 95f, 95f, 170f) / 255f;
    private readonly Color staminaExhaustedColor = new Color(255f, 0f, 0f, 170f) / 255f;
    [SerializeField] private Image staminaBorder;
    [SerializeField] private Transform water;

    void Start()
    {
        maxStamina = baseMaxStamina;
        stamina = maxStamina;
        controller = GetComponent<CharacterController>();
        grounded = true;
        moveSpeed = walkSpeed;
        playerLevelSystem = GetComponent<PlayerLevelSystem>();
    }

    
    void Update()
    {
        // Creates a sphere at the position of the groundcheck thats's placed at the players feet. GroundDistance is the radius of the sphere.
        // It only collides if theres a game object laying on the ground (groundMask) that will collide with the sphere and return a bool.
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        timer += Time.deltaTime;
        timer %= 4.033f;
        timerText.text = timer.ToString() + "s";

        if (grounded && velocity.y < 0f)
        {
            velocity.y = -3f;
        }

        // Only run movement code when inventory is closed to avoid accidental moves.
        if (!InventorySystem.inventoryActive)
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

            gravity = InWater() ? waterGravity : landGravity;

            if (Input.GetKeyDown(KeyCode.Space) && grounded && !InWater())
            {
                velocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            if (Input.GetKey(KeyCode.Space) && InWater())
            {
                velocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else if (InventorySystem.inventoryActive)
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

    private bool InWater() => transform.position.y < water.position.y;
}
