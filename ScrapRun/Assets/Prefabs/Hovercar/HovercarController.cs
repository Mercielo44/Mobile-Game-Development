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
    public float springConstant;
    public float springDamping;
    public float springDamingSensitivity;
    float[] deltaLength = new float[4];

    private void Awake()
    {
        for (int i = 0; i < springs.Count; i++) { deltaLength[i] = springLength; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.centerOfMass = HovercarCenterOfMass.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < springs.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(springs[i].transform.position, transform.TransformDirection(Vector3.down), out hit, springLength))
            {
                rb.AddForceAtPosition((Time.deltaTime*(transform.up * Mathf.Lerp(((rb.mass* 9.8066500286389f) /Time.deltaTime)+ (Mathf.Clamp(springLength - hit.distance, 0, springLength)) * springConstant, 0, hit.distance/springLength) + (transform.up * Mathf.Clamp((deltaLength[i] - hit.distance) * springDamingSensitivity, 0, springDamping)))), springs[i].transform.position);
                //rb.AddForceAtPosition((Time.deltaTime * (((transform.TransformDirection(Vector3.up) * (((Mathf.Clamp(springLength - hit.distance, 0, springLength)) * springConstant) + (rb.mass / Time.deltaTime))) + (transform.up * Mathf.Clamp((deltaLength[i] - hit.distance) * springDamingSensitivity, 0, springDamping))))), springs[i].transform.position);
                print(Time.deltaTime*(transform.up * Mathf.Lerp(rb.mass/Time.deltaTime, 0, hit.distance/springLength) + (transform.up * Mathf.Clamp((deltaLength[i] - hit.distance) * springDamingSensitivity, 0, springDamping))));
                deltaLength[i] = springLength - hit.distance;
                //print(hit.distance);
            }
        }
    }
}
