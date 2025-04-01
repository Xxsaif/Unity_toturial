using UnityEngine;

public class Slot : MonoBehaviour
{
    [HideInInspector] public bool hovered;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Hover()
    {
        hovered = true;
    }

    public void UnHover()
    {
        hovered = false;
    }
}
