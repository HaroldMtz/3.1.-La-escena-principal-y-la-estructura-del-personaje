using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // Soporta el New Input System si está activo
#endif

/// <summary>
/// Movimiento 2D básico con salto y ataque.
/// - Soporta teclado con Legacy o New Input System.
/// - Controla Animator (Speed / IsJumping / IsAttacking).
/// - Voltea el sprite según dirección.
/// Requiere: Rigidbody2D en Player y Animator + SpriteRenderer en el hijo "Graphics".
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
    [SerializeField] private float debugInputX;  
    [SerializeField] private float debugSpeed;   
    [SerializeField] private bool isGrounded;
    
    private bool jumpPressed;

    private static readonly int HashSpeed = Animator.StringToHash("Speed");
    private static readonly int HashIsJumping = Animator.StringToHash("IsJumping");
    private static readonly int HashIsAttacking = Animator.StringToHash("IsAttacking");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        rb.freezeRotation = true;
        if (rb.gravityScale <= 0f) rb.gravityScale = 3f;
        rb.sharedMaterial = null; 
    }

    void Update()
    {
        float x = 0f;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame) jumpPressed = true;

            if (Keyboard.current.jKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
                if (anim) anim.SetTrigger(HashIsAttacking);
        }
        else
#endif
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;

            if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

           if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                if (anim) anim.SetTrigger(HashIsAttacking);
        }

        debugInputX = Mathf.Clamp(x, -1f, 1f);
        debugSpeed = Mathf.Abs(debugInputX);

        if (anim) anim.SetFloat(HashSpeed, debugSpeed);

        if (sr)
        {
            if (debugInputX > 0f) sr.flipX = false;
            else if (debugInputX < 0f) sr.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        }
        else
        {
           isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        }

        rb.linearVelocity = new Vector2(debugInputX * speed, rb.linearVelocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        jumpPressed = false;

        
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
