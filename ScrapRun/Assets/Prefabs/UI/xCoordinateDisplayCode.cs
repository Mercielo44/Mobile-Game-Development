using System.IO;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class xCoordinateDisplayCode : MonoBehaviour
{
    public GameObject Player;
    public TextMeshProUGUI coordinateText;
    public TextMeshProUGUI rotationText;
    float currentRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentRotation = UnityEngine.InputSystem.GravitySensor.current.gravity.y.ReadValue();
    }

    // Update is called once per frame
    void Update()
    {
        //coordinateText.text = Player.transform.position.x.ToString();
        rotationText.text = (UnityEngine.InputSystem.GravitySensor.current.gravity.y.ReadValue()-currentRotation).ToString();
    }
}
