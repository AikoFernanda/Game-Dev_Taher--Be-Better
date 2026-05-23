using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cutscene : MonoBehaviour
{
    [System.Serializable]
    public struct Slide
    {
        public Texture image;
        [TextArea(3, 5)] public string storyText;
    }

    [Header("Daftar Cerita")]
    public Slide[] allSlides;
    private int currentSlideIndex = 0;

    [Header("Referensi UI")]
    public RawImage displayImage;
    public TextMeshProUGUI displayText;
    public float typingSpeed = 0.05f; // Di-set lebih cepat sedikit agar nyaman dibaca

    [Header("Transition Settings")]
    public string sceneToLoad = "Main";
    public Graphic faderPanel;      // Tarik objek Fader
    public float fadeDuration = 1.5f;         // Durasi waktu menggelap (detik)

    [Header("Audio SFX")]
    public AudioClip sfxClickNext;  // Tarik MP3 suara klik lanjut/E
    private AudioSource audioMesin; // Variabel penampung mesin otomatis

    private bool isTyping = false;
    private string fullText;
    private Coroutine typingCoroutine; // penampung Coroutine eja

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // auto Mengambil komponen AudioSource yang nempel di objek Cutscene
        audioMesin = GetComponent<AudioSource>();
        // kursor terkunci saat cutscene
        Cursor.visible = false;
        // Di awal game, pastikan panel fader dalam kondisi transparan (Alpha = 0)
        if (faderPanel != null)
        {
            Color c = faderPanel.color;
            c.a = 0f;
            faderPanel.color = c;
        }

        ShowSlide();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // jika mengeja, langsung munculkan teks utuh
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                displayText.text = fullText;
                isTyping = false; // Set false agar klik E berikutnya bisa lanjut slide
            }
            else
            {
                // sfx Setiap kali tombol E ditekan, suara klik langsung berbunyi
                if (audioMesin != null && sfxClickNext != null)
                {
                    audioMesin.PlayOneShot(sfxClickNext);
                }
                // jika sudah utuh, baru boleh lanjut slide
                NextSlide();
            }
        }
    }

    void ShowSlide()
    {
        displayImage.texture = allSlides[currentSlideIndex].image; // textures
        fullText = allSlides[currentSlideIndex].storyText;

        // Pemicu ejaan teks berjalan aman
        typingCoroutine = StartCoroutine(TypeStory(fullText));
    }

    // Mengembalikan fungsi eja teks yang hilang
    System.Collections.IEnumerator TypeStory(string text)
    {
        isTyping = true;
        displayText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    System.Collections.IEnumerator FadeAndLeave()
    {
        if (faderPanel != null)
        {
            // Pastikan objeknya aktif secara visual sebelum transisi dimulai
            faderPanel.gameObject.SetActive(true);
            faderPanel.enabled = true;

            float counter = 0f;

            // Naikkan Alpha perlahan dari 0 ke 1
            while (counter < fadeDuration)
            {
                counter += Time.deltaTime;
                float alphaBaru = Mathf.Lerp(0f, 1f, counter / fadeDuration);

                // Set warna hitam dengan alpha yang terus bertambah
                faderPanel.color = new Color(0f, 0f, 0f, alphaBaru);

                yield return null;
            }
        }

        // Setelah hitam pekat, baru pindah scene
        SceneManager.LoadScene(sceneToLoad);
    }

    void NextSlide()
    {
        currentSlideIndex++;

        if (currentSlideIndex < allSlides.Length)
        {
            ShowSlide();
        }
        else
        {
            // transisi dulu 
            StartCoroutine(FadeAndLeave());
        }
    }
}