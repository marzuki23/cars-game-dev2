using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float timeRemaining = 60f;
    public Text timerText;
    public bool timerRunning = true;

    public GameManager gameManager;

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;

                if (gameManager != null)
                {
                    gameManager.OnMissionFailed();
                }
            }

            timerText.text = "Waktu: " + Mathf.Ceil(timeRemaining).ToString();
        }
    }
}