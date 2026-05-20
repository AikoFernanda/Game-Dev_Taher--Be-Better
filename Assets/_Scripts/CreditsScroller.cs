using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    [Header("Pengaturan Kecepatan")]
    public float speed = 400f;     // angka murni piksel layar
    public float limitY = 1500f;   // Titik di mana teks berhenti/pindah scene

    [Header("Pengaturan Waktu Muncul")]
    public float startDelay = 5f; // Teks jalan setelah delay selesai
    
    private float timer = 0f;
    private RectTransform rectTransform; // Komponen khusus UI

    void Start()
    {
        // Ambil komponen RectTransform milik teks saat game mulai
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Hitung waktu berjalan
        timer += Time.deltaTime;

        // Jika waktu belum mencapai delay, tahan teks 
        if (timer < startDelay)
        {
            return; 
        }

        // Gerakkan koordinat Anchor UI langsung ke atas
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        }

        // Jika teks sudah sampai atas (berdasarkan anchoredPosition Y) atau player klik mouse
        if (rectTransform.anchoredPosition.y > limitY || Input.GetMouseButtonDown(0))
        {
            // Kembali ke Main Menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}