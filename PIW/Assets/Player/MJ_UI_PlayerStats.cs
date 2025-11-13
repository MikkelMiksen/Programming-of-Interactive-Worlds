using TMPro;
using UnityEngine;

public class MJ_UI_PlayerStats : MonoBehaviour
{

    [Header("Body Energy / Fat")]
    [SerializeField] TextMeshProUGUI fatPct;
    [SerializeField] TextMeshProUGUI fatLossPerHour;
    [SerializeField] TextMeshProUGUI fatGainPerMeal;

    [Header("Clothing Insulation (0–1 each)")]
    [SerializeField] TextMeshProUGUI jacketInsulation;
    [SerializeField] TextMeshProUGUI pantsInsulation;
    [SerializeField] TextMeshProUGUI bootsInsulation;
    [SerializeField] TextMeshProUGUI hatInsulation;

    void Update()
    {
        UpdateBodyFat();
        UpdateClothingInsulation();
    }



    void UpdateBodyFat()
    {
        fatPct.text = "Fat %: " + MJ_PlayerStats01.Instance.fatPct.ToString();
        fatLossPerHour.text = "Fat Loss Per Hour: " + MJ_PlayerStats01.Instance.hp.ToString();
        fatGainPerMeal.text = "Fat Gain Per Meal: " + MJ_PlayerStats01.Instance.hp.ToString();
    }

    void UpdateClothingInsulation()
    {
        if (MJ_PlayerStats01.Instance.jacketInsulation != 0 ||
        MJ_PlayerStats01.Instance.pantsInsulation != 0 ||
        MJ_PlayerStats01.Instance.bootsInsulation != 0 ||
        MJ_PlayerStats01.Instance.hatInsulation != 0)
        {
            jacketInsulation.text = "Jacket Insulation: " + MJ_PlayerStats01.Instance.jacketInsulation.ToString();
            pantsInsulation.text = "Pants Insulation: " + MJ_PlayerStats01.Instance.pantsInsulation.ToString();
            bootsInsulation.text = "Boots Insulation: " + MJ_PlayerStats01.Instance.bootsInsulation.ToString();
            jacketInsulation.text =  "Hat Insulation: " + MJ_PlayerStats01.Instance.hatInsulation.ToString();
        }
        else
        {
            jacketInsulation.text = "Jacket Insulation:  0";
            pantsInsulation.text = "Pants Insulation:  0";
            bootsInsulation.text = "Boots Insulation:  0";
            hatInsulation.text = "Hat Insulation:  0";
        }
    }
}
