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

    void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

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

        // Si no hay disponibles, crea uno nuevo y lo agrega al pool
        GameObject newObj = Instantiate(platformPrefab, position, rotation);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnPlatform(GameObject obj)
    {
        obj.SetActive(false);
    }
}