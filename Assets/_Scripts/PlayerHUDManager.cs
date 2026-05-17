using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Heart System")]
    public GameObject[] hearts;

    [Header("Quest Tracker")]
    public Slider reputationSlider;

    [Header("Quest Tracker")]
    public TextMeshProUGUI questText;

    // Fungsi untuk mengurangi nyawa Taher
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Jika index kurang dari nyawa sekarang, hati aktif. Jika tidak, hati mati.
            if (i < currentHealth) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }
    }

    // Fungsi untuk mengubah nilai reputasi (bisa ditambah atau dikurang)
    public void ChangeReputation(float amount)
    {
        reputationSlider.value += amount;
    }

    // Fungsi untuk memperbarui teks tugas di pojok layar
    public void UpdateQuestTracker(string newQuestInfo)
    {
        questText.text = newQuestInfo;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
