using UnityEngine;

public class Visable : MonoBehaviour
{
    [HideInInspector] public bool visable = false;
    
    private void OnBecameVisible()
    {
        visable = true;
    }

    private void OnBecameInvisible()
    {
        visable = false;
    }
}
