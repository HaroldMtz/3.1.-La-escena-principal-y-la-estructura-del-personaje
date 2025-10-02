using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ajustes")]
    public float radius = 0.18f;
    public LayerMask groundMask;
    public Transform feetPoint;

    public bool IsGrounded()
    {
        if (feetPoint == null) feetPoint = transform;
        return Physics2D.OverlapCircle(feetPoint.position, radius, groundMask) != null;
    }

    void OnDrawGizmosSelected()
    {
        if (feetPoint == null) feetPoint = transform;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPoint.position, radius);
    }
}
