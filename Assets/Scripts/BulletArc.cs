using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BulletArc : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad horizontal inicial del proyectil.")]
    public float speed = 10f;

    [Tooltip("Impulso vertical inicial para formar la parábola.")]
    public float launchY = 4f;

    [Tooltip("Escala de gravedad aplicada por el Rigidbody2D.")]
    public float gravityScale = 2f;

    [Header("Ciclo de vida")]
    [Tooltip("Tiempo máximo antes de destruir la bala si no colisiona (segundos).")]
    public float lifeTime = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Asegura que el collider NO sea trigger para que OnCollisionEnter2D se dispare.
        var col = GetComponent<Collider2D>();
        col.isTrigger = false;
    }

    /// <summary>
    /// Llamado por PlayerShoot al instanciar la bala. 'dir' debe ser (1,0) o (-1,0).
    /// </summary>
    public void Init(Vector2 dir)
    {
        dir = dir.normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, launchY);
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        // Destruye SOLO si choca con el piso (Layer "Ground")
        if (c.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Opcional: verifica que la normal apunte hacia arriba para evitar paredes/rampas.
            if (c.contacts.Length == 0 || c.contacts[0].normal.y > 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
