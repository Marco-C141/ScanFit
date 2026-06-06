using UnityEngine;
using UnityEngine.SceneManagement; // Required for managing scenes

public class SceneChanger : MonoBehaviour
{
    // This function must be public so the button can see it
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}