using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platform;
    public Transform lastPlatform;
    Vector3 lastPosition;
    Vector3 newPos;
    Quaternion newRot;
    bool lastWasRotated;

    void Start()
    {
        lastPosition = lastPlatform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SpawnPlatforms();
        }
    }

    void SpawnPlatforms()
    {
        GeneratePosition();
        Instantiate(platform, newPos, newRot);
        lastPosition = newPos;
    }
    void GeneratePosition()
    {
        newPos = lastPosition;

        int rand = Random.Range(0, 2);

        if (rand > 0) // Avanza en X
        {
            if (lastWasRotated)
            {
                newPos.x += 3f;
                newPos.z += 1f;
            }
            else
            {
                newPos.x += 4f;
            }
            newRot = Quaternion.identity;
            lastWasRotated = false;
        }
        else // Avanza en Z
        {
            if (!lastWasRotated)
            {
                newPos.x += 1f;
                newPos.z += 3f;
            }
            else
            {
                newPos.z += 4f;
            }
            newRot = Quaternion.Euler(0f, 90f, 0f);
            lastWasRotated = true;
        }
    }
}