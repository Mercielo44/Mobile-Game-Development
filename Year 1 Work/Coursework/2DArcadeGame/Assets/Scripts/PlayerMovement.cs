using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    bool canJump;
    bool canGFlip;
    int roationDirectionMultiplier = -1;

    Vector2 moveInput;

    InputAction moveAction;

    [SerializeField] InputActionReference moveVerb;

    void OnEnable() => moveVerb.action.Enable();
    void OnDisable() => moveVerb.action.Disable();

    const float MinSwipeDp = 50f;
    const float MaxSwipeTime = 0.4f;

    GameObject gameManager;
    private GameManagerCode gameManagerCode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        canJump = true;
        canGFlip = true;
        roationDirectionMultiplier = -1;

        gameManager = GameObject.Find("Game Manager");
        gameManagerCode = gameManager.GetComponent<GameManagerCode>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = moveVerb.action.ReadValue<Vector2>();
        GetComponent<Rigidbody2D>().linearVelocityX = move.x * speed;
        SetRoation(speed);

    }

    //void OnMove(InputValue input)
    //{
    //   moveInput = input.Get<Vector2>();
    //}

    void OnJump()
    {
        print("jump");
        if (canJump)
        {
            GetComponent<Rigidbody2D>().AddForceY(jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (canGFlip == false)
            {
                roationDirectionMultiplier *= -1;
            }
            SetJumpToTrue();
            canGFlip = true;
        }

        if (collision.gameObject.tag == "Collidable")
        {
            gameManagerCode.playerDead = true;
            gameManagerCode.saveGame();
            gameManagerCode.StartCoroutine(gameManagerCode.LoadGameoverScreen());
            Destroy(gameObject);
        }
    }

    private void SetJumpToTrue()
    {
        canJump = true;
    }

    private void GravityFlipper()
    {
        if (canGFlip)
        {
            GetComponent<Rigidbody2D>().AddForceY(jumpForce, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().gravityScale *= -1.0f;
            canGFlip = false;
            jumpForce *= -1;
        }
    }

    private void OnGFlip()
    {
        GravityFlipper();
    }

    private void SetRoation(float currentSpeed)
    {
        float diameter = GetComponent<Collider2D>().bounds.size.y;
        float angleCalc = (currentSpeed/ (Mathf.PI * diameter)) * roationDirectionMultiplier;
        Quaternion currentAngle = Quaternion.identity * Quaternion.AngleAxis(angleCalc, Vector3.forward);
        transform.rotation *= currentAngle;
    }

    void CheckSwipe(Touch touch)
    {
        if (touch.phase != TouchPhase.Ended) return;

        Vector2 delta = touch.screenPosition - touch.startScreenPosition;
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
        float distDp = delta.magnitude / (dpi / 160f);
        float time = (float)(touch.time - touch.startTime);

        if (distDp < MinSwipeDp || time > MaxSwipeTime) return;

        if (distDp >= 50f && time <= 0.4f)
        {
            OnGFlip();
        }
    }


    }
