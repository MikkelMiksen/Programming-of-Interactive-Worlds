using UnityEngine;

public class MJ_BodyTemp : MonoBehaviour
{
    public static MJ_BodyTemp Instance; void Awake() { Instance = this; }

    public float bodyTemp = 37f;

    public float minSafeBodyTemp = 28f;

    public float maxSafeBodyTemp = 41f;

    public float temperatureChangeRate = 0.02f;

    public float currentAreaTemp;

    public bool isBodyTempSafe, isBodyTempFreezing, isBodyTempOverheating;

    public float hpInfluenceCoefficient;

    public string readBodyTemp;

    void Start()
    {
        currentAreaTemp = DayNightCycle.Instance.temp;
    }

    void Update()
    {
        UpdateBodyTemperature();
        CheckTemperatureEffects();
    }

    private void UpdateBodyTemperature()
    {
        float environmentTemp = currentAreaTemp;

        // Body temperature moves toward environment temperature gradually
        float delta = environmentTemp - bodyTemp;
        bodyTemp += delta * temperatureChangeRate * Time.deltaTime;

        // Clamp within humanly possible limits
        bodyTemp = Mathf.Clamp(bodyTemp, 14.3f, 45f);
    }

    private void CheckTemperatureEffects()
    {
        if (bodyTemp >= minSafeBodyTemp && bodyTemp <= maxSafeBodyTemp)
        {
            readBodyTemp = $"Body temp: {bodyTemp:F1}°C";
            isBodyTempSafe = true;
            isBodyTempFreezing = false;
            isBodyTempOverheating = false;
            hpInfluenceCoefficient = 1;

        }
        else if (bodyTemp <= minSafeBodyTemp)
        {
            readBodyTemp = $"Hypothermia risk! Body temp: {bodyTemp:F1}°C";
            isBodyTempSafe = false;
            isBodyTempFreezing = true;
            isBodyTempOverheating = false;
            hpInfluenceCoefficient = 0.96f;
        }
        else if (bodyTemp >= maxSafeBodyTemp)
        {
            readBodyTemp = $"Overheating risk! Body temp: {bodyTemp:F1}°C";
            isBodyTempSafe = false;
            isBodyTempFreezing = false;
            isBodyTempOverheating = true;
            hpInfluenceCoefficient = 0.96f;
        }
    }
}
