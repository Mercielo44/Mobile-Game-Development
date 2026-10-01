using UnityEngine;

public class ButtonBounce : MonoBehaviour
{
    Vector3 baseScale;
    float t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake() { baseScale = transform.localScale; }
    public void PlayBounce() { t = 5f; }

    // Update is called once per frame
    void Update()
    {
        if (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            float s = 1f + Mathf.Sin((0.2f - t) * 25f) * 0.08f;
            transform.localScale = baseScale * s;
            if (t <= 0f) { transform.localScale = baseScale; }
        }
    }

    
}
