using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Events;

public class DayCycle : MonoBehaviour
{
    //Used https://www.youtube.com/watch?v=is-OijFIC9o&t=454s to help with the day coding
    //Used https://www.youtube.com/watch?v=UqQsfRUdukE to help with the night coding
    [Header("Time Settings")]
    [Range(0f, 24f)]
    public float currentTime;
    public float timeSpeed = 1f;

    [Header("CurrentTime")]
    public string currentTimeString;

    [Header("Sun Settings")]
    public Light sunLight;
    [Range(0f, 90f)]
    public float sunLatitude = 20f;
    [Range(-180f, 180f)]
    public float sunLongitude = -90f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve sunTemperatureCurve;

    public bool isDay = true;
    public bool sunActive = true;
    public bool moonActive = true;

    [Header("Moon Settings")]
    public Light moonLight;
    [Range(0f, 90f)]
    public float moonLatitude = 40f;
    [Range(-180f, 180f)]
    public float moonLongitude = 90f;
    public float moonIntensity = 1f;
    public AnimationCurve moonIntensityMultiplier;
    public AnimationCurve moonTemperatureCurve;

    [Header("Stars")]
    public VolumeProfile volumeProfile;
    private PhysicallyBasedSky skySettings;
    public float starsIntensity = 1f;
    public AnimationCurve starsCurve;
    [Range(0f, 90f)]
    public float polarStarLatitude = 40f;
    [Range(-180f, 180f)]
    public float polarStarLongitude = 90f;

    [Header("Events")]
    public UnityEvent EnterDay; //this triggers when the day cycle begins and calls all the necessary methods
    public UnityEvent EnterNight;//this triggers when the night cycle begins and calls all the necessary methods

    void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
        SkyStar();

        
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        //Makes sure than once the clock hits 24 it circle back to 0 and resets the cycle
        if (currentTime >= 24)
        {
            currentTime = 0;
        }

        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
        SkyStar();
    }

    private void OnValidate()
    {
        UpdateLight();
        CheckShadowStatus();
        SkyStar();
    }

    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ":" + ((currentTime % 1) * 60).ToString("00");
    }

    void UpdateLight()
    {
        //Adds Longitude and Latitude rotation to both the sun and moon while setting the rotation with the time of day
        float sunRotation = currentTime / 24f * 360f;
        sunLight.transform.localRotation = (Quaternion.Euler(sunLatitude - 90, sunLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));
        moonLight.transform.localRotation = (Quaternion.Euler(90 - moonLatitude, moonLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));

        //Adds additional lighting data as well as an animation curve to change the light intensity throughout the day
        float normalizedTime = currentTime / 24f;
        float sunIntensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);
        float moonIntensityCurve = moonIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonLightData = moonLight.GetComponent<HDAdditionalLightData>();

        if (sunLightData != null)
        {
            sunLightData.intensity = sunIntensityCurve * sunIntensity;
        }

        if (moonLightData != null)
        {
            moonLightData.intensity = moonIntensityCurve * moonIntensity;
        }

        //Add an animation curve for color temperature throughout the day
        float suntemperatureMultiplier = sunTemperatureCurve.Evaluate(normalizedTime);
        float moontemperatureMultiplier = moonTemperatureCurve.Evaluate(normalizedTime);
        Light sunlightComponent = sunLight.GetComponent<Light>();
        Light moonlightComponent = moonLight.GetComponent<Light>();

        if (sunlightComponent != null)
        {
            sunlightComponent.colorTemperature = suntemperatureMultiplier * 10000f;
        }

        if (moonlightComponent != null)
        {
            moonlightComponent.colorTemperature = moontemperatureMultiplier * 10000f;
        }
    }

    void CheckShadowStatus()
    {
        //Day: Enables shadows from the sun during the day and disables them at night
        //Night: Enables shadows from the moon during the night and disables them at day
        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonLightData = moonLight.GetComponent<HDAdditionalLightData>();
        float currentSunRotation = currentTime;

        if (currentSunRotation >= 5.7f && currentSunRotation <= 17.7f)
        {
            sunLightData.EnableShadows(true);
            moonLightData.EnableShadows(false);
            isDay = true;

            //call day event
            EnterDay.Invoke();
        }

        else
        {
            sunLightData.EnableShadows(false);
            moonLightData.EnableShadows(true);
            isDay = false;

            //call night event
            EnterNight.Invoke();
        }

        if (currentSunRotation >= 5.7f && currentSunRotation <= 18.3f)
        {
            //sunLight.gameObject.SetActive(true);
            sunActive = true;
        }

        else
        {
            //sunLight.gameObject.SetActive(false);
            sunActive = false;
        }

        if (currentSunRotation >= 6.3f && currentSunRotation <= 17.7f)
        {
            //moonLight.gameObject.SetActive(false);
            moonActive = false;
        }

        else
        {
            //moonLight.gameObject.SetActive(true);
            moonActive = true;
        }
    }

    void SkyStar()
    {
        volumeProfile.TryGet<PhysicallyBasedSky>(out skySettings);
        skySettings.spaceEmissionMultiplier.value = starsCurve.Evaluate(currentTime / 24.0f) * starsIntensity;

        skySettings.spaceRotation.value = (Quaternion.Euler(90 - polarStarLatitude, polarStarLongitude, 0) * Quaternion.Euler(0, currentTime / 24.0f * 360.0f, 0)).eulerAngles;
    }
}