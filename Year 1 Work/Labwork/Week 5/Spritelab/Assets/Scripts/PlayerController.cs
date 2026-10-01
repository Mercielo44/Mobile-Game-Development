using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 12f;
    public float maxSpeed = 5f;
    public float turnLerp = 12f;

    [Header("Water Drag")]
    public float linearDrag = 3f;

    [Header("Bounds (optional)")]
    public BoxCollider2D tankBounds;
    public float boundsPadding = 0.3f;
    public bool clampToBounds = true;

    [Header("Sprite")]
    public bool facesRightByDefault = true; // set false if your art faces left

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float inputX, inputY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.angularDamping = 0f;
        rb.freezeRotation = true;

        sr = GetComponent<SpriteRenderer>();

        // Ensure scale is positive to avoid mirrored rotations
        var s = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(s.x), Mathf.Abs(s.y), s.z);
    }

    void Update()
    {
        // Gather input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // Flip immediately based on input, fall back to velocity if no input
        float dirX = Mathf.Abs(inputX) > 0.01f ? inputX : rb.linearVelocity.x;
        float dirY = Mathf.Abs(inputY) > 0.01f ? inputY : rb.linearVelocity.y;
        bool movingLeft = dirX < 0f;
        if (sr != null && Mathf.Abs(dirX) > 0.01f)
        {
            sr.flipX = facesRightByDefault ? movingLeft : !movingLeft;
        }

        // Rotate to face movement without causing upside-down issue
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {   if (movingLeft != true)
            {
                float angle = Mathf.Atan2(rb.linearVelocity.y, Mathf.Abs(rb.linearVelocity.x)) * Mathf.Rad2Deg;
                // Use abs on x so left and right share the same tilt, flipX handles mirroring
                Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnLerp * Time.deltaTime);
            }
            else
            {

                float angle = Mathf.Atan2(rb.linearVelocity.y, Mathf.Abs(rb.linearVelocity.x)) * Mathf.Rad2Deg;
                angle *= -1;
                // Use abs on x so left and right share the same tilt, flipX handles mirroring
                Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnLerp * Time.deltaTime);
            }
            }

        // Keep inside bounds
        if (clampToBounds && tankBounds != null)
        {
            Vector3 p = transform.position;
            Bounds b = tankBounds.bounds;
            p.x = Mathf.Clamp(p.x, b.min.x + boundsPadding, b.max.x - boundsPadding);
            p.y = Mathf.Clamp(p.y, b.min.y + boundsPadding, b.max.y - boundsPadding);
            transform.position = p;
        }
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(inputX, inputY).normalized;

        rb.AddForce(input * acceleration, ForceMode2D.Force);

        // Water drag
        rb.linearVelocity = rb.linearVelocity / (1f + linearDrag * Time.fixedDeltaTime);

        // Max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}