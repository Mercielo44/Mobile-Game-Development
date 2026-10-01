using UnityEngine;

public class ChickenMovement : MonoBehaviour

{
    public float horizontalSpeed = 7.0f;
    public float flapForce = 25.0f;
    public bool ground;
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && ground == true && GetComponent<Rigidbody2D>().linearVelocityY >= 0.0f)
        {
            Jump();
        }

    }

    private void OnTriggerEnter2D(Collider2D check)
    {
        if (check.CompareTag("Ground"))
        {
            ground = true;
        }
        else { ground = false; }

        if (check.CompareTag("Trampoline"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        GetComponent<Rigidbody2D>().AddForceY(flapForce, ForceMode2D.Impulse);
        ground = false;
    }
    
}
