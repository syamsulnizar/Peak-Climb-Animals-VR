using UnityEngine;
using UnityEngine.Events;

public class GroupEnemy : MonoBehaviour
{
    [SerializeField] private EnemyAI[] enemies;
    [SerializeField] private UnityEvent onAllEnemiesDied;

    public void CheckAllDead()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.isDead)
            {
                return;
            }
        }

        AllEnemiesDead();
    }

    void AllEnemiesDead()
    {
        onAllEnemiesDied.Invoke();
    }

    public void AllEnemiesReady()
    {
        foreach (var enemy in enemies)
        {
            enemy.playerDetected = true;
        }
    }

    public void WakeEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.enabled = true;
        }
    }
}