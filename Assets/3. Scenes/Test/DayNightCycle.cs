using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun;

    [SerializeField] private float sunRotationY;
    [SerializeField,Range(0,144)] private float timeOfDay;
    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private Gradient FogColor;
    
    [Header("SkyBoxPreset")]
    [SerializeField] private Material SkyboxM;
    [SerializeField,Range(0,360)] private float rotation;
    [SerializeField] private Gradient SkyBoxColor;
    [SerializeField] private float timeSpeed;

    private void Update()
    {
        //timeOfDay += Time.deltaTime;
        /*if (timeOfDay>24)
            timeOfDay = 0;*/
        /*UpdateSunRotation();
        UpdateLighting();
        UpdateMaterialParameter();*/
    }

    private void Start()
    {
        Init();

        EventManager.Subscribe(EventType.Minute, Rotation);
    }

    void Rotation()
    {
        StopAllCoroutines();

        timeOfDay = GameManager.Instance.GameTime.GetTimeIdx();
        StartCoroutine(CRotation());
    }
    
    IEnumerator CRotation()
    {
        var cycle = GameManager.Instance.GameTime.GetGameSpeed();
        var second = 0f;
        var time = timeOfDay;
        while (true)
        {
            timeOfDay = time + Mathf.Lerp(0, 1, second / cycle);
            second += Time.deltaTime;
            UpdateSunRotation();
            UpdateLighting();
            UpdateMaterialParameter();

            yield return null;
        }
    }

    void Init()
    {
        var hour = GameManager.Instance.GameTime.GetHour();
        var minute = GameManager.Instance.GameTime.GetMinute();
        
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
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 144f);
        sun.transform.rotation=Quaternion.Euler(sunRotation,sunRotationY,sun.transform.rotation.z);
    }

    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 144f;
        RenderSettings.ambientEquatorColor=equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor=skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
        RenderSettings.fogColor=FogColor.Evaluate(timeFraction);
    }

    private void UpdateMaterialParameter()
    {
        SkyboxM.SetFloat("_Rotation", rotation);
        float timeFration = timeOfDay / 144f;
        SkyboxM.SetColor("_Tint", SkyBoxColor.Evaluate(timeFration));
   }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventManager.Unsubscribe(EventType.Minute, Rotation);
    }
}
