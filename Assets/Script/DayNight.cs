using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Range(0f, 1f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    public float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    [Header("Day Text")]
    public TextMeshProUGUI dayText;

    private int dayNum;
    public bool nextDay;

    Resource resource;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        dayNum = 0;
        nextDay = false;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        LightingUpdate(sun, sunColor, sunIntensity);
        LightingUpdate(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        if(time >= 0.99f && !nextDay)
        {
            dayNum += 1;
            nextDay = true;
        }

        if(time < 0.99f)
        {
            nextDay = false;
        }

        dayText.text = $"Day : {dayNum.ToString()}";
    }

    private void LightingUpdate(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        GameObject go = lightSource.gameObject;

        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }

        else if(lightSource.intensity > 0 && go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
