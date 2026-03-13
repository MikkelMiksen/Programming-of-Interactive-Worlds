using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeToSceneIndex(int index) => Application.LoadLevel(index); 
}
