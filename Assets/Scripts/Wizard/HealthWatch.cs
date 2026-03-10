using TMPro;
using UnityEngine;

public class HealthWatch : MonoBehaviour
{
    public TextMeshProUGUI healthTextWatch;

    private void Start()
    {
        PlayerHealth.Instance.OnHealthUpdate += UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthTextWatch.text = PlayerHealth.Instance.currentHealth.ToString();
    }
}
