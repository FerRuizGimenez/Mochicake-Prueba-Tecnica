using System.Collections;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public Transform lastPlatform;

    private Vector3 lastPosition;
    private Vector3 newPos;
    private Quaternion newRot;
    private bool lastWasRotated;

    void Start()
    {
        // Initialize spawn position from the last platform in the scene
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

        // Randomly decide whether the next platform advances in X or Z
        int rand = Random.Range(0, 2);

        if (rand > 0) // Advance in X axis
        {
            if (lastWasRotated)
            {
                // Transition from Z to X: compensate pivot offset between both orientations
                // Offset = half of long side (2) + half of short side (1) = 3 on X
                // and half of short side (1) on Z to align edges
                newPos.x += 3f;
                newPos.z += 1f;
            }
            else
            {
                // Same direction (X to X): move by the full long side length
                newPos.x += 4f;
            }
            newRot = Quaternion.identity;
            lastWasRotated = false;
        }
        else // Advance in Z axis
        {
            if (!lastWasRotated)
            {
                // Transition from X to Z: compensate pivot offset between both orientations
                // Offset = half of short side (1) on X to align edges
                // and half of long side (2) + half of short side (1) = 3 on Z
                newPos.x += 1f;
                newPos.z += 3f;
            }
            else
            {
                // Same direction (Z to Z): move by the full long side length
                newPos.z += 4f;
            }
            newRot = Quaternion.Euler(0f, 90f, 0f);
            lastWasRotated = true;
        }
    }
}