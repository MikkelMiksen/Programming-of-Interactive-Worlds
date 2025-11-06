using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    DefaultUI mainUI;
    [SerializeField]
    InventoryUI inventoryUI;

    bool isDfaultUI;

    void Start()
    {
        mainUI.Show();
        inventoryUI.Hide();
        isDfaultUI = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isDfaultUI)
            {
                DefaultUI.Instance.Show();
                InventoryUI.Instance.Hide();
                isDfaultUI = true;
            }
            else
            {
                DefaultUI.Instance.Hide();
                InventoryUI.Instance.Show();
                isDfaultUI = false;
            }
        }
    }
}
