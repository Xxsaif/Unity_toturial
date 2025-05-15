using TMPro;
using UnityEngine;
// Created by Herman Bergström
public class QuestManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Inventory chestInventory;
    [SerializeField] private DroppedItem[] missionItems;
    [SerializeField] private GameObject gameoverScreen;
    private int currentQuestId = 0;
    void Start()
    {
        questText.text = GetQuestText(currentQuestId);
        missionItems[0].waypointActive = true;
    }

    
    void Update()
    {
        if (IsQuestDone(currentQuestId))
        {
            NextQuest();
        }
        //if (Input.GetKeyUp(KeyCode.T)) // Instant win button for testing gamer over screen
        //{
        //    currentQuestId = 5;
        //    NextQuest();
        //}
    }

    private bool IsQuestDone(int id)
    {
        switch (id)
        {
            case 0:
                return playerInventory.ContainsType(Item.ItemType.Rock);

            case 1:
                return chestInventory.ContainsType(Item.ItemType.Rock);

            case 2:
                return playerInventory.ContainsType(Item.ItemType.Stick);

            case 3:
                return chestInventory.ContainsType(Item.ItemType.Stick);

            case 4:
                return playerInventory.ContainsType(Item.ItemType.String);

            case 5:
                return chestInventory.ContainsType(Item.ItemType.String);
        }
        return false;
    }

    private void NextQuest()
    {
        currentQuestId++;
        questText.text = GetQuestText(currentQuestId);
        switch (currentQuestId)
        {
            case 2:
                missionItems[1].waypointActive = true;
                break;

            case 4:
                missionItems[2].waypointActive = true;
                break;
            case 6:
                Time.timeScale = 0;
                GameObject player = GameObject.Find("Player");
                GameObject playerCam = GameObject.Find("PlayerCam");
                player.GetComponent<PlayerController>().enabled = false;
                player.GetComponent<PlayerInteractions>().enabled = false;
                playerCam.GetComponent<MouseLook>().enabled = false;
                gameoverScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

        }
    }

    

    private string GetQuestText(int id)
    {
        switch (id)
        {
            case 0:
                return "Quest 1: Acquire a rock";

            case 1:
                return "Quest 2: Return the rock to the chest";

            case 2:
                return "Quest 3: Acquire a stick";

            case 3:
                return "Quest 4: Return the stick to the chest";

            case 4:
                return "Quest 5: Acquire a string";

            case 5:
                return "Quest 6: Return the string to the chest";
        }
        return string.Empty;
    }
}
