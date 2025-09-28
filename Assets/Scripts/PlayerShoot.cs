using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    private SpriteRenderer sr;

    void Awake()
    {
        // Busca el SpriteRenderer en el hijo "Graphics"
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Dispara con J o K
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // Dirección depende del flipX del SpriteRenderer
        if (sr != null && sr.flipX)
            bulletScript.direction = Vector2.left;
        else
            bulletScript.direction = Vector2.right;
    }
}
