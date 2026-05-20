using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [TextArea(3,10)] // biar kotak teks di inspektor luas
    public string[] dialogSentences;
    private DialogManager dm;
    private bool isPlayerInRange = false;
    private bool isTalking = false;
    public Transform player;
    public float interactionDistance = 3f;
    private PlayerHUDManager hud; // menampung HUD

    [Header("Custom Choices Text")]
    public string teksPositif = "Tentu, saya bantu!";
    public string teksNegatif = "Gak peduli, cari sendiri!";
    public string teksNetral  = "Maaf, saya sedang lelah.";

    [Header("UI Reference")]
    public GameObject interactionPrompt; // teks "Press E to Talk"

    [Header("Quest Identity")]
    public string npcQuestID; // Tulis ID Unik di Inspector (Contoh: "Wallet", "Trash", atau "Kite")
    public float questDuration = 20f; // Nilai default 20 detik, bisa ubah sesuaikan tiap npc diInspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dm = FindFirstObjectByType<DialogManager>();  // cari script
        hud = FindFirstObjectByType<PlayerHUDManager>(); // cari script
    }

    // Update is called once per frame
    void Update()
    {
        // cek apakah player di dekat npc dan tekan E dan menunggu pilihan (agar klik E tidak skip saat tombol pilihan muncul)
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !dm.isWaitingForChoice)
        {
            // JIKA QUEST LAGI JALAN -> CUEKIN PLAYER
            if (QuestManager.instance != null && QuestManager.instance.CheckIfQuestActive())
            {
                Debug.Log("NPC: Taher lagi nyari barang, jangan diganggu dulu.");
                return;
            }
            // Cek apakah ID Quest milik NPC ini sudah pernah dimenangkan?
            if (QuestManager.instance != null && QuestManager.instance.CheckIfQuestCompleted(npcQuestID))
            {
                Debug.Log($"NPC: Quest {npcQuestID} sudah selesai. NPC ini tidak bisa diinteraksi lagi.");
                return; 
            }
            // npc hadap player
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < interactionDistance && Input.GetKeyDown(KeyCode.E))
            {
                LookAtPlayer();
            }

            if (!isTalking)
            {
                dm.StartDialog(dialogSentences, this); // kirim this (identitas npc ini) ke manager
                isTalking = true;
            }
            else
            {
                dm.DisplayNextSentence();
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // agar npc tidak bergerak sumbu y
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // terdeteksi player menjauh
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isTalking = false;
            if (dm != null) 
            {
                dm.EndDialog();
            }
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }

        // terdeteksi player menjauh
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Jika quest lagi jalan 
            if (QuestManager.instance != null)
            {
                // Sembunyikan "Press E" untuk SEMUA NPC kalau lagi ada quest jalan
                if (QuestManager.instance.CheckIfQuestActive())
                {
                    return; 
                }
                // 2. Sembunyikan "Press E" HANYA untuk NPC yang quest-nya sudah beres
                if (QuestManager.instance.CheckIfQuestCompleted(npcQuestID)) return;
            }
            isPlayerInRange = true;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    // Respon dari pilihan taher
    public void TriggerPositiveResponse()
    {
        Debug.Log("Taher memilih positif");
        if (hud != null)
        {
            hud.ChangeReputation(5f); // +5 reputasi
        }

        // KIRIMKAN ID DAN DURASI NPC INI KE QUEST MANAGER
        if (QuestManager.instance != null)
        {
            // NPC akan mengirim IDquest-nya dan durasinya
            QuestManager.instance.StartQuest(npcQuestID, questDuration); 
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        dm.EndDialog();
        isTalking = false;
    }

    public void TriggerNegativeResponse()
    {
        Debug.Log("Taher memilih negatif");
        // TODO: Mentriger pengurangan reputasi bar 10% dari max bar reputasi
        if (hud != null)
        {
            hud.ChangeReputation(-10f); // JIKA MERESPONS NEGATIF, REPUTASI BERKURANG 10
        }
        dm.EndDialog();
        isTalking = false;
    }

    public void TriggerNeutralResponse()
    {
        Debug.Log("Taher memilih netral");
        // NETRAL TIDAK MEMENGARUHI REPUTASI
        dm.EndDialog();
        isTalking = false;
    }
}
