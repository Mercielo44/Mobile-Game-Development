using UnityEngine;


public class FollowBall : MonoBehaviour

{

    public Transform ballTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {


    }


    // Update is called once per frame

    void Update()

    {

        transform.position = new Vector3(ballTransform.position.x, ballTransform.position.y, -10.0f);

    }

}