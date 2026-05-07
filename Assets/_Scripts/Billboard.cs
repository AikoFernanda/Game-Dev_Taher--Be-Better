using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // buat UI selalu hadap kamera
        transform.LookAt(transform.position + Camera.main.transform.forward);    }
}
