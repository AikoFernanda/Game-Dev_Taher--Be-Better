using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // Untuk detektif UI
using UnityEngine.InputSystem;  // TAMBAHKAN BARIS INI (Biar Unity kenal 'Mouse')

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText; // tarik teks utama UI
    public GameObject dialogBox; // tarik panel/Image Background UI
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Choice System")]
    public GameObject choicePanel; // Tarik Choice_Panel ke sini
    public TextMeshProUGUI posButtonText; // Tarik teks di dalam Positif
    public TextMeshProUGUI negButtonText; // Tarik teks di dalam Negatif  
    public TextMeshProUGUI neutButtonText;// Tarik teks di dalam Netral
    private Queue<string> sentences; // antrean kalimat
    [HideInInspector] public bool isWaitingForChoice = false; // Mencegah pemain pencet E saat milih
    private NpcInteraction currentNPC; // Menyimpan referensi NPC yang sedang diajak bicara
    void Start()
    {
        sentences = new Queue<string>();
        dialogBox.SetActive(false); // sembunyikan dialog saat awal game
        choicePanel.SetActive(false);
    }

    public void StartDialog(string[] lines, NpcInteraction npc)
    {
        dialogBox.SetActive(true);
        choicePanel.SetActive(false);
        isWaitingForChoice = false;
        currentNPC = npc;
        
        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line); // masukkan semua kalimat ke antrean
        }
        DisplayNextSentence();
    }

    void ShowChoices()
    {
        isWaitingForChoice = true;
        // membersihkan teks cerita npc
        dialogText.text = "Pilih tindakan Taher:";
        // ambil teks dari np yang sedang berbicara
        if (currentNPC != null)
        {
            posButtonText.text = currentNPC.teksPositif;
            negButtonText.text = currentNPC.teksNegatif;
            neutButtonText.text = currentNPC.teksNetral;
        }
        choicePanel.SetActive(true); // munculkan panel setelah teksnya berubah

        // muncul kursor untuk memilih
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisplayNextSentence()
    {
        // Jika kalimat tinggal 1 dan tombol E dipencet, berarti itu kalimat terakhir
        if(sentences.Count == 0)
        {
            ShowChoices();
            return;
        }
        string sentence = sentences.Dequeue(); // ambil kalimat paling depan
        dialogText.text = sentence;
    }

    // Fungsi yang akan dipanggil saat tombol diklik
    public void SelectChoice(string type)
    {
        choicePanel.SetActive(false);
        isWaitingForChoice = false;

        // sembunyikan kembali kursor setelah memilih
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (type == "Positif")
        {
            currentNPC.TriggerPositiveResponse();
        }
        else if (type == "Negatif")
        {
            currentNPC.TriggerNegativeResponse();
        }
        else if (type == "Netral")
        {
            currentNPC.TriggerNeutralResponse();
        }
    }

    public void EndDialog()
    {
        dialogBox.SetActive(false);
        choicePanel.SetActive(false);
        isWaitingForChoice = false;

        // sembunyikan kursor kembali jika dialog selesai/batal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Dialog Selesai.");
    }
    // Update is called once per frame
    void Update()
    {
        // Kode detektif ini hanya berjalan saat tombol pilihan muncul di layar
        if (isWaitingForChoice)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
        
            // Mengambil posisi mause dari New Input System
            if (Mouse.current != null)
            {
                pointerData.position = Mouse.current.position.ReadValue();
            }

            // Menampung hasil "sinar laser" klik mouse
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            // Jika mouse menyentuh sesuatu di UI, cetak namanya di Console
            if (results.Count > 0)
            {
                Debug.Log("Kursor MacBook lagi menyentuh objek UI bernama: " + results[0].gameObject.name);
            }
        }
    }
}
