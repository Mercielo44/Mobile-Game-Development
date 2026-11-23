using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class HovercarController : MonoBehaviour
{
    // GameManager GameManager

    public static HovercarController instance;
    public List<GameObject> springs;
    public Rigidbody rb;
    public GameObject prop;
    public GameObject HovercarCenterOfMass;
    public Camera hoverCamera;
    public float springLength;
    float raycastLengthDraw;
    float raycastLengthPadded;
    float springConstant;
    float springDamping;
    float[] deltaLength;
    float lastPressTime;
    float lastDashTime;
    bool goingRight;
    bool wentRight;

    public float speed;
    public float steerSpeed;
    float steerInput;
    bool isGryoSteering;
    bool2 canDash;
    public PlayerInput Input;
    public float steerSensitivity;
    public float steerDamping;
    float currentRotation;
    public TextMeshProUGUI velocityText;

    private Vector3 hoverCameraOffset;

    private void Awake()
    {
        // variables that don't need to be repeatedly set
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        deltaLength = new float[springs.Count];
        for (int i = 0; i < springs.Count; i++) { deltaLength[i] = springLength; }
        raycastLengthPadded = springLength + 0.25f;
        raycastLengthDraw = springLength+0.1f;
        springDamping = (rb.mass * 2);
        springConstant = (rb.mass * 9.81f);
        steerSpeed*=rb.mass;
        steerDamping *= rb.mass;
        isGryoSteering = false;
        canDash = new bool2(false, false);
        hoverCameraOffset = hoverCamera.transform.position - transform.position;
        lastPressTime = 0.200000000001f;
        goingRight = false;
        wentRight = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.centerOfMass = HovercarCenterOfMass.transform.localPosition;
        rb.linearVelocity = new Vector3(0, 0, speed);
        //OnEnableGyro();
        //currentRotation = UnityEngine.InputSystem.GravitySensor.current.gravity.y.ReadValue();
    }

    // FixedUpdate is called at fixed intervals during unity's physics update loop
    void FixedUpdate()
    {
        rb.AddForce(steerInput, 0, (rb.mass * (speed + 2.041f)));
        if (Input.actions.FindAction("Steering(Keyboard)").ReadValue<float>() == 0 && Input.actions.FindAction("Steering (Mobile-Gyro)").ReadValue<float>() == 0)
        {
            //print(true);
            rb.AddForce(-rb.linearVelocity.x*steerDamping, 0, (rb.mass * (speed + 2.041f)));
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.025f);

        for (int i = 0; i < springs.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(springs[i].transform.position, transform.TransformDirection(Vector3.down), out hit, raycastLengthDraw))
            {
                // Spring Force = (spring rate(mass in newtons) * current spring length) - (spring speed ((current spring length - spring length of previous interval) / refresh rate) * damping (mass doubled))
                rb.AddForceAtPosition(((((transform.up * (springConstant) *Mathf.Clamp(((raycastLengthPadded) - hit.distance),0,springLength)) - (transform.up * (((Mathf.Clamp((hit.distance-deltaLength[i]), -springLength, 0)/Time.fixedDeltaTime) * (springDamping))))))), springs[i].transform.position);
                deltaLength[i] = hit.distance;
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        hoverCamera.transform.position = transform.position + hoverCameraOffset;
    }

    public void OnSteerButton(InputAction.CallbackContext context)
    { 
        goingRight = Input.actions.FindAction("Steering(Keyboard)").ReadValue<float>() == 1;
        float timeSinceLastTap = Time.time - lastPressTime;
        float timeSinceLastDash = Time.time - lastDashTime;
        print(timeSinceLastTap);

        if (timeSinceLastTap <= 0.2f && wentRight == goingRight && timeSinceLastDash >= 1)
        {
            print(wentRight == goingRight);
            print(timeSinceLastDash);
            print("Dash");
            OnSideDash(Input.actions.FindAction("Steering(Keyboard)").ReadValue<float>());
            lastDashTime = Time.time;
            steerInput = (steerSpeed * Input.actions.FindAction("Steering(Keyboard)").ReadValue<float>());
        }else if (!context.performed) {
            return;
        }
        else
        {
            steerInput = (steerSpeed * Input.actions.FindAction("Steering(Keyboard)").ReadValue<float>());
        }
            lastPressTime = Time.time;
        wentRight = Input.actions.FindAction("Steering(Keyboard)").ReadValue<float>() == 1;


    }

    public void OnSteerAnalogue()
    {
        steerInput = (-1* steerSpeed* (Mathf.Clamp(UnityEngine.InputSystem.GravitySensor.current.gravity.y.ReadValue() - currentRotation, -8,8)/4)); //Adding the velocity to go left or right
    }

    private void OnEnableGyro()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.GravitySensor.current);
        isGryoSteering = true;
    }

    void OnSideDash(float input)
    {

        rb.AddForce(transform.right*50*rb.mass*input, ForceMode.Impulse);
    }

    private void OnDisableGyro()
    {
        InputSystem.DisableDevice(UnityEngine.InputSystem.GravitySensor.current);
    }

    
}
