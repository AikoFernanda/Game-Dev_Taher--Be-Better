using UnityEngine;
using TMPro;

public class ClueManager : MonoBehaviour
{
    public static ClueManager instance;

    public GameObject cluePanel;
    public TextMeshProUGUI clueText;

    private string currentRequester = ""; // siapa yang sedang pakai panel

    void Awake()
    {
        instance = this;
        if (cluePanel != null) cluePanel.SetActive(false);
    }

    public void ShowClue(string questID, string pesan)
    {
        currentRequester = questID;
        if (cluePanel != null) cluePanel.SetActive(true);
        if (clueText != null) clueText.text = pesan;
    }

    public void HideClue(string questID)
    {
        // Hanya sembunyikan kalau yang minta adalah yang sedang aktif
        if (currentRequester == questID)
        {
            currentRequester = "";
            if (cluePanel != null) cluePanel.SetActive(false);
        }
    }
}