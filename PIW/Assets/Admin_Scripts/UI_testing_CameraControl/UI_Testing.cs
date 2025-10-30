using System;
using UnityEngine;

public class UI_Testing : MonoBehaviour
{
    [SerializeField] GameObject lookAt_object,player,ui1,ui2;

    public enum lookAt_enum
    {
        player,
        ui_panel1,
        ui_panel2,
    }

    public lookAt_enum ui_object;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ui_object = lookAt_enum.player;
    }

    // Update is called once per frame
    void Update()
    {

        switch (ui_object)
        {
            case lookAt_enum.player:
                lookAt_object = player;
                break;
            case lookAt_enum.ui_panel1:
                lookAt_object = ui1;
                break;
            case lookAt_enum.ui_panel2:
                lookAt_object = ui2;
                break;
            default:
                break;
        }

        transform.LookAt(lookAt_object.transform.position);
    }
}


