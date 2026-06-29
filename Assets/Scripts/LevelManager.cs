using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Panel Cara Bermain")]
    [SerializeField] private GameObject howToPlayPanel;

    private string selectedScene = "";

    private void Start()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    // ==========================
    // PILIH LEVEL
    // ==========================

    public void SelectLevel1()
    {
        selectedScene = "level 1";   // Sesuaikan dengan nama scene
        ShowPanel();
    }

    public void SelectLevel2()
    {
        selectedScene = "level 2";
        ShowPanel();
    }

    public void SelectLevel3()
    {
        selectedScene = "level 3";
        ShowPanel();
    }

    // ==========================
    // PANEL CARA BERMAIN
    // ==========================

    private void ShowPanel()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("HowToPlayPanel belum dihubungkan di Inspector!");
        }
    }

    public void ClosePanel()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    // ==========================
    // MULAI GAME
    // ==========================

    public void StartGame()
    {
        if (string.IsNullOrEmpty(selectedScene))
        {
            Debug.LogError("Silakan pilih level terlebih dahulu!");
            return;
        }

        // Pastikan game berjalan normal
        Time.timeScale = 1f;

        // Hentikan musik menu
        GameObject music = GameObject.Find("MenuMusic");
        if (music != null)
        {
            Destroy(music);
        }

        // Masuk ke scene yang dipilih
        SceneManager.LoadScene(selectedScene);
    }
}