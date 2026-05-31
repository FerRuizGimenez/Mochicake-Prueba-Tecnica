using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : MonoBehaviour
{
    // When the player leaves the starting platform, trigger the fall with a small delay
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("Fall", 0.4f);       
        }
    }

    // Enable gravity and destroy the platform after 2 seconds
    void Fall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 2f);
    }
}