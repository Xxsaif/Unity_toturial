using UnityEngine;
// Created and written by Oliver
// Edited by
[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptable Objects/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
