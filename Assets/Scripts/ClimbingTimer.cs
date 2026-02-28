using UnityEngine;
using UnityEngine.Events;

public class ClimbingTimer : MonoBehaviour
{
    [Header("Timer Events")]
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerStop;
    public UnityEvent OnTimerFinish;

    [Header("Score Events")]
    public UnityEvent OnNewBestTime;
    public UnityEvent OnNotBestTime;

    private float timer;
    private bool isRunning;

    private const string LAST_TIME_KEY = "LastClimbTime";
    private const string BEST_TIME_KEY = "BestClimbTime";

    public float CurrentTime => timer;

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
        }
    }

    // =========================
    // TIMER CONTROL
    // =========================

    public void StartTimer()
    {
        timer = 0f;
        isRunning = true;
        OnTimerStart?.Invoke();
    }

    public void StopTimer()
    {
        isRunning = false;
        SaveLastTime(timer);
        OnTimerStop?.Invoke();
    }

    public void FinishTimer()
    {
        isRunning = false;

        float yourTime = timer;
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, -1f);

        SaveLastTime(yourTime);

        // Jika belum ada best time sebelumnya
        if (bestTime < 0f)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, yourTime);
            PlayerPrefs.Save();
            OnNewBestTime?.Invoke();
        }
        else
        {
            if (yourTime <= bestTime)
            {
                PlayerPrefs.SetFloat(BEST_TIME_KEY, yourTime);
                PlayerPrefs.Save();
                OnNewBestTime?.Invoke();
            }
            else
            {
                OnNotBestTime?.Invoke();
            }
        }

        OnTimerFinish?.Invoke();
    }

    // =========================
    // DATA ACCESS
    // =========================

    void SaveLastTime(float time)
    {
        PlayerPrefs.SetFloat(LAST_TIME_KEY, time);
        PlayerPrefs.Save();
    }

    public float GetLastTime()
    {
        return PlayerPrefs.GetFloat(LAST_TIME_KEY, 0f);
    }

    public float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BEST_TIME_KEY, -1f);
    }
}