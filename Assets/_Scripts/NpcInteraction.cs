using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public Transform player;
    public float interactionDistance = 3f;

    [Header("UI Reference")]
    public GameObject interactionPrompt; // teks "Press E to Talk"
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // cek apakah player di dekat npc dan tekan E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }

        // npc hadap player
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // agar npc tidak bergerak sumbu y
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // interaksi 
    private void StartInteraction()
    {
        Debug.Log("Halo Taher! Saya sedang tidak enak badan, bisa bantu saya?");
        // sistem dialog dibawah
    }

    // terdeteksi player mendekat
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    // terdeteksi player menjauh
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
}
