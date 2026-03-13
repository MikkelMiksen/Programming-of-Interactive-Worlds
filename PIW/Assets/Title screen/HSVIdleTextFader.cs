using TMPro;
using UnityEngine;

public class HSVIdleTextFader : MonoBehaviour
{
    private const float FadeSpeed = 0.7f;
    private TextMeshProUGUI myTextMesh;
    [SerializeField] private Color[] color;
    private float t = 0.6f;
    
    void Start()
    {
        myTextMesh = GetComponent<TextMeshProUGUI>();
        myTextMesh.color = color[0];
    }
    
    void Update()
    {
        t += FadeSpeed * Time.deltaTime;
        myTextMesh.color = Color.Lerp(color[0], color[1],Mathf.PingPong(t,0.6f));
    }
}
