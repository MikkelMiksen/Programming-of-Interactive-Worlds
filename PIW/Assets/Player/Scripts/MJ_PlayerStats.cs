using TMPro;
using UnityEngine;

public class MJ_PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private float hp = 100f, maxHP = 100f;

    [SerializeField]
    private TextMeshProUGUI hpText, bodyTemp;

    private Color[] colors = new Color[3] { Color.cyan, Color.grey, Color.red };


    private string hpDispText => "HP " + (int)hp + " / " + (int)maxHP;
    private string bodyDispText => MJ_BodyTemp.Instance.readBodyTemp;

    void Update()
    {
        hpText.text = hpDispText;

        bodyTemp.text = bodyDispText;

        if (MJ_BodyTemp.Instance.isBodyTempFreezing)
        {
            bodyTemp.color = colors[0];
        } else if (MJ_BodyTemp.Instance.isBodyTempSafe)
        {
            bodyTemp.color = colors[1];
        } else if (MJ_BodyTemp.Instance.isBodyTempOverheating)
        {
            bodyTemp.color = colors[2];
        }
        else
        {
            bodyTemp.color = colors[0];
        }




        if (hp <= 0)
        {
            hpText.text = "player has died";
        }
    }
}
