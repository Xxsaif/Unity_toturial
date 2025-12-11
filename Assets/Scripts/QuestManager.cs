using TMPro;
using UnityEngine;
// Created by Herman Bergström
public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Hotbar playerHotbar;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Inventory chestInventory;
    [SerializeField] private DroppedItem[] missionItems;
    [SerializeField] private GameObject gameoverScreen;

    private int currentItemIndex = 0;
    private bool isDepositPhase = false;
    private bool questCompleted = false;

    void Start()
    {
        if (missionItems.Length > 0)
        {
            UpdateQuestText();
            missionItems[0].waypointActive = true;
        }
    }

    void Update()
    {
        if (!questCompleted && currentItemIndex < missionItems.Length && IsCurrentQuestDone())
        {
            NextQuest();
        }
    }

    private bool PlayerHasItem(Item.ItemType type)
    {
        return playerHotbar.ContainsType(type) || playerInventory.ContainsType(type);
    }

    private bool ChestHasItem(Item.ItemType type)
    {
        return chestInventory.ContainsType(type);
    }

    private bool IsCurrentQuestDone()
    {
        Item.ItemType currentType = missionItems[currentItemIndex].type;

        if (!isDepositPhase)
        {
            // Pickup phase - player has it OR it's already in chest (skipped ahead)
            return PlayerHasItem(currentType) || ChestHasItem(currentType);
        }
        else
        {
            // Deposit phase - check if chest has the item
            return ChestHasItem(currentType);
        }
    }

    private void NextQuest()
    {
        Item.ItemType currentType = missionItems[currentItemIndex].type;

        if (!isDepositPhase)
        {
            // Check if item is already in chest (player skipped ahead)
            if (ChestHasItem(currentType))
            {
                // Skip directly to next item
                currentItemIndex++;

                if (currentItemIndex >= missionItems.Length)
                {
                    questCompleted = true;
                    TriggerWin();
                    return;
                }

                missionItems[currentItemIndex].waypointActive = true;
                UpdateQuestText();
            }
            else
            {
                // Normal flow - move to deposit phase
                isDepositPhase = true;
                UpdateQuestText();
            }
        }
        else
        {
            // Completed deposit - move to next item
            currentItemIndex++;

            if (currentItemIndex >= missionItems.Length)
            {
                questCompleted = true;
                TriggerWin();
                return;
            }

            isDepositPhase = false;
            missionItems[currentItemIndex].waypointActive = true;
            UpdateQuestText();
        }
    }

    private void UpdateQuestText()
    {
        string itemName = missionItems[currentItemIndex].type.ToString();
        int questNumber = (currentItemIndex * 2) + (isDepositPhase ? 2 : 1);

        if (!isDepositPhase)
        {
            questText.text = $"Quest {questNumber}: Acquire a {itemName}";
        }
        else
        {
            questText.text = $"Quest {questNumber}: Return the {itemName} to the chest";
        }
    }

    private void TriggerWin()
    {
        questText.text = "All items collected! You win!";
        Time.timeScale = 0;
        GameObject player = GameObject.Find("Player");
        GameObject playerCam = GameObject.Find("PlayerCam");
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInteractions>().enabled = false;
        playerCam.GetComponent<MouseLook>().enabled = false;
        gameoverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}