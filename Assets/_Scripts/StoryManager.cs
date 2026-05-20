using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject storyPanel;

    public RawImage backgroundImage;
    public TMP_Text storyText;
    public TMP_Text nextButtonText;

    public Texture[] backgrounds;

    string[] story =
    {
        "Di sebuah desa, terdapat remaja bernama Taher yang memiliki kepribadian yang buruk. Taher tidak pernah mendengarkan nasehat yang diberikan ibunya dan selalu menjawab.",
        "Hingga di suatu hari, ibu Taher tidak lagi memiliki kesabaran untuk menasehati Taher dan menyeret Taher keluar rumah.",
        "Dengan emosi, Ibu mengusir Taher dari rumah, Ibu berharap, anaknya akan berubah menjadi lebih baik.",
        "Taher yang terbiasa mendapat kecukupan, kini merasa khawatir setelah diusir. Akhirnya, Taher bertekad untuk berubah menjadi pribadi yang lebih baik."
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

    // Kalau halaman terakhir
    if(index == story.Length - 1)
    {
        nextButtonText.text = "Selesai";
    }
    else
    {
        nextButtonText.text = "Lanjut";
    }
}
}