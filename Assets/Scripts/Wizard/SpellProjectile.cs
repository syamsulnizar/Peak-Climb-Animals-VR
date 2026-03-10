using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 1;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb.velocity = transform.forward * speed;
        Invoke("DisableProjectile", lifeTime);
    }

    void DisableProjectile()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            DisableProjectile();

        }

    }
}