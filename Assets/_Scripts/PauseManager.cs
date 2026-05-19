using UnityEngine;
using UnityEngine.SceneManagement; // untuk kembali ke scene Menu

public class PauseManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pausePanel; // tarik pausepanel
    private bool isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // jika nyawa taher 0, blokir tombol esc
        if (QuestManager.instance != null && QuestManager.instance.CheckIfPlayerDead())
        {
            return; // tombol esc jadi tidak berfungsi
        }

        // deteksi jika pemain mekena escape (Esc)
        if (Input.GetKeyDown(KeyCode.Escape))
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
        Time.timeScale = 1f; // normalkan waktu game sebelum pindah scene
        SceneManager.LoadScene("MainMenu"); // load scenen MainMenu
    }
}
