using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField,Range(0,24)] private float timeOfDay;
    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;
    [Header("SkyBoxPreset")]
    [SerializeField] private Material SkyboxM;
    [SerializeField,Range(0,360)] private float rotation;
    [SerializeField] private Gradient SkyBoxColor;

    private void Update()
    {
        timeOfDay += Time.deltaTime;
        if (timeOfDay>24)
            timeOfDay = 0;
        UpdateSunRotation();
        UpdateLighting();
        UpdateMaterialParameter();
    }

    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();
        UpdateMaterialParameter();
    }

    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation=Quaternion.Euler(sunRotation,sun.transform.rotation.y,sun.transform.rotation.z);
    }

    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor=equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor=skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }

    private void UpdateMaterialParameter()
    {
        SkyboxM.SetFloat("_Rotation", rotation);
        float timeFration = timeOfDay / 24;
        SkyboxM.SetColor("_Tint", SkyBoxColor.Evaluate(timeFration));
   }
}
