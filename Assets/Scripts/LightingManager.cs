using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 
// Created and written by Oliver
// Edited by Louis
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    // - References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    // Skybox
    [SerializeField] private Material skybox;
    private readonly Color dayTopColor = new Color(90f, 155f, 255f) / 255f;
    private float startHorizonExponent;
    private float startHorizonContribution;
    private Color startSkyTop;
    // -  Variables
    [SerializeField, Range(0, 24)]private float TimeOfDay;
    [SerializeField] private Text timeText;
    private readonly float timeSpeed = 0.1f;


    private void Start()
    {
        startHorizonExponent = skybox.GetFloat("_HorizonLineExponent");
        startHorizonContribution = skybox.GetFloat("_HorizonLineContribution");
        startSkyTop = skybox.GetColor("_SkyGradientTop");
    }
    private void Update()
    {
        if (Preset == null)
            return;

        if(Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * timeSpeed;
            TimeOfDay %= 24;
            UpdateSkybox();
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


    private void UpdateSkybox()
    {
        if (TimeOfDay < 6 || TimeOfDay >= 18) // Night
        {
            skybox.SetColor("_SkyGradientTop", new Color(0f, 0f, 0f));
        }
        if (TimeOfDay < 4 || TimeOfDay >= 22) // Night
        {
            RenderSettings.ambientIntensity = 0.3f;
            skybox.SetColor("_SunHaloColor", new Color(255f, 0f, 0f) / 255f);
        }
        if (TimeSlot(0, 6)) // Night to sunrise
        {
            skybox.SetFloat("_HorizonLineExponent", Mathf.Lerp(12f, 0f, TimeOfDay / 6f));

        }
        if (TimeSlot(4, 6)) // Sunrise
        {
            skybox.SetFloat("_HorizonLineContribution", (TimeOfDay - 4f) / 2f);
        }
        if (TimeSlot(5, 7))
        {
            skybox.SetColor("_SunHaloColor", new Color(255f, Mathf.Lerp(0f, 255f, (TimeOfDay - 5f) / 2f), 0f) / 255f);
            skybox.SetColor("_SkyGradientTop", new Color(90, 155f, 255f) * ((TimeOfDay - 5f) / 2f) / 255f);
            RenderSettings.ambientIntensity = Mathf.Lerp(0.3f, 1, (TimeOfDay - 5f) / 2f);

        }
        if (TimeSlot(7, 16)) // Day
        {
            RenderSettings.ambientIntensity = 1f;
            //skybox.SetColor("_SkyGradientTop", dayTopColor);
        }
        if (TimeSlot(16, 20)) // Sunset
        {
            skybox.SetFloat("_HorizonLineContribution", Mathf.Lerp(1f, 0f, (TimeOfDay - 16f) / 4f));
            skybox.SetColor("_SunHaloColor", new Color(255f, Mathf.Lerp(255f, 0f, (TimeOfDay - 16f) / 4f), 0f) / 255f);
        }
        if (TimeSlot(16, 20)) // Sunset
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(1f, 0.3f, (TimeOfDay - 16f) / 4f);
        }
        
    }

    
    private bool TimeSlot(float startTime, float endTime) => TimeOfDay > startTime && TimeOfDay <= endTime;

    private void OnApplicationQuit()
    {
        skybox.SetFloat("_HorizonLineExponent", startHorizonExponent);
        skybox.SetFloat("_HorizonLineContribution", startHorizonContribution);
        skybox.SetColor("_SkyGradientTop", startSkyTop);
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
