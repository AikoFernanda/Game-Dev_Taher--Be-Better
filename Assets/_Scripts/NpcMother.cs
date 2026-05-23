using UnityEngine;
using TMPro;
using UnityEngine.UI; // Wajib ada untuk membaca Bar UI

public class NpcMother : MonoBehaviour
{
    private DialogManager dm;
    private bool isPlayerInRange = false;

    [Header("Connector to UI Canvas")]
    public GameObject latterPanel; // tarik later panel
    public Slider reputationBar; // tarik UI bar reputasi

    [Header("Player Setting")]
    public Transform player; // posisi taher
    public float jarakInteraksi = 3f; // jarak minimal

    [Header("UI Reference")]
    public GameObject interactionPrompt; // teks "Press E to Talk"
    public GameObject motherDialogPanel; // Tarik PanelDialog
    public TextMeshProUGUI motherDialogText;         // PENTING: Tarik TeksDialogIbu ke sini

    [Header("Audio SFX")]
    public AudioClip sfxMenang;    // Tarik MP3 suara Good Ending
    public AudioClip sfxMenang2;    // Tarik MP3 suara Good Ending
    public AudioClip sfxGagal;     // Tarik MP3 suara Bad Ending
    private AudioSource audioMesin; // Variabel penampung mesin pemutar

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dm = FindFirstObjectByType<DialogManager>();  // cari script
        // Mengambil mesin AudioSource yang menempel pada objek NPC Ibu
        audioMesin = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // hitung jarak taher dn ibu
        float jarak = Vector3.Distance(transform.position, player.position);
        // Cek apakah DialogManager ada. Jika tidak ada, buat kondisi default agar tidak crash
        bool waitingForChoice = (dm != null) ? dm.isWaitingForChoice : false;
        // validasi jarak dan keycode
        if (isPlayerInRange && jarak <= jarakInteraksi && Input.GetKeyDown(KeyCode.E) && !waitingForChoice)
        {
            // JIKA QUEST LAGI JALAN -> CUEKIN PLAYER
            if (QuestManager.instance != null && QuestManager.instance.CheckIfQuestActive())
            {
                LookAtPlayer();
                ShowMotherSpeech("Ibu: Fokus berguna bagi sekitar Taher...");
                return;
            }

            // NPC hadap player (Memanggil fungsi yang di bawah)
            LookAtPlayer();

            if (reputationBar.value >= reputationBar.maxValue && QuestManager.instance.IsReadyForEnding())
            {
                // Sembunyikan dialog biasa jika surat menang terbuka
                if (motherDialogPanel != null) motherDialogPanel.SetActive(false);
                // SFX MENANG (Cek agar tidak spam bunyi jika dipencet E berkali-kali)
                if (audioMesin != null && sfxMenang != null && sfxMenang2 != null &&!latterPanel.activeSelf)
                {
                    audioMesin.PlayOneShot(sfxMenang2);
                    audioMesin.PlayOneShot(sfxMenang);
                }
                // berhasil, munculkan surat ibu
                latterPanel.SetActive(true);
                // Sembunyikan prompt "Press E" karena surat sudah terbuka
                if (interactionPrompt != null) interactionPrompt.SetActive(false);
                // Hentikan waktu game
                Time.timeScale = 0f;

                Cursor.lockState = CursorLockMode.None; // Lepas kunci kursor agar bebas bergerak
                Cursor.visible = true;
            }
            else if (QuestManager.instance.IsReadyForEnding() && reputationBar.value <= reputationBar.maxValue)
            {
                // jika quest seelesai tapi reputasi tidak penuh adalah bad ending muncul panel gameover
                if (motherDialogPanel != null) motherDialogPanel.SetActive(false);
                if (interactionPrompt != null) interactionPrompt.SetActive(false);

                // Kita picu fungsi khusus Bad Ending di bawah atau panggil panel GameOver
                TriggerBadEnding();
            }
            else
            {
                // jika belum 100%
                ShowMotherSpeech("Ibu:\nJadilah pemuda yang berguna bagi desa ini ya, Nak... hmm, kamu dengar itu? Sepertinya ada seseorang yang sedang menangis. Suaranya berasal dari arah timur.");
            }

        }
    }

    // Fungsi pembantu untuk menyalakan teks dialog Ibu
    void ShowMotherSpeech(string kalimat)
    {
        if (motherDialogPanel != null && motherDialogText != null)
        {
            motherDialogText.text = kalimat; // Ganti isi teksnya secara dinamis
            motherDialogPanel.SetActive(true); // Munculkan panel kotaknya di layar
            if (interactionPrompt != null) interactionPrompt.SetActive(false); // Sembunyikan tulisan "Press E"
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // agar npc tidak bergerak sumbu y
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // terdeteksi player mendekat
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Jika quest global lagi jalan, sembunyikan prompt "Press E"
            if (QuestManager.instance != null && QuestManager.instance.CheckIfQuestActive())
            {
                if (interactionPrompt != null) interactionPrompt.SetActive(false);
                return;
            }

            // Nyalakan prompt Press E hanya jika panel dialog Ibu sedang tidak terbuka
            if (interactionPrompt != null && motherDialogPanel != null && !motherDialogPanel.activeSelf)
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

            if (dm != null)
            {
                dm.EndDialog();
            }
            if (interactionPrompt != null && motherDialogPanel != null)
            {
                interactionPrompt.SetActive(false);
                motherDialogPanel.SetActive(false);

            }
        }
    }

    void TriggerBadEnding()
    {
        Debug.Log("TAHER BAD ENDING: Tugas selesai tapi reputasi buruk!");

        // FX Gagal / Bad Ending
        if (audioMesin != null && sfxGagal != null)
        {
            audioMesin.PlayOneShot(sfxGagal);
        }

        // Panel GameOver ada di QuestManager
        if (QuestManager.instance != null && QuestManager.instance.gameOverPanel != null)
        {
            // cari teks di dalam panel gameover untuk diganti isinya secara dinamis
            TextMeshProUGUI gameOverText = QuestManager.instance.gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (gameOverText != null)
            {
                gameOverText.text = "BAD ENDING\nKamu menyelesaikan tugas, tapi hatimu belum tulus membantu warga.";
            }

            QuestManager.instance.gameOverPanel.SetActive(true);
        }

        // Bekukan game seperti biasa
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
