using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIControllers : MonoBehaviour
{
    [Header("Swim")]
    public float swimSpeed = 3.5f;
    public float turnLerp = 6f;
    public bool facesRightByDefault; // set false if your sprite faces left by default

    [Header("Targeting")]
    public float arriveDistance = 0.25f;
    public Vector2 retargetTimeRange = new Vector2(2.5f, 5f);

    [Header("Bounds")]
    public BoxCollider2D tankBounds;
    public float boundsPadding = 0.4f;

    [Header("Avoidance")]
    public float feelerDistance = 1.2f;
    public float avoidStrength = 6f;
    public LayerMask obstacleMask;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 targetPos;
    private float retargetTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.angularDamping = 0f;
        rb.freezeRotation = true;

        sr = GetComponent<SpriteRenderer>();
        PickNewTarget(true);
    }

    void Update()
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f || Vector2.Distance(transform.position, targetPos) < arriveDistance)
        {
            PickNewTarget(false);
        }

        // --- FLIP SPRITE BASED ON HORIZONTAL DIRECTION ---
        if (sr != null)
        {
            // Prefer velocity; if very small, fall back to intent toward target
            float dirX = Mathf.Abs(rb.linearVelocity.x) > 0.02f
                ? rb.linearVelocity.x
                : (targetPos.x - transform.position.x);

            if (Mathf.Abs(dirX) > 0.02f)
            {
                // If your sprite artwork faces right by default, flip when dirX < 0.
                // If it faces left by default, invert this with facesRightByDefault.
                bool movingLeft = dirX < 0f;
                sr.flipY = facesRightByDefault ? movingLeft : !movingLeft;
            }
        }
        // -----------------------------------------------

        // Rotate to face velocity (keeps that fishy turn)
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnLerp * Time.deltaTime);
        }
    }


    void FixedUpdate()
    {
        Vector2 pos = rb.position;

        // Desired direction to target
        Vector2 toTarget = (targetPos - pos);
        Vector2 desired = toTarget.normalized * swimSpeed;

        // Simple wall avoidance via feeler ray
        Vector2 forward = rb.linearVelocity.sqrMagnitude > 0.01f ? rb.linearVelocity.normalized : toTarget.normalized;
        RaycastHit2D hit = Physics2D.Raycast(pos, forward, feelerDistance, obstacleMask);
        if (hit.collider != null)
        {
            // Steer away from obstacle normal
            Vector2 away = Vector2.Reflect(forward, hit.normal);
            desired += away.normalized * avoidStrength;
        }

        // Stay inside bounds by nudging back in
        if (tankBounds != null)
        {
            Bounds b = tankBounds.bounds;
            Vector2 nudge = Vector2.zero;
            if (pos.x < b.min.x + boundsPadding) nudge.x = 1f;
            else if (pos.x > b.max.x - boundsPadding) nudge.x = -1f;
            if (pos.y < b.min.y + boundsPadding) nudge.y = 1f;
            else if (pos.y > b.max.y - boundsPadding) nudge.y = -1f;
            if (nudge != Vector2.zero) desired += nudge.normalized * swimSpeed;
        }

        // Smooth accelerate toward desired
        Vector2 steer = desired - rb.linearVelocity;
        rb.AddForce(steer, ForceMode2D.Force);
    }

    void PickNewTarget(bool first)
    {
        if (tankBounds != null)
        {
            Bounds b = tankBounds.bounds;
            float x = Random.Range(b.min.x + boundsPadding, b.max.x - boundsPadding);
            float y = Random.Range(b.min.y + boundsPadding, b.max.y - boundsPadding);
            targetPos = new Vector2(x, y);
        }
        else
        {
            // Fallback: random circle around current position
            Vector2 rnd = Random.insideUnitCircle * 3f;
            targetPos = (Vector2)transform.position + rnd;
        }

        retargetTimer = Random.Range(retargetTimeRange.x, retargetTimeRange.y);
        if (first)
        {
            retargetTimer *= 0.5f; // get moving quickly at start
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize target and feeler
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(targetPos, 0.1f);

        if (rb != null)
        {
            Vector2 forward = rb.linearVelocity.sqrMagnitude > 0.01f ? rb.linearVelocity.normalized : Vector2.right;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + forward * feelerDistance);
        }
    }
}