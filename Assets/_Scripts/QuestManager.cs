using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    // Singleton agar mudah dipanggil skrip lain
    public static QuestManager instance;

    [Header("Quest Settings")]
    public GameObject walletObject; // tarik object dompet
    public GameObject flashdiskObject; // tarik object flashdisk
    public GameObject balloon1Object; // tarik object balloon1
    public GameObject balloon2Object; // tarik object balloon2
    public GameObject balloon3Object; // tarik object balloon3
    public GameObject balloon4Object; // tarik object balloon4
    public GameObject trashObject; // tarik object trash
    public float questDuration = 20.0f; // waktu quest default
    
    private float timer;
    private bool isQuestActive = false;
    private int playerHealth = 3; // default awal
    private PlayerHUDManager hud;

    // Variabel checklist penanda barang
    private bool hasWallet = false;
    private bool hasFlashdisk = false;
    private bool hasBalloon1 = false;
    private bool hasBalloon2 = false;
    private bool hasBalloon3 = false;
    private bool hasBalloon4 = false;
    private int countTrash = 0;

    // List: Menyimpan daftar ID Quest yang sudah berhasil dimenangkan
    private List<string> completedQuestIDs = new List<string>();
    // Variabel baru untuk mencatat ID quest apa yang sedang dikerjakan Taher saat ini
    private string currentActiveQuestID = "";

    [Header("Game Over Settings")]
    public GameObject gameOverPanel; // tarik gameoverpanel

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
        if (balloon1Object != null) balloon1Object.SetActive(false);
        if (balloon2Object != null) balloon2Object.SetActive(false);
        if (balloon3Object != null) balloon3Object.SetActive(false);
        if (balloon4Object != null) balloon4Object.SetActive(false);
        if (trashObject != null) trashObject.SetActive(false);



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
        hasBalloon1 = false;
        hasBalloon2 = false;
        hasBalloon3 = false;
        hasBalloon4 = false;
        countTrash = 0;

        // Munculkan kedua barang di map (khusus quest wallet)
        if (questID == "Wallet")
        {
            if (walletObject != null) walletObject.SetActive(true);
            if (flashdiskObject != null) flashdiskObject.SetActive(true);
        }

        // Munculkan kedua barang di map (khusus quest balloon)
        if (questID == "Balloon")
        {
            if (balloon1Object != null) balloon1Object.SetActive(true);
            if (balloon2Object != null) balloon2Object.SetActive(true);
            if (balloon3Object != null) balloon3Object.SetActive(true);
            if (balloon4Object != null) balloon4Object.SetActive(true);
        }

         // Munculkan trash di map (khusus quest trash)       
        if (questID == "Trash")
        {
            if (trashObject != null) trashObject.SetActive(true);
        }

        isQuestActive = true;
        timer = questDuration;

        UpdateQuestListVisual(questID);
    }
    
    public void RegisterItemCollected(string itemTag)
    {
        if (!isQuestActive) return;

        if (itemTag == "Dompet") hasWallet = true;
        if (itemTag == "Flashdisk") hasFlashdisk = true;
        if (itemTag == "Balloon1") hasBalloon1 = true;
        if (itemTag == "Balloon2") hasBalloon2 = true;
        if (itemTag == "Balloon3") hasBalloon3 = true;
        if (itemTag == "Balloon4") hasBalloon4 = true;
        if (itemTag == "Trash") countTrash += 1;

        // perbarui tulisan di task list dinamis
        UpdateQuestListVisual(currentActiveQuestID);

        // jika 2 barang ditemukan, panggil completequest
        if (hasWallet && hasFlashdisk)
        {
            CompleteQuest();
        }

        // jika 4 balon barang ditemukan, panggil completequest
        if (hasBalloon1 && hasBalloon2 && hasBalloon3 && hasBalloon4)
        {
            CompleteQuest();
        }

        // jika 14 barang ditemukan, panggil completequest
        if (countTrash == 14)
        {
            CompleteQuest();
        }
    }
    // fungsi merapikan teks list tugas
    public void UpdateQuestListVisual(string questID)
    {
        if (hud == null) return;

        if (questID == "Wallet")
        {
            string walletStatus = hasWallet ? "[+] Dompet Ditemukan" : "[-] Cari Dompet!";
            string flashdiskStatus = hasFlashdisk ? "[+] Flashdisk Ditemukan" : "[-] Cari Flashdisk!";
            hud.UpdateQuestTracker($"{walletStatus}\n{flashdiskStatus}");
        }

        if (questID == "Balloon")
        {
            string balloon1Status = hasBalloon1 ? "[+] Balon Bulat Ditemukan" : "[-] Ambil Balon Bulat!";
            string balloon2Status = hasBalloon2 ? "[+] Balon pudel Ditemukan" : "[-] Ambil Balon pudel!";
            string balloon3Status = hasBalloon3 ? "[+] Balon Bintang Ditemukan" : "[-] Ambil Balon Bintang!";
            string balloon4Status = hasBalloon4 ? "[+] Balon Hati Ditemukan" : "[-] Ambil Balon Hati!";
            hud.UpdateQuestTracker($"{balloon1Status}\n{balloon2Status}\n{balloon3Status}\n{balloon4Status}");
        }

        if (questID == "Trash")
        {
            string trashStatus = countTrash == 14 ? "[+] Berhasil mengumpulkan semua sampah" : "[-] Kumpulkan semua sampah! (" + countTrash + "/14)";
            hud.UpdateQuestTracker($"{trashStatus}");
        }

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

        // deteksi nyawa 0 (lose condition)
        if (playerHealth <= 0)
        {
            TriggerGameOver();
            return;
        }

        // Sembunyikan kembali dompetnya karena sudah gagal
        if (walletObject != null) walletObject.SetActive(false);
        if (flashdiskObject != null) flashdiskObject.SetActive(false);
        if (balloon1Object != null) balloon1Object.SetActive(false);
        if (balloon2Object != null) balloon2Object.SetActive(false);
        if (balloon3Object != null) balloon3Object.SetActive(false);
        if (balloon4Object != null) balloon4Object.SetActive(false);
        if (trashObject != null) trashObject.SetActive(false);
    }

    void TriggerGameOver()
    {
        // munculkan panel kalah
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // hentikan total waktu dalam game agar player/npc berhenti
        Time.timeScale = 0f;
        // munculkan kursor mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Fungsi baru untuk mengecek nyawa Taher 0
    public bool CheckIfPlayerDead()
    {
        // Mengembalikan nilai true jika playerHealth kurang dari atau sama dengan 0
        return playerHealth <= 0;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f; // kembalikan kecepatan waktu game ke normal
        // reload ulang Main scene dari awal secara bersih
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // kembalikan kecepatan waktu game ken normal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // load scene MainMenu
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
}