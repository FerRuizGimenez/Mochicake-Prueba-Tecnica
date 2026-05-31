using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    public static PlatformPool instance;
    public GameObject platformPrefab;
    public int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        instance = this;
        InitPool();
    }

    // Pre-instantiate all platforms at startup and keep them inactive
    void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Retrieve an inactive platform from the pool, or create a new one if none are available
    public GameObject GetPlatform(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        // Pool exhausted: instantiate a new platform and add it to the pool for future reuse
        GameObject newObj = Instantiate(platformPrefab, position, rotation);
        pool.Add(newObj);
        return newObj;
    }

    // Return a platform to the pool by deactivating it
    public void ReturnPlatform(GameObject obj)
    {
        obj.SetActive(false);
    }
}