using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    // Singleton agar mudah dipanggil skrip lain
    public static QuestManager instance;

    [Header("Quest Settings")]
    public GameObject walletObject; // tarik object dompet
    public GameObject flashdiskObject; // tarik object flashdisk
    public float questDuration = 20.0f; // waktu quest
    
    private float timer;
    private bool isQuestActive = false;
    private int playerHealth = 3; // default awal
    private PlayerHUDManager hud;

    // Variabel checklist penanda barang
    private bool hasWallet = false;
    private bool hasFlashdisk = false;

    // List: Menyimpan daftar ID Quest yang sudah berhasil dimenangkan
    private List<string> completedQuestIDs = new List<string>();
    // Variabel baru untuk mencatat ID quest apa yang sedang dikerjakan Taher saat ini
    private string currentActiveQuestID = "";

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hud = FindFirstObjectByType<PlayerHUDManager>();
        // Sembunyikan item di awal game
        if (walletObject != null) walletObject.SetActive(false);
        if (flashdiskObject != null) flashdiskObject.SetActive(false);
        // Awal game, kosongkan list tugas dan sembunyikan teks timer
        if (hud != null)
        {
            hud.UpdateQuestTracker("-");
            hud.UpdateTimerText("", false); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isQuestActive)
        {
            timer -= Time.deltaTime; // jalankan countdown timer

            // tampilkan timer UI
            if (hud != null)
            {
                hud.UpdateTimerText($"Sisa Waktu: {Mathf.CeilToInt(timer)}s", true);
            }

            // Jika waktu habis (gagal)
            if (timer <= 0)
            {
                FailQuest();
                return;
            }
        }

    }

    // lebih umum, untuk semua quest

    public void StartQuest(string questID, float duration)
    {
        if (isQuestActive) return; 

        // 1. IKAT ID Quest yang sedang aktif saat ini!
        currentActiveQuestID = questID; 

        // 2. Gunakan durasi yang dikirim langsung dari NPC masing-masing
        questDuration = duration; 

        hasWallet = false;
        hasFlashdisk = false;

        // Munculkan kedua barang di map (khusus quest wallet)
        if (questID == "Wallet")
        {
            if (walletObject != null) walletObject.SetActive(true);
            if (flashdiskObject != null) flashdiskObject.SetActive(true);
        }

        isQuestActive = true;
        timer = questDuration;

        UpdateQuestListVisual();
    }
    
    public void RegisterItemCollected(string itemTag)
    {
        if (!isQuestActive) return;

        if (itemTag == "Dompet") hasWallet = true;
        if (itemTag == "Flashdisk") hasFlashdisk = true;

        // perbarui tulisan di task list dinamis
        UpdateQuestListVisual();

        // jika 2 barang ditemukan, panggil completequest
        if (hasWallet && hasFlashdisk)
        {
            CompleteQuest();
        }
    }
    // fungsi merapikan teks list tugas
    public void UpdateQuestListVisual()
    {
        if (hud == null) return;

        string walletStatus = hasWallet ? "[+] Dompet Ditemukan" : "[-] Cari Dompet!";
        string flashdiskStatus = hasFlashdisk ? "[+] Flashdisk Ditemukan" : "[-] Cari Flashdisk!";

        hud.UpdateQuestTracker($"{walletStatus}\n{flashdiskStatus}");
    }

    public void CompleteQuest()
    {
        if (!isQuestActive) return;

        isQuestActive = false;
        
        // ID apa pun yang tadi diikat di awal, langsung dimasukkan ke list pemenang
        completedQuestIDs.Add(currentActiveQuestID); 
        
        Debug.Log($"Quest {currentActiveQuestID} Berhasil!");

        if (hud != null)
        {
            hud.UpdateQuestTracker("-");
            hud.UpdateTimerText("", false);
            hud.TriggerPopupNotification("QUEST BERHASIL!", Color.green);
            hud.ChangeReputation(20f);
        }
    }

    void FailQuest()
    {
        isQuestActive = false;
        Debug.Log("Waktu Habis! Quest Gagal");

        // kurangi nyawa taher
        playerHealth--;

        if (hud != null)
        {
            // Bersih list tugas dan sembunyikan teks timer
            hud.UpdateQuestTracker("-");
            hud.UpdateTimerText("", false);

            // Update nyawa player
            hud.UpdateHearts(playerHealth); 

            // Kirim pesan gagal ke POPUP TENGAH LAYAR dengan warna MERAH MALAM
            hud.TriggerPopupNotification("QUEST GAGAL!\nWaktu Telah Habis", Color.red);
        }

        // Sembunyikan kembali dompetnya karena sudah gagal
        if (walletObject != null) walletObject.SetActive(false);
        if (flashdiskObject != null) flashdiskObject.SetActive(false);
    }

    // fungsi agar skrip NPC bisa mengecek
    public bool CheckIfQuestActive()
    {
        return isQuestActive;
    }

    public bool CheckIfQuestCompleted(string questID)
    {
        // Cek apakah ID quest-nya ada di dalam list yang sudah menang
        return completedQuestIDs.Contains(questID);
    }

    public bool HasWallet()
    {
        return hasWallet;
    }

    public bool HasFlashdisk()
    {
        return hasFlashdisk;
    }
}
