using UnityEngine;

public class Compass : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;   // Tarik objek Boy1 / Player ke sini
    public RectTransform compassWheel;  // Tarik objek Compass_Wheel ke sini

    void Update()
    {
        if (playerTransform != null && compassWheel != null)
        {
            // Ambil rotasi sudut Y (horizontal) dari Player
            float playerYRotation = playerTransform.eulerAngles.y;

            // Putar roda kompas di UI sesuai arah hadap player
            // Di Unity UI, rotasi Z bernilai positif berarti berlawanan arah jarum jam
            compassWheel.localRotation = Quaternion.Euler(0, 0, playerYRotation);
        }
    }
}
