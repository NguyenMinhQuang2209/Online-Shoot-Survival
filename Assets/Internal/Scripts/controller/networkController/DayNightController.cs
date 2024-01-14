using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightController : NetworkBehaviour
{
    private NetworkVariable<int> currentDay = new(0);
    private NetworkVariable<float> currentHour = new(0);
    private NetworkVariable<FixedString128Bytes> currentTimeString = new();
    [SerializeField] private float startAtHour = 0f;

    [Tooltip("Sun rise always greater than 0f")]
    [SerializeField] private float sunRiseAtHour = 7f;
    [SerializeField] private float sunSetAtHour = 20f;
    [SerializeField] private float timeRate = 600f;

    [SerializeField] private TextMeshProUGUI dayNightTxt;
    [SerializeField] private Light2D light2d;
    [SerializeField] private float sunSetValue = 0.5f;
    DateTime currentTime;
    DateTime startTime;
    TimeSpan sunRiseTime;
    TimeSpan sunSetTime;
    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startAtHour);
        startTime = DateTime.Now.Date;

        sunRiseTime = TimeSpan.FromHours(sunRiseAtHour);
        sunSetTime = TimeSpan.FromHours(sunSetAtHour);

    }
    private void Update()
    {
        if (IsServer)
        {
            currentDay.Value = GetCurrent();
            currentTime += TimeSpan.FromSeconds(Time.deltaTime * timeRate);
            currentHour.Value = currentTime.Hour + currentTime.Minute / 60f;
            currentTimeString.Value = new(currentTime.ToString("HH:mm"));
        }


        dayNightTxt.text = "Ngày " + currentDay.Value + "\n" + currentTimeString.Value;
        TimeSpan currentTimeSpawn = TimeSpan.FromHours(currentHour.Value);
        if (currentTimeSpawn >= sunRiseTime && currentTimeSpawn <= sunSetTime)
        {
            light2d.intensity = 1f;
        }
        else
        {
            light2d.intensity = sunSetValue;
        }
    }
    private int GetCurrent()
    {
        return (currentTime - startTime).Days;
    }
    public float GetCurrentHour()
    {
        return currentHour.Value;
    }
    public int GetCurrentDay()
    {
        return currentDay.Value;
    }
}
