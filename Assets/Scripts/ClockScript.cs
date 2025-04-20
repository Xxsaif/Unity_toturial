using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public Text clockText;
    public float speed = 10f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime * speed;
        if ( timer >= 100f ) timer = 0f;
        float totalHours = ( timer / 100f ) * 24f;
        int hours = Mathf.FloorToInt(totalHours) % 24;
        int minutes = Mathf.FloorToInt(( totalHours - hours ) * 60f);
        clockText.text = hours.ToString("00") + ":" + minutes.ToString("00");
    }
}
