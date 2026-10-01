using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public Animator animator;
    public float horizontalSpeed = 7.0f;
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
    }
}
