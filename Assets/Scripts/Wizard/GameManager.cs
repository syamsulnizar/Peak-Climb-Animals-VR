using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;

    float startTime;
    bool gameFinished = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startTime = Time.time;
    }

    public void AddScore(int amount)
    {
        score += amount;

        CheckAllEnemiesDead();
    }

    void CheckAllEnemiesDead()
    {
        if (FindObjectsOfType<EnemyAI>().Length == 0)
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        if (gameFinished) return;

        gameFinished = true;

        float yourTime = Time.time - startTime;

        float bestTime = PlayerPrefs.GetFloat("BestTime", 9999f);

        if (yourTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", yourTime);
        }

        Debug.Log("Your Time: " + yourTime);
        Debug.Log("Best Time: " + PlayerPrefs.GetFloat("BestTime"));
    }
}