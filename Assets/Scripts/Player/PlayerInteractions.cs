using TMPro;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;
    private GameObject interactionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractableObject>(out _))
        {
            interactionObject = other.gameObject;
            interactionObject.GetComponent<InteractableObject>().InteractRangeEnter();
        }

        
    }

    private void Update()
    {
        if (interactionObject != null)
        {
            if (interactionObject.TryGetComponent<InteractableObject>(out _))
            {
                interactionObject.GetComponent<InteractableObject>().InteractRangeStay();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactionObject.GetComponent<InteractableObject>().Interact();
                }
            }
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractableObject>(out _))
        {
            interactionObject.GetComponent<InteractableObject>().InteractRangeExit();
            interactionObject = null;
        }
    }
}
