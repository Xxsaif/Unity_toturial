using UnityEngine;
// Created by Herman Bergström
public class MouseLook : MonoBehaviour
{
    private float sensitivity = 200f;
    private float xRotation = 0f;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform water;
    [SerializeField] private GameObject underWaterHud;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    
    void Update()
    {
        if (!InventorySystem.inventoryActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;



            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.transform.Rotate(Vector3.up * mouseX);
        }

        underWaterHud.SetActive(InWater());
    }

    private bool InWater() => transform.position.y < water.position.y;
}
