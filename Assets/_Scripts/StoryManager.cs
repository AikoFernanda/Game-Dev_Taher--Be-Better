using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject storyPanel;

    public RawImage backgroundImage;
    public TMP_Text storyText;

    public Texture[] backgrounds;

    string[] story =
    {
        "Di sebuah dunia gelap...",
        "Cahaya mulai muncul...",
        "Cahaya mulai muncul...",
        "Petualangan dimulai..."
    };

    int index = 0;

    public void StartStory()
    {
        mainMenu.SetActive(false);
        storyPanel.SetActive(true);

        ShowPage();
    }

    public void NextPage()
    {
        index++;

        if(index < story.Length)
        {
            ShowPage();
        }
        else
        {
            Debug.Log("Story selesai");
        }
    }

    void ShowPage()
    {
        backgroundImage.texture = backgrounds[index];
        storyText.text = story[index];
    }
}