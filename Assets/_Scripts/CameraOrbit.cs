using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // object player
    public float distance = 5.0f; // jarak kamera
    public float sensitivity = 3.0f; // kecepatan putaran
    public float heightOffset = 1.5f;     //mengatur tinggi pandangan
    public float pitchMin = -20f; // batas bawah
    public float pitchMax = 60f; // batas atas
    private float yaw = 0f; // rotasi kiri-kanan
    private float pitch = 0f; // rotasi atas-bawah
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sembunyikan kursor agar tidak mengganggu
        Cursor.lockState = CursorLockMode.Locked;
        // ambil rotasi awal kamera jika ada
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ambil input mouse
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // batasi rotasi vertical (Pitch) agar kamera tidak berputar terbalik
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // hitung rotasi dan posisi baru
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // titik fokus player sedikit diatas kaki
        Vector3 focusPosition = target.position + new Vector3(0, heightOffset, 0);

        // rumus posisi kamera
        Vector3 position = focusPosition - (rotation * Vector3.forward * distance);

        // update posisi dan rotasi kamera
        transform.rotation = rotation;
        transform.position = position;
    }
}
