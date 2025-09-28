using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleMover2D : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jump = 9f;

    Rigidbody2D rb;
    OnGround2D ground;
    float moveX;
    bool wantJumpThisFrame;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<OnGround2D>();
    }

    void Update() {
        float x = 0f;
        var kb = Keyboard.current;
        if (kb != null) {
            if (kb.leftArrowKey.isPressed || kb.aKey.isPressed)  x -= 1f;
            if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) x += 1f;
            if (kb.spaceKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame)
                wantJumpThisFrame = true;
        }
        var gp = Gamepad.current;
        if (gp != null) {
            float lx = gp.leftStick.ReadValue().x;
            if (Mathf.Abs(lx) > 0.1f) x = lx;
            if (gp.buttonSouth.wasPressedThisFrame) wantJumpThisFrame = true;
        }
        moveX = Mathf.Clamp(x, -1f, 1f);
    }

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        if (wantJumpThisFrame && ground != null && ground.OnGround)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        wantJumpThisFrame = false;
    }
}
