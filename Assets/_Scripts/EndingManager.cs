using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    [Header("UI Transisi")]
    public CanvasGroup fadeOverlay;
    public float fadeDuration = 2f;
    public string namaSceneEnding = "Ending";

    [Header("Audio SFX")]
    public AudioClip sfxClickLanjut; //Tarik MP3 suara klik lanjut
    private AudioSource audioMesin;  // Variabel penampung mesin otomatis

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        {
        // Mengambil komponen AudioSource yang nempel di objek EndingManager
        audioMesin = GetComponent<AudioSource>();
        }
    }
    public void LanjutKeEnding()
    {
        // SFX KLIK sebelum waktu dinormalkan dan scene mulai meredup
        if (audioMesin != null && sfxClickLanjut != null)
        {
            audioMesin.PlayOneShot(sfxClickLanjut);
        }
        // kembalikan waktu normal
        Time.timeScale = 1f;

        // mulai proses meredupkan layar
        StartCoroutine(ProsesFadeOut());
    }
    IEnumerator ProsesFadeOut()
    {
        float timer = 0f;

        // overlay tidak halangi klik lain
        if (fadeOverlay != null)
        {
            fadeOverlay.blocksRaycasts = true;

            // loop menaikkan alpha
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Lerp(0,1,timer/fadeDuration);
                yield return null;
            }
        }

        // pindah scene
        SceneManager.LoadScene(namaSceneEnding);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
