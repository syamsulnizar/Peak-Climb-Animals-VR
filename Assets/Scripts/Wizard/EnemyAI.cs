using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float detectionRange = 10f;
    public float attackRange = 2f;

    public int maxHealth = 3;

    int currentHealth;

    public bool playerDetected = false;
    public bool isDead = false;
    bool isHit = false;

    Animator anim;
    NavMeshAgent agent;

    float attackCooldown = 1.5f;
    float nextAttackTime = 0f;

    float hitDuration = 0.5f;

    public UnityEvent onStartAttack;
    public UnityEvent onEndAttack;
    public UnityEvent onDeath;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead || isHit) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!playerDetected && distance <= detectionRange)
        {
            playerDetected = true;
        }

        if (!playerDetected)
        {
            anim.Play("Idle");
            agent.isStopped = true;
            return;
        }

        if (distance > attackRange)
        {
            WalkToPlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    public void StartAttack()
    {
        onStartAttack?.Invoke();
    }

    public void EndAttack()
    {
        onEndAttack?.Invoke();
    }

    void WalkToPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        anim.Play("Walk");
    }

    void AttackPlayer()
    {
        agent.isStopped = true;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            anim.Play("Attack");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (!playerDetected)
        {
            playerDetected = true;
            StartCoroutine(HitRoutine());
            anim.SetTrigger("Hit");
            return;
        }

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            StartCoroutine(HitRoutine());
            anim.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    System.Collections.IEnumerator HitRoutine()
    {
        isHit = true;

        agent.isStopped = true;

        anim.Play("Get Hit");

        yield return new WaitForSeconds(hitDuration);

        isHit = false;
    }

    void Die()
    {
        isDead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        agent.isStopped = true;

        anim.Play("Died");
        onDeath?.Invoke();
        Invoke(nameof(HideEnemy), 3f);
    }

    public void HideEnemy()
    {
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}