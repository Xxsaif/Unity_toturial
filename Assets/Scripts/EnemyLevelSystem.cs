using UnityEngine;

public class EnemyLevelSystem : MonoBehaviour
{
    public int enemyLevel;
    public float EnemyDamageMultiplier { get { return Mathf.Pow(1.1f, enemyLevel - 1); } }
    public float EnemyHealthMultiplier { get { return Mathf.Pow(1.1f, enemyLevel - 1); } }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
