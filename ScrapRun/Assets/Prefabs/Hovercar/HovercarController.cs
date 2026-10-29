using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

public class HovercarController : MonoBehaviour
{
    public List<GameObject> springs;
    public Rigidbody rb;
    public GameObject prop;
    public GameObject HovercarCenterOfMass;
    public float springLength;
    float raycastLengthDraw;
    float raycastLengthPadded;
    float springConstant;
    float springDamping;
    float[] deltaLength;

    private void Awake()
    {
        // variables that don't need to be repeatedly set
        deltaLength = new float[springs.Count];
        for (int i = 0; i < springs.Count; i++) { deltaLength[i] = springLength; }
        raycastLengthPadded = springLength + 0.25f;
        raycastLengthDraw = springLength+0.1f;
        springDamping = (rb.mass * 2);
        springConstant = (rb.mass * 9.81f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.centerOfMass = HovercarCenterOfMass.transform.localPosition;

    }

    // FixedUpdate is called at fixed intervals during unity's physics update loop
    void FixedUpdate()
    {
        rb.AddForce(transform.forward*(rb.mass*(100+2.041f)));
        print(rb.linearVelocity.z);
        for (int i = 0; i < springs.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(springs[i].transform.position, transform.TransformDirection(Vector3.down), out hit, raycastLengthDraw))
            {
                // Spring Force = (spring rate(mass in newtons) * current spring length) - (spring speed ((current spring length - spring length of previous interval) / refresh rate) * damping (mass doubled))
                rb.AddForceAtPosition(((((transform.up * (springConstant) *Mathf.Clamp(((raycastLengthPadded) - hit.distance),0,springLength)) - (transform.up * ((((hit.distance-deltaLength[i])/Time.fixedDeltaTime) * (springDamping))))))), springs[i].transform.position);
                deltaLength[i] = hit.distance;
            }
        }
    }
}
