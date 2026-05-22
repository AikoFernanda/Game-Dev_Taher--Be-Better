using UnityEngine;

public class NpcClue : MonoBehaviour
{
    [Header("Quest Target")]
    public string myQuestID;

    [Header("Player & Distance Settings")]
    public Transform playerTransform;
    public float radiusClue = 3f;

    [Header("Clue Message")]
    [TextArea(3, 5)]
    public string pesanClue;

    private bool isClueShowing = false;
    private bool wasQuestKelarLastFrame = false; // tracking frame sebelumnya
    private bool isPlayerInRadius = false;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null || QuestManager.instance == null) return;

        bool isQuestKelar = QuestManager.instance.CheckIfQuestCompleted(myQuestID);

        // Quest belum selesai: reset semua state
        if (!isQuestKelar)
        {
            wasQuestKelarLastFrame = false;
            isPlayerInRadius = false;
            if (isClueShowing)
            {
                isClueShowing = false;
                ClueManager.instance.HideClue(myQuestID);
            }
            return;
        }

        float jarak = Vector3.Distance(transform.position, playerTransform.position);
        bool isDekat = jarak <= radiusClue;

        // Quest BARU saja selesai di frame ini: catat posisi player dulu
        // Jangan langsung tampilkan, tunggu player keluar lalu masuk lagi
        if (!wasQuestKelarLastFrame)
        {
            wasQuestKelarLastFrame = true;
            isPlayerInRadius = isDekat; // catat posisi awal tanpa trigger show
            return; // skip frame pertama
        }

        // Deteksi player masuk radius (dari luar ke dalam)
        if (isDekat && !isPlayerInRadius)
        {
            isPlayerInRadius = true;
            if (!isClueShowing)
            {
                isClueShowing = true;
                ClueManager.instance.ShowClue(myQuestID, pesanClue);
            }
        }
        // Deteksi player keluar radius
        else if (!isDekat && isPlayerInRadius)
        {
            isPlayerInRadius = false;
            if (isClueShowing)
            {
                isClueShowing = false;
                ClueManager.instance.HideClue(myQuestID);
            }
        }
    }
}