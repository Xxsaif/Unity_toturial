using Unity.VisualScripting;
using UnityEngine;

public class KeyPad : interactable
{
    void Start()
    {
        //Debug.Log("not interacting with anything with");


    }

    void Update()
    {

        //Interact();


    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision");
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        Interact();
    //    }
    //}

    protected override void Interact()
    {
        Debug.Log("interacting");


    }
}
