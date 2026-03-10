using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;

    public int currentHealth;

    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public Action OnHealthUpdate;

    public static PlayerHealth Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString();

        OnHealthUpdate?.Invoke();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        healthText.text = currentHealth.ToString();
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }

        OnHealthUpdate?.Invoke();
    }

    public void RestoreHP()
    {
        currentHealth = maxHealth;

        healthText.text = currentHealth.ToString();
        healthSlider.value = currentHealth;
        OnHealthUpdate?.Invoke();
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}