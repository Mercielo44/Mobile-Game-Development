using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HovercarController : MonoBehaviour
{
    public List<GameObject> springs;
    public Rigidbody rb;
    public GameObject prop;
    public GameObject HovercarCenterOfMass;
    public float springLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.centerOfMass = HovercarCenterOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject spring in springs)
        {
            RaycastHit hit;
            if (Physics.Raycast(spring.transform.position, transform.TransformDirection(Vector3.down), out hit, springLength))
            {
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up)*Mathf.Pow(springLength- hit.distance, 2)/3f*250f, spring.transform.position);
            }
        }
    }
}
