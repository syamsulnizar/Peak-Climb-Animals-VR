using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public int damage = 1;

    Collider swordCollider;

    void Start()
    {
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false; // default mati
    }

    public void EnableDamage()
    {
        swordCollider.enabled = true;
    }

    public void DisableDamage()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}