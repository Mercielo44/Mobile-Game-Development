using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D light2D;
    public float baseIntensity = 1f;
    public float flickerAmount = 0.2f;

    void Update()
    {
        light2D.intensity = baseIntensity + Random.Range(0, flickerAmount);
    }
}