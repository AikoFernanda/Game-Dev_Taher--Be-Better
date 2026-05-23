using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    public float wanderRadius = 10f; // Jarak maksimal NPC berjalan
    public float wanderTimer = 3f;   // Waktu diam sebelum mencari titik baru

    private NavMeshAgent agent;
    private float timer;

    private Animator animator;
    private bool isWalking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();

        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Logika mencari titik baru
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0; // Reset timer
        }

        // --- LOGIKA ANIMASI ---
        if (animator != null)
        {
            bool currentlyMoving = agent.velocity.magnitude > 0.1f;

            if (currentlyMoving && !isWalking)
            {
                animator.SetTrigger("walk");

                isWalking = true;
            }

            else if (!currentlyMoving && isWalking)
            {
                animator.SetTrigger("idle");

                isWalking = false;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}