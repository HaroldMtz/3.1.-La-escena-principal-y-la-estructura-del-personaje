using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bulletStraightPrefab; // J -> Bullet.prefab
    public GameObject bulletArcPrefab;      // K -> BulletArc.prefab

    [Header("Referencias")]
    public Transform firePoint;             // hijo frente a la mano/arma
    public Animator animator;               // Animator del hijo "Graphics" (opcional)
    public string shootTriggerName = "Attack";

    [Header("Cadencia")]
    public float cooldown = 0.16f;
    float nextFire;

    SpriteRenderer sr; // del hijo Graphics

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.time < nextFire) return;

        if (Input.GetKeyDown(KeyCode.J)) Fire(bulletStraightPrefab, Mode.Straight);
        else if (Input.GetKeyDown(KeyCode.K)) Fire(bulletArcPrefab, Mode.Arc);
    }

    enum Mode { Straight, Arc }

    void Fire(GameObject prefab, Mode mode)
    {
        if (!prefab || !firePoint) return;

        nextFire = Time.time + cooldown;

        var go = Instantiate(prefab, firePoint.position, Quaternion.identity);

        // Dirección según hacia dónde mira tu sprite (flipX) o la escala
        bool facingRight = (sr != null) ? !sr.flipX : (transform.localScale.x >= 0f);
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;

        switch (mode)
        {
            case Mode.Straight:
                go.GetComponent<Bullet>()?.Init(dir);
                break;
            case Mode.Arc:
                go.GetComponent<BulletArc>()?.Init(dir);
                break;
        }

        if (animator && !string.IsNullOrEmpty(shootTriggerName))
            animator.SetTrigger(shootTriggerName);
    }
}
