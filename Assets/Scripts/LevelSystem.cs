using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static float playerLevel;
    public static float experience;
    public static float PlayerDamageMultiplier { get {  return Mathf.Pow(1.05f, playerLevel - 1); } }

    void Start()
    {
        playerLevel = 1;
        experience = 0;
    }

    
    void Update()
    {
        if (experience >= ExperienceRequirement())
        {
            playerLevel++;
            experience = 0;
        }
    }

    private static float ExperienceRequirement() => Mathf.Pow(playerLevel + 1, 3f) * 10f;

    public static void IncreaseExperience(float amount)
    {
        //Debug.Log("Experience Gained: (" + experience + " / " + ExperienceRequirement() + ") -> (" + (experience + amount) + " / " + ExperienceRequirement() + ")");
        experience += amount;
        if (experience >= ExperienceRequirement())
        {
            playerLevel++;
            experience = 0;
        }
    }
}
