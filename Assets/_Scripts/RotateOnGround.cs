using UnityEngine;

public class RotateOnGround : MonoBehaviour
{
    [Header("Pengaturan Putaran")]
    public float rotationSpeed = 60f; // Kecepatan berputar (derajat per detik)

    void Update()
    {
        // Putar di sumbu Z (0, 0, 1) secara lokal
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
    }
}