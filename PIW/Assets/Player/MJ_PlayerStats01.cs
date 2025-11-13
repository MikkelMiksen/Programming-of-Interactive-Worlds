using System;
using TMPro;
using UnityEngine;

public class MJ_PlayerStats01 : MonoBehaviour
{
    public static MJ_PlayerStats01 Instance; void Awake() { Instance = this; }
    [Header("Core Stats")]
    [SerializeField] public float hp = 100f;
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float hpRecoveryRate = 0.02f;
    [SerializeField] private float hpLossRateBase = 0.03f;

    [Header("Body Energy / Fat")]
    [SerializeField] public float fatPct = 60f;  // 0–100: long-term energy & insulation
    [SerializeField] public float fatLossPerHour = 0.03f;
    [SerializeField] public float fatGainPerMeal = 5f;
    [SerializeField] private float starvationThreshold = 25f;

    [Header("Clothing Insulation (0–1 each)")]
    [SerializeField] public float jacketInsulation = 0f;
    [SerializeField] public float pantsInsulation = 0f;
    [SerializeField] public float bootsInsulation = 0f;
    [SerializeField] public float hatInsulation = 0f;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI bodyTempText;


    private DayNightCycle dayNightCycle;
    private MJ_BodyTemp bodyTemp;
    private Color[] colors = new Color[3] { Color.cyan, Color.grey, Color.red };

    private float insulationTotal => jacketInsulation + pantsInsulation + bootsInsulation + hatInsulation;
    private string hpDispText => $"HP {(int)hp} / {(int)maxHP}";
    private string bodyDispText => bodyTemp.readBodyTemp;

    private float hoursSinceLastMeal = 0f;
    private DateTime lastFatUpdateTime;

    void Start()
    {
        bodyTemp = MJ_BodyTemp.Instance;
        dayNightCycle = DayNightCycle.Instance;

        lastFatUpdateTime = DayNightCycle.Instance.currentTime;
    }


    void Update()
    {
        SimulateFatOverTime();
        UpdateHealthLogic();
        UpdateUI();
    }

    private void SimulateFatOverTime()
    {
        TimeSpan elapsed = DayNightCycle.Instance.currentTime - lastFatUpdateTime;

        float inGameHours = (float)elapsed.TotalHours;

        hoursSinceLastMeal += inGameHours;

        // Natural fat burn
        fatPct -= fatLossPerHour * inGameHours;

        // Starvation speeds it up
        if (hoursSinceLastMeal > 8f)
            fatPct -= fatLossPerHour * inGameHours * 2f;

        fatPct = Mathf.Clamp(fatPct, 0f, 100f);

        lastFatUpdateTime = DayNightCycle.Instance.currentTime;
    }

    private void UpdateHealthLogic()
    {
        float totalInsulation = insulationTotal + Mathf.Lerp(0f, 1f, fatPct / 100f);
        float hpTempMultiplier = bodyTemp.hpInfluenceCoefficient;

        // ❄ Cold damage
        if (bodyTemp.isBodyTempFreezing)
        {
            float coldPenalty = (1f - totalInsulation * 0.1f) * hpLossRateBase * (37f - bodyTemp.bodyTemp);
            hp -= coldPenalty * Time.deltaTime * 10f;
        }
        // ☀ Heat damage
        else if (bodyTemp.isBodyTempOverheating)
        {
            float heatPenalty = hpLossRateBase * (bodyTemp.bodyTemp - 37f);
            hp -= heatPenalty * Time.deltaTime * 5f;
        }
        // ❤️ Recovery
        else if (bodyTemp.isBodyTempSafe && fatPct > 40f)
        {
            hp += hpRecoveryRate * hpTempMultiplier * Time.deltaTime * 20f;
        }

        // 🍖 Starvation penalty
        if (fatPct < starvationThreshold)
        {
            float starvationPenalty = (starvationThreshold - fatPct) * 0.01f;
            hp -= starvationPenalty * Time.deltaTime * 10f;
        }

        hp = Mathf.Clamp(hp, 0f, maxHP);
    }

    private void UpdateUI()
    {
        hpText.text = hp <= 0 ? "Player has died" : hpDispText;
        bodyTempText.text = bodyDispText;

        if (bodyTemp.isBodyTempFreezing)
            bodyTempText.color = colors[0];
        else if (bodyTemp.isBodyTempSafe)
            bodyTempText.color = colors[1];
        else if (bodyTemp.isBodyTempOverheating)
            bodyTempText.color = colors[2];
        else
            bodyTempText.color = colors[0];
    }

    // 🍖 Call this when the player eats something
    public void EatMeal()
    {
        fatPct += fatGainPerMeal;
        fatPct = Mathf.Clamp(fatPct, 0f, 100f);
        hoursSinceLastMeal = 0f;
        Debug.Log($"🍖 Ate meal: fat {fatPct:F1}%");
    }

    // 🧥 Call this when equipping clothing (values between 0–1)
    public void ApplyClothing(float jacket, float pants, float boots, float hat)
    {
        jacketInsulation = jacket;
        pantsInsulation = pants;
        bootsInsulation = boots;
        hatInsulation = hat;
    }
}
