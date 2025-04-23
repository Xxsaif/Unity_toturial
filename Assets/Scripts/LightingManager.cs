using UnityEngine;
using UnityEngine.UI; 
// Created and written by Oliver
// Edited by Louis
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //References
    [SerializeField]private Light DirectionalLight;
    [SerializeField]private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)]private float TimeOfDay;
    [SerializeField] private Text timeText;


    private void Update()
    {
        if (Preset == null)
            return;

        if(Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }

        UpdateTimeUI();
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170, 0));
        }
    }

    private void UpdateTimeUI()
    {
        if (timeText == null) return;

        int hours = Mathf.FloorToInt(TimeOfDay);
        int minutes = Mathf.FloorToInt((TimeOfDay - hours) * 60f);

        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

//    private void OnValidate()
//    {
//        if (DirectionalLight != null)
//            return;

//        if(RenderSettings.sun != null)
//        {
//            DirectionalLight = RenderSettings.sun;
//        }
//        else
//        {
//            Light[] lights = GameObject.FindFirstObjectByType<Light>();
//            foreach (Light light in lights)
//            {
//                if(light.type == LightType.Directional)
//                {
//                    DirectionalLight = light;
//                    return;
//                }

//            }
//        }
//    }


}
