using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public ClimbingTimer timer;
    public TextMeshProUGUI timerText;

    public Text myTime;
    public Text bestTime;

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ShowBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestClimbTime", -1f);

        this.bestTime.text = bestTime < 0f ? "0" : bestTime.ToString("F2") + " s";
        myTime.text = timer.CurrentTime.ToString("F2") + " s";
    }
}