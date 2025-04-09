using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen && other.CompareTag("Player"))
        {
            transform.Rotate(0f, 90f, 0f);
            isOpen = true;
        }
        //else if(isOpen && other.CompareTag("Player"))
        //{
        //    transform.Rotate(0f,-90f,0f);
        //    isOpen = false;
        //}

    }


    private void OnTriggerExit(Collider other)
    {
        if (isOpen && other.CompareTag("Player"))
        {
            transform.Rotate(0f, -90f, 0f); // rotate back
            isOpen = false;
        }
    }
}
