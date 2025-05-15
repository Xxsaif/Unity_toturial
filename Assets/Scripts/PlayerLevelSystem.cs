using UnityEngine;
// Created by Herman Bergström
public class PlayerLevelSystem : MonoBehaviour
{
    public static float playerLevel;
    public static float experience;
    public float PlayerDamageMultiplier { get { return Mathf.Pow(1.05f, playerLevel - 1); } }
    public float PlayerHealthMultiplier { get { return Mathf.Pow(1.05f, playerLevel - 1); } }
    public float PlayerStaminaMultiplier { get { return Mathf.Pow(1.05f, playerLevel - 1); } }
    private PlayerHealth playerHealthScr;
    private PlayerController playerController;


    void Start()
    {
        playerLevel = 1;
        experience = 0;
        playerHealthScr = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();

    }


    void Update()
    {
        if (experience >= ExperienceRequirement())
        {
            playerLevel++;
            experience = 0;
        }
    }

    private float ExperienceRequirement() => Mathf.Pow(playerLevel + 1, 3f) * 10f;

    public void IncreaseExperience(float amount)
    {
        //Debug.Log("Experience Gained: (" + experience + " / " + ExperienceRequirement() + ") -> (" + (experience + amount) + " / " + ExperienceRequirement() + ")");
        experience += amount;
        if (experience >= ExperienceRequirement())
        {
            experience -= ExperienceRequirement();
            playerLevel++;
            playerHealthScr.UpdateMaxHealth();
            playerController.UpdateMaxStamina();
        }
    }
}
