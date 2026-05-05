using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 720.0f;
    public float jumpForce = 5.0f;

    [Header("Physics Settings")]
    public bool isGrounded;
    public LayerMask groundLayer; // pilih layer "default" atau "Ground"
    public Transform groundCheck; // objek kosong di telapak kaki

    private Rigidbody rb;
    private Animator anim;
    private Vector3 moveInput;

    void Start()
    {
        // mengambil referensi komponen saat game dimulai
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // cek apakah di tanah, buat lingkaran kecil di kaki untuk deteksi lantai
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        
        // mengambil input dari keyboard
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 

        moveInput = new Vector3(horizontal, 0, vertical).normalized;

        //Input lompat (space)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // update animasi mengirim nilai kecepatan ke parameter "Speed" di animator
        if (anim != null)
        {
            anim.SetFloat("Speed", moveInput.magnitude);
        }

    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        // logika pergerakan fisik RG
        if (moveInput.magnitude >= 0.1f)
        {
            // ambil arah depan dan kanan kamera
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            // buat arah tetap rata di tanah
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            //gabungkan input WASD dengan arah kamera
            Vector3 movementDirection = (camForward * moveInput.z) + (camRight * moveInput.x);

            // putar karakter ke arah tujuan berjalan
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // menggerakkan posisi karakter
            rb.MovePosition(rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
