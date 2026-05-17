using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // untuk run coroutine (fungsi delay waktu)

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Heart System")]
    public GameObject[] hearts;

    [Header("Quest Tracker")]
    public Slider reputationSlider;

    [Header("Quest Tracker")]
    public TextMeshProUGUI questText;

    [Header("New Quest Elements")]
    public TextMeshProUGUI timerText;
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;

    // fungsi update angka timer
    public void UpdateTimerText(string text, bool show)
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(show);
            timerText.text = text;
        }
    }

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

    // Fungsi untuk memicu popup notifikasi dari luar skrip
    public void TriggerPopupNotification(string message, Color textColor)
    {
        StopAllCoroutines(); // stop notifikasi sebelumnya jika ada tabrakan
        StartCoroutine(ShowNotificationRoutine(message, textColor));
    }

    // Perintah delay: Munculkan popup, tunggu 3 detik, lalu sembunyikan lagi
    private IEnumerator ShowNotificationRoutine(string message, Color textColor)
    {
        if (notificationPanel != null && notificationText != null)
        {
            notificationText.text = message;
            notificationText.color = textColor;
            notificationPanel.SetActive(true); 

            yield return new WaitForSeconds(3f); 
            notificationPanel.SetActive(false); 
        }
    }
}
