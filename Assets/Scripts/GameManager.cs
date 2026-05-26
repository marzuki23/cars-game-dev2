using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public TimerManager timerManager;

    [Header("Crash Settings")]
    public int maxCrash = 3;

    private int crashCount = 0;
    private int starsEarned = 0;
    private bool isMissionComplete = false;
    private bool isMissionFailed = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (timerManager == null)
        {
            timerManager = GetComponent<TimerManager>();
        }
    }

    private void Update()
    {
        if (isMissionComplete || isMissionFailed) return;

        // gagal jika waktu habis
        if (timerManager != null && timerManager.timeRemaining <= 0f)
        {
            starsEarned = 0;
            OnMissionFailed();
        }
    }

    // dipanggil saat mobil parkir
    public void OnMissionComplete()
    {
        if (isMissionComplete || isMissionFailed) return;

        starsEarned = CalculateStars();
        isMissionComplete = true;
        Time.timeScale = 0f;
    }

    // dipanggil saat tabrakan
    public void AddCrash()
    {
        if (isMissionComplete || isMissionFailed) return;

        crashCount++;
        Debug.Log("Tabrakan: " + crashCount);

        if (crashCount >= maxCrash)
        {
            starsEarned = 0;
            OnMissionFailed();
        }
    }

    public void OnMissionFailed()
    {
        if (isMissionComplete || isMissionFailed) return;

        isMissionFailed = true;
        Time.timeScale = 0f;
    }

    private int CalculateStars()
    {
        if (timerManager == null) return 0;

        float timeLeft = timerManager.timeRemaining;

        // bintang dari waktu
        int timeStars;
        if (timeLeft >= 10f)
            timeStars = 3;
        else if (timeLeft >= 5f)
            timeStars = 2;
        else if (timeLeft > 0f)
            timeStars = 1;
        else
            timeStars = 0;

        // bintang dari tabrakan
        int crashStars;
        if (crashCount == 0)
            crashStars = 3;
        else if (crashCount == 1)
            crashStars = 2;
        else if (crashCount == 2)
            crashStars = 1;
        else
            crashStars = 0;

        // ambil nilai terendah
        return Mathf.Min(timeStars, crashStars);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGUI()
    {
        if (isMissionComplete)
            DrawResult("MISI BERHASIL!", Color.green);

        if (isMissionFailed)
            DrawResult("MISI GAGAL!", Color.red);
    }

    private void DrawResult(string title, Color color)
    {
        float boxW = 900;
        float boxH = 500;
        float x = (Screen.width - boxW) / 2f;
        float y = (Screen.height - boxH) / 2f;

        GUI.Box(new Rect(x, y, boxW, boxH), "");

        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 50;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = color;

        GUI.Label(new Rect(x, y + 30, boxW, 60), title, titleStyle);

        DrawStars(x, y + 150, boxW);

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 28;
        infoStyle.alignment = TextAnchor.MiddleCenter;
        infoStyle.normal.textColor = Color.white;

        string reason = "";

        if (isMissionFailed)
        {
            if (timerManager != null && timerManager.timeRemaining <= 0f)
                reason = "Waktu Habis!";
            else if (crashCount >= maxCrash)
                reason = "Tabrakan 3x!";
        }

        GUI.Label(new Rect(x, y + 290, boxW, 50), reason, infoStyle);

        GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.fontSize = 28;

        if (GUI.Button(new Rect(x + 300, y + 360, 300, 70), "ULANGI", btnStyle))
        {
            Retry();
        }
    }

    private void DrawStars(float panelX, float panelY, float panelW)
    {
        GUIStyle starStyle = new GUIStyle(GUI.skin.label);
        starStyle.fontSize = 100;
        starStyle.alignment = TextAnchor.MiddleCenter;
        starStyle.fontStyle = FontStyle.Bold;

        float starSize = 120;
        float spacing = 50;
        float totalW = starSize * 3 + spacing * 2;
        float startX = panelX + (panelW - totalW) / 2f;

        for (int i = 0; i < 3; i++)
        {
            Rect r = new Rect(startX + i * (starSize + spacing), panelY, starSize, starSize);

            bool earned = i < starsEarned;

            starStyle.normal.textColor = earned
                ? new Color(1f, 0.85f, 0f)
                : new Color(0.3f, 0.3f, 0.3f);

            GUI.Label(r, earned ? "★" : "☆", starStyle);
        }
    }
}