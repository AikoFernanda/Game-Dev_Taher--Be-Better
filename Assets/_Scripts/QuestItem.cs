using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Jika yang menyentuh dompet adalah Player
        if (other.CompareTag("Player"))
        {
            // Beritahu QuestManager bahwa quest sukses
            if (QuestManager.instance != null)
            {
                QuestManager.instance.RegisterItemCollected(gameObject.tag);
            }

            // Sembunyikan item yang sudah diambil agar tidak bisa diambil dua kali
            gameObject.SetActive(false);
        }
    }
}