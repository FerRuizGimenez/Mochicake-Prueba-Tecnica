using System.Collections;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public Transform lastPlatform;
    Vector3 lastPosition;
    Vector3 newPos;
    Quaternion newRot;
    bool lastWasRotated;

    void Start()
    {
        lastPosition = lastPlatform.position;
        StartCoroutine(SpawnPlatforms());
    }

    IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            GeneratePosition();
            PlatformPool.instance.GetPlatform(newPos, newRot);
            lastPosition = newPos;

            yield return new WaitForSeconds(0.25f);
        }
    }

    void GeneratePosition()
    {
        newPos = lastPosition;

        int rand = Random.Range(0, 2);

        if (rand > 0)
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
        else
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