using UnityEngine;
using UnityEngine.SceneManagement; // untuk kembali ke scene Menu

public class PauseManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pausePanel; // tarik pausepanel
    private bool isPaused = false;

    public GameObject latterPanel;

    [Header("Audio SFX")]
    public AudioClip sfxPause;      // tarik MP3 suara klik pause
    public AudioClip sfxClick;    //tarik MP3 suara klik pause
    
    private AudioSource audioMesin; // Variabel penampung mesin otomatis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Mengambil komponen AudioSource yang nempel di objek
        audioMesin = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // jika nyawa taher 0, blokir tombol esc
        if (QuestManager.instance != null && QuestManager.instance.CheckIfPlayerDead())
        {
            return; // tombol esc jadi tidak berfungsi
        }

        // deteksi jika pemain meknekan escape (Esc) DAN panel surat (winning conditon) tidak sedang aktif
        if (Input.GetKeyDown(KeyCode.Escape) && (latterPanel == null || !latterPanel.activeSelf))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // fungsi menghentikan game
    public void PauseGame()
    {
        isPaused = true;

        // SFX PAUSE: Diputar TEPAT sebelum waktu game dibekukan (0f)
        if (audioMesin != null && sfxPause != null)
        {
            audioMesin.PlayOneShot(sfxPause);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(true); // muncul panel pause
        }

        Time.timeScale = 0f; // hentikan total waktu dalam game

        // munculkan kursor mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Fungsi untuk lanjut game
    public void ResumeGame()
    {
        isPaused = false;

        // SFX klik resume sebelum panel hilang
        if (audioMesin != null && sfxClick != null)
        {
            audioMesin.PlayOneShot(sfxClick);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // sembuyikan panel pause
        }

        Time.timeScale = 1f; // kembalikan kecepatan waktu ke normal

        // paksa kursor untuk 'None' dulu, lalu paksa kunci total
        Cursor.lockState = CursorLockMode.None;

        // Sembunyikan kembali kursor mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Fungsi kembali ke main
    public void GoToMainMenu()
    {
        // SFX klik sebelum panel hilang
        if (audioMesin != null && sfxClick != null)
        {
            audioMesin.PlayOneShot(sfxClick);
        }
        Time.timeScale = 1f; // normalkan waktu game sebelum pindah scene
        SceneManager.LoadScene("MainMenu"); // load scenen MainMenu
    }
}
