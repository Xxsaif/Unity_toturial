using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private float sensitivity = 200f;
    private float xRotation = 0f;

    [SerializeField] private GameObject player;
    private Inventory inventoryScr;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventoryScr = player.GetComponent<Inventory>();
    }

    
    void Update()
    {
        if (!inventoryScr.inventoryActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;



            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.transform.Rotate(Vector3.up * mouseX);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
