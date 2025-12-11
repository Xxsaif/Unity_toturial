using TMPro;
using UnityEngine;
// Created by Herman Bergstr�m
// Modified by Louis Gericke
public class DroppedItem : MonoBehaviour, InteractableObject
{
    private Hotbar hotbar;  // Changed from Inventory to Hotbar
    public Item item;
    public Item.ItemType type;
    public int quantity;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject waypoint;
    private Camera playerCam;
    private Transform playerTransform;
    [SerializeField] private Collider objCollider;
    [HideInInspector] public bool waypointActive;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        hotbar = player.GetComponent<Hotbar>();
        playerTransform = player.GetComponent<Transform>();
        item = new Item(type, quantity);
        placementIndicator.SetActive(false);
        objects[(int)type].SetActive(true);
        playerCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
    }

    // If the quest script tells us that this dropped item is part of an active quest we have to calculate 
    // the world to screen position and draw a waypoint icon on ourselfs.
    void Update()
    {
        waypoint.SetActive(waypointActive && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(playerCam), objCollider.bounds));
        if (waypoint.activeSelf)
        {
            waypoint.transform.position = playerCam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
        }
    }

    public void InteractRangeEnter()
    {
        interactionText.text = "Press F to\nPick up " + item.Name;
    }

    public void InteractRangeStay()
    {
        interactionText.text = "Press F to\nPick up " + item.Name;
    }

    public void InteractRangeExit()
    {
        interactionText.text = string.Empty;
    }

    public void Interact()
    {
        hotbar.AddItem(type, quantity);
        interactionText.text = string.Empty;
        gameObject.SetActive(false);
    }
}