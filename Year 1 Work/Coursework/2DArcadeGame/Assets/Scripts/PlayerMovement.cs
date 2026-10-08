using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
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

    const float MinSwipeDp = 15f;
    const float MaxSwipeTime = 0.075f;

    private Vector2 initialMousePosition;
    private float initialTouchTime;

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

    public void OnJump(InputAction.CallbackContext context)
    {
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
        float angleCalc = (currentSpeed / (Mathf.PI * diameter)) * roationDirectionMultiplier;
        Quaternion currentAngle = Quaternion.identity * Quaternion.AngleAxis(angleCalc, Vector3.forward);
        transform.rotation *= currentAngle;
    }

    public void OnCheckSwipe(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
        {
            initialMousePosition = ((Pointer)context.control.device).position.ReadValue();
            initialTouchTime = Time.time;
            return;
        }
        if (context.ReadValue<float>() < 0.5f)
        {
            float time = Time.time - initialTouchTime;
 
            if (time < MaxSwipeTime && canJump)
            {
                print("Jump");
                GetComponent<Rigidbody2D>().AddForceY(jumpForce, ForceMode2D.Impulse);
                canJump = false;
                return;
            }


            Vector2 delta = ((Pointer)context.control.device).position.ReadValue() - initialMousePosition;
            float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
            float distDp = delta.magnitude / (dpi / 160f);

            print("MinDist");
            if (distDp < MinSwipeDp) return;

            if (distDp >= 50f && time <= 0.4f)
            {
                print("GFLIP");
                OnGFlip();
            }
        }
    }

}
