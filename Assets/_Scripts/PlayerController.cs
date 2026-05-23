using UnityEngine;

// Menambahkan AudioSource otomatis jika belum ada di GameObject
[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 720.0f;
    public float jumpForce = 5.0f;

    [Header("Physics Settings")]
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [Header("Audio Settings (Footsteps)")]
    [Tooltip("Masukkan 2 file MP3 hentakan kaki di sini")]
    public AudioClip[] footstepClips; // kiri, kanan
    private AudioSource audioSource;
    private int currentFootstepIndex = 0; // Untuk melacak giliran kaki kanan/kiri

    private Rigidbody rb;
    private Animator anim;

    // 2. Cache the camera and pre-calculated direction
    private Transform mainCamTransform;
    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Ambil komponen AudioSource

        // Cache the camera transform so we don't search for it every frame
        if (Camera.main != null)
        {
            mainCamTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // 3. Safety check to prevent errors if groundCheck isn't assigned
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;

        // 4. Calculate camera-relative direction in Update, NOT FixedUpdate
        if (mainCamTransform != null && moveInput.magnitude >= 0.1f)
        {
            Vector3 camForward = mainCamTransform.forward;
            Vector3 camRight = mainCamTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            movementDirection = (camForward * moveInput.z) + (camRight * moveInput.x);
        }
        else
        {
            movementDirection = Vector3.zero;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", moveInput.magnitude);
            // 5. Send grounded state to Animator for falling/landing animations
            anim.SetBool("IsGrounded", isGrounded);
        }
    }

    void Jump()
    {
        // Reset Y velocity before jumping to ensure consistent jump heights
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        // 6. Only apply physical movement here using the direction calculated in Update
        if (movementDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Fungsi ini akan dipanggil otomatis via Animation Event di dalam animasi Lari
    public void PlayFootstep()
    {
        // Validasi: Hanya bunyikan jika Taher menyentuh tanah, sedang bergerak, dan file audio sudah dimasukkan
        if (isGrounded && movementDirection.magnitude >= 0.1f && footstepClips.Length > 0)
        {
            // Ambil clip berdasarkan giliran (index 0 atau index 1)
            AudioClip clipToPlay = footstepClips[currentFootstepIndex];
            
            // Putar suaranya sekali tembak (PlayOneShot)
            audioSource.PlayOneShot(clipToPlay);

            // Ganti giliran indeks untuk langkah berikutnya (bergantian antara 0 dan 1)
            currentFootstepIndex = (currentFootstepIndex + 1) % footstepClips.Length;
        }
    }
}