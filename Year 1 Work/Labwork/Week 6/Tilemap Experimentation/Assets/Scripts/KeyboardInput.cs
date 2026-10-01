using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public Animator animator;
    public float horizontalSpeed = 7.0f;
    public float jumpForce = 10.0f;
    bool ground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().linearVelocityX = -horizontalSpeed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().linearVelocityX = horizontalSpeed;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        bool moving = Input.GetAxisRaw("Horizontal") != 0;
        animator.SetBool("isWalking", moving);

        if (Input.GetKeyDown(KeyCode.UpArrow) && ground == true && GetComponent<Rigidbody2D>().linearVelocityY >= 0.0f)
        {
            Jump();
        }

        bool jumping = Input.GetAxisRaw("Vertical") != 0;
        animator.SetBool("isJumping", jumping);

    }

    private void OnTriggerEnter2D(Collider2D check)
    {
        if (check.CompareTag("Ground"))
        {
            ground = true;
        }
        else { ground = false; }
    }


    private void Jump()
    {
        GetComponent<Rigidbody2D>().AddForceY(jumpForce, ForceMode2D.Impulse);
        ground = false;
    }
}

