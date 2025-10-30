using UnityEngine;

public class look_at_cam_ui : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
