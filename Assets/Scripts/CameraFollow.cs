using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothValue;

    private Vector3 distance;

    void Start()
    {
        // Store the initial offset between the camera and the target
        distance = target.position - transform.position;
    }

    void Update()
    {
        // Only follow the target if the game is running and the player hasn't fallen
        if(target.position.y >= 0 && GameManager.instance.gameStarted)
        {
            Follow();
        }
    }

    void Follow()
    {
        Vector3 currentPos = transform.position;
        // Calculate the desired camera position maintaining the initial offset
        Vector3 targetPos = target.position - distance;

        // Smoothly interpolate towards the target position
        transform.position = Vector3.Lerp(currentPos, targetPos, smoothValue * Time.deltaTime);
    }
}