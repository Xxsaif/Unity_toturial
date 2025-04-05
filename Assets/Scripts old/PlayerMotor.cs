using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;

    public bool isGrounded;
    public float gravity;

    public float speed = 5f;

    public float runSpeed; 

    public float jumpHeight;

    


    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = -40f;
        jumpHeight = 1.0f;
        runSpeed = 10f;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

    }

    public void ProcessMove(Vector2 input)
    {

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;   


        if (isGrounded && playerVelocity.y <0)
        
            playerVelocity.y = -2;
        
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }

    public void Sprint()
    {
        speed = runSpeed;  

    }

    public void StopSprint()
    {
        speed = 5f;
    }

    public void Jump()
    {
        if (isGrounded) 
        {
          playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        
        }

    }
}
