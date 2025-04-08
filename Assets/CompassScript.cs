using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour
{
    public Transform player;
    public Text compassText;
    private string[] directions = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };

    void Update()
    {
        float angle = player.eulerAngles.y;
        int index = Mathf.RoundToInt(angle / 45f) % 8;
        compassText.text = directions[index];
    }
}
