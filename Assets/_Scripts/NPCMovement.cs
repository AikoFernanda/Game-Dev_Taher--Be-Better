using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float wanderRadius = 10f; // Jarak maksimal NPC berjalan untuk mencari titik baru
    public float wanderTimer = 3f;   // Berapa lama NPC diam sebelum berjalan lagi ke titik lain

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Jika waktu timer sudah habis, cari titik tujuan acak yang baru
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0; // Reset timer
        }
    }

    // Fungsi matematika untuk mencari titik acak secara akurat di atas area NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        // Memastikan titik acaknya benar-benar berada di atas jaring biru (NavMesh)
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}