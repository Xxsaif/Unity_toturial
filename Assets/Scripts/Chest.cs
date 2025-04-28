using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour, InteractableObject
{
    [HideInInspector] public Item[,] chestItems = new Item[3, 6];
    [HideInInspector] public TextMeshProUGUI[,] chestSlotIcons = new TextMeshProUGUI[3, 6];
    [HideInInspector] public TextMeshProUGUI[,] chestSlotQuantity = new TextMeshProUGUI[3, 6];
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private Inventory inventory;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void InteractRangeEnter()
    {
        interactionText.text = "Press F to open chest";
    }

    public void InteractRangeStay()
    {
        
    }

    public void InteractRangeExit()
    {
        interactionText.text = string.Empty;
    }

    public void Interact()
    {
        inventory.ChangeActiveState();
    }
}
