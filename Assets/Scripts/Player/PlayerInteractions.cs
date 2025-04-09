using TMPro;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;
    private GameObject interactionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Door>(out _))
        {
            interactionObject = other.gameObject;
        }
    }

    private void Update()
    {
        if (interactionObject != null)
        {
            if (interactionObject.TryGetComponent<Door>(out _))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactionObject.GetComponent<Door>().ChangeState(transform.position);
                }
                Door doorScr = interactionObject.GetComponent<Door>();
                interactionText.text = "Press F to\n" + (doorScr.state == Door.State.Closed ? "open" : "close") + " gate";
            }
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Door>(out _))
        {
            interactionText.text = string.Empty;
            interactionObject = null;
        }
    }
}
