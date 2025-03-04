using UnityEngine;
using UnityEngine.VFX;

public abstract class interactable : MonoBehaviour
{

    public string promtMessege;


    public void BasseInteract()
    {

        Interact();
    }
   

    protected virtual void Interact()
    {

    }
}
