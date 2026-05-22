using UnityEngine;
using UnityEngine.SceneManagement; // Untuk mengendalikan perpindahan scene
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel; // Tarik MainMenuPanel
    public GameObject settingsPanel; // Tarik SettingsPanel
    public GameObject controlsPanel; // Tarik ControlsPanel
    public GameObject aboutPanel; // Tarik AboutPanel

    [Header("Transition Settings")]
    public CanvasGroup fadeScreen; // tarik objek fadepanel
    public float fadeDuration = 1.0f; // durasi animasi menggelap
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Audio Settings")]
    public AudioSource uiAudioSource; // Tarik komponen Audio Source
    public AudioClip clickSfx;        // Tarik file suara klik
    void Start()
    {
        // awal game layar transparan
        if (fadeScreen != null)
        {
            // Kondisi awal game: Hanya Main Menu yang muncul
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(false);
            if (aboutPanel != null) aboutPanel.SetActive(false);
            fadeScreen.alpha = 0f;
        }
    }

    public void PlayClickSound()
    {
        if (uiAudioSource != null && clickSfx != null)
        {
            uiAudioSource.PlayOneShot(clickSfx);
        }
    }

    // fungsi yang akan dipanggil oleh tombol play
    public void Playgame()
    {
        PlayClickSound(); // bunyikan sfx click
        // jalankan perintah hitung mundur transisi di background
        StartCoroutine(FadeAndLoadScene("IntroCutscene")); // scene utama
    }

    // Fungsi Navigas
    // Settings
    public void OpenSettings()
    {
        PlayClickSound(); // bunyikan sfx click
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Membuka menu Panduan Tombol
    public void OpenControls()
    {
        PlayClickSound(); // bunyikan sfx click
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(true);
    }

    public void OpenAbout()
    {
        PlayClickSound(); // bunyikan sfx click
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(true);
    }

    // Back button
    public void BackToMainMenu()
    {
        PlayClickSound(); // bunyikan sfx click
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // fungsi tombol keluar game
    public void QuitGame()
    {
        PlayClickSound(); // bunyikan sfx click
        #if UNITY_EDITOR
        // Jika dimainkan di dalam Unity Editor MacBook, matikan Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Jika sudah jadi game asli (.app / .exe), matikan aplikasinya
        Application.Quit();
        #endif
    }

    // coroutine untuk animasi fade out (gak ribet bikin animasi clip)
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if(fadeScreen != null)
        {
            float timer = 0f;
            // selama timer belum mencapai durasi, naikkan alpha panel hitam perlahan
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeScreen.alpha = timer / fadeDuration;
                yield return null; // tunggu sampai frame berikutnya
            }
            // setelah layar hitam pekat, baru pindah Main scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
