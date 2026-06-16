using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadSceneBaru(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnApplicationQuit()
    {
        Debug.Log("Aplikasi Keluar");
            Application.Quit();
    }
}
