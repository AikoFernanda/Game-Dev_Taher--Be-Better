using UnityEngine;

public class RotateOnGround : MonoBehaviour
{
    [Header("Pengaturan Putaran dan pulse")]
    public float rotationSpeed = 60f; // Kecepatan berputar (derajat per detik)
    public float pulseSpeed = 3f; // kecepatan memudar (fade in/out)
    private float minAlpha = 0.17f;
    private float maxAlpha = 1.0f;
    private Material mat;
    private Color baseColor;

    void Start()
    {
        // ambil material dari object
        mat = GetComponent<MeshRenderer>().material;
        // ambil warna asli objek
        if (mat.HasProperty("_BaseColor"))
        {
            baseColor = mat.GetColor("_BaseColor");
        }
        else
        {
            baseColor = mat.color;
        }
    }
    void Update()
    {
        // Putar di sumbu Z (0, 0, 1) secara lokal
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);

        // gelombang naik turun
        float lerp = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // kalukulasi nilai alpha baru based gelombang
        baseColor.a = Mathf.Lerp(minAlpha, maxAlpha, lerp);

        // input kembali warna alpha yg sudah diubah ke mat
        if(mat.HasProperty("_BaseColor"))
        {
            mat.SetColor("_BaseColor", baseColor);
        }
        else
        {
            mat.color = baseColor;
        }
    }
}