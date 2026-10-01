using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundMask;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.0f;      // faster fall
    public float lowJumpMultiplier = 2.0f;   // tap jump = shorter
    public float coyoteTime = 0.1f;          // grace time after leaving edge
    public float jumpBufferTime = 0.1f;      // early jump buffering

    Rigidbody2D rb;
    float xInput;
    float coyoteCounter;
    float jumpBufferCounter;

    bool wasGrounded;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        // Horizontal
        xInput = Input.GetAxisRaw("Horizontal");

        // Jump input buffer
        if (Input.GetButtonDown("Jump")) { jumpBufferCounter = jumpBufferTime; }
        else { jumpBufferCounter -= Time.deltaTime; }

        // Grounded?
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
        if (grounded) 
        {
            coyoteCounter = coyoteTime;
        }
        else 
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Execute jump if buffered and allowed by coyote
        if (jumpBufferCounter > 0.0f && coyoteCounter > 0.0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0; 
        }

        // Better jump feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
    void LateUpdate()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
        if (grounded && !wasGrounded)
        {
            var src = GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
            if (src != null & Mathf.Abs(rb.linearVelocity.y) > 4f) src.GenerateImpulse();
        }
        wasGrounded = grounded;

    }
}