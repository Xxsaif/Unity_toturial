using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInteract : MonoBehaviour
{
    
    private Camera cam;
    [SerializeField]private float distance = 3f;

    [SerializeField] private LayerMask mask;

    private PlayerUI playerUI;
    void Start()
    {

        cam = GetComponent<PlayerLook>().cam;

        playerUI = GetComponent<PlayerUI>();
        
    }

   
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        //skjuter en ray från cameran som är lika lång som distance
        Ray ray = new Ray(cam.transform.position , cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        //sparar info om ray
        RaycastHit hitInfo;

        //kollar ifall ray har kolliderat med något

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {

            //kollar ifall ray har kolliderat med ett "collidable object";
            if (hitInfo.collider.GetComponent<interactable>() != null)
            {
                playerUI.UpdateText(hitInfo.collider.GetComponent<interactable>().promtMessege);

                Debug.Log("Interacted with this ");

            }

        }
       
        
    }
}
