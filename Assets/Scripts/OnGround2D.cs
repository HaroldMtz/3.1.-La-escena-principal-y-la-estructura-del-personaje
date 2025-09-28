using UnityEngine;

public class OnGround2D : MonoBehaviour
{
    public bool OnGround;
    [SerializeField] string groundTag = "Ground";
    SpriteRenderer sr;

    void Awake() { sr = GetComponent<SpriteRenderer>(); }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.collider.CompareTag(groundTag)) {
            OnGround = true;
            if (sr) sr.color = Color.cyan; // opcional
        }
    }

    void OnCollisionExit2D(Collision2D c) {
        if (c.collider.CompareTag(groundTag)) {
            OnGround = false;
            if (sr) sr.color = Color.white; // opcional
        }
    }
}
