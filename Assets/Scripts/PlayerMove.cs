using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // Soporta el New Input System si está activo
#endif

/// <summary>
/// Movimiento 2D básico con salto y ataque.
/// - Lee teclado con Legacy o New Input System (Both).
/// - Actualiza Animator (Speed / IsJumping / IsAttacking).
/// - Voltea el sprite según dirección.
/// Requiere: Rigidbody2D en Player y Animator+SpriteRenderer en el hijo "Graphics".
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad horizontal.")]
    public float speed = 5f;

    [Tooltip("Fuerza de salto (impulso).")]
    public float jumpForce = 9f;

    [Header("Chequeo de suelo")]
    [Tooltip("Punto bajo de los pies.")]
    public Transform groundCheck;

    [Tooltip("Radio de detección del suelo.")]
    public float groundRadius = 0.20f;

    [Tooltip("Capa que usa el objeto Ground.")]
    public LayerMask groundLayer;

    // Referencias
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // Estado / debug visibles en el Inspector
    [SerializeField] private float debugInputX;  // -1 a 1
    [SerializeField] private float debugSpeed;   // |x|
    [SerializeField] private bool  isGrounded;

    // Buffers de entrada por frame
    private bool jumpPressed;

    // Hashes de parámetros (más rápido/seguro que strings)
    private static readonly int HashSpeed       = Animator.StringToHash("Speed");
    private static readonly int HashIsJumping   = Animator.StringToHash("IsJumping");
    private static readonly int HashIsAttacking = Animator.StringToHash("IsAttacking");

    void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();       // en "Graphics"
        sr   = GetComponentInChildren<SpriteRenderer>(); // en "Graphics"
    }

    void Update()
    {
        // --- Lectura de teclado (ambos sistemas) ---
        float x = 0f;

        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame) jumpPressed = true;

            // Ataque (J o K)
            if (Keyboard.current.jKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
                if (anim) anim.SetTrigger(HashIsAttacking);
        }
        else
        #endif
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;

            if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

            // Ataque (J o K)
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                if (anim) anim.SetTrigger(HashIsAttacking);
        }

        // Debug / Animator
        debugInputX = Mathf.Clamp(x, -1f, 1f);
        debugSpeed  = Mathf.Abs(debugInputX);

        if (anim) anim.SetFloat(HashSpeed, debugSpeed);

        // Voltear sprite
        if (sr)
        {
            if (debugInputX > 0f) sr.flipX = false;
            else if (debugInputX < 0f) sr.flipX = true;
        }
    }

    void FixedUpdate()
    {
        // ¿Estoy en el suelo?
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        else
            isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f; // respaldo si olvidaste asignar

        // Movimiento horizontal
        rb.linearVelocity = new Vector2(debugInputX * speed, rb.linearVelocity.y);

        // Salto (un solo impulso cuando está en suelo)
        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // limpia velocidad vertical
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        jumpPressed = false;

        // Animator: avisar si está en el aire
        if (anim) anim.SetBool(HashIsJumping, !isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
