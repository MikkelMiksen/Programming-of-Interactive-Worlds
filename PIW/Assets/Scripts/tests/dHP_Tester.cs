using UnityEngine;

public class dHP_Tester : MonoBehaviour
{
    [Header("Status Flags")]
    [SerializeField] private bool isTemperatureSafe = true;
    [SerializeField] private bool isMoving = false;

    [Header("Core Stats")]
    [SerializeField] private float hp = 100f;
    [SerializeField] private float stamina = 1f;     // 0–1
    [SerializeField] private float fatPct = 100f;    // 0–100
    [SerializeField] private float timeSinceFoodConsumed = 0f;

    [Header("Rates & Constants")]
    [SerializeField] private float staminaDrainRate = 0.15f;
    [SerializeField] private float staminaRecoveryRate = 0.08f;
    [SerializeField] private float fatBurnRate = 0.05f;
    [SerializeField] private float hungerSpeed = 0.01f;
    [SerializeField] private float unsafeTempMultiplier = 2f; // doubles drain when cold
    [SerializeField] private float hpDecayBase = 0.05f;

    void Update()
    {
        // Update hunger timer
        timeSinceFoodConsumed += Time.deltaTime * (isMoving ? 1.2f : 1f);
        timeSinceFoodConsumed = Mathf.Clamp(timeSinceFoodConsumed, 0f, 100f);

        // 🔹 Stamina updates
        float dStamina = isMoving ? -staminaDrainRate : staminaRecoveryRate;
        stamina = Mathf.Clamp01(stamina + dStamina * Time.deltaTime);

        // 🔹 Fat burns when stamina is used, or slowly if unfed
        if (isMoving)
            fatPct -= fatBurnRate * Time.deltaTime * 3f;
        else
            fatPct -= hungerSpeed * Time.deltaTime * (timeSinceFoodConsumed / 10f);

        fatPct = Mathf.Clamp(fatPct, 0f, 100f);

        // 🔹 Simulate eating (space resets hunger)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timeSinceFoodConsumed = 0f;
            fatPct = Mathf.Min(fatPct + 20f, 100f);
            Debug.Log("🍖 Ate some food.");
        }

        // 🔹 HP drain logic — the "feel" layer
        float energyDeficit = (1f - stamina) + (1f - (fatPct / 100f)); // 0–2 range
        float conditionMultiplier = isTemperatureSafe ? 1f : unsafeTempMultiplier;

        // Exponential decay effect: small when healthy, accelerates when drained
        float hpLoss = Mathf.Pow(energyDeficit, 2.2f) * hpDecayBase * conditionMultiplier;
        hp -= hpLoss * Time.deltaTime * 100f;
        hp = Mathf.Clamp(hp, 0f, 100f);

        Debug.Log($"❤️ HP:{hp:F1}  ⚡Stamina:{stamina:F2}  🧈Fat:{fatPct:F1}  ❄Safe:{isTemperatureSafe}");
    }
}
