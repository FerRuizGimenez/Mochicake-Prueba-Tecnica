using System.Collections.Generic;
using UnityEngine;

public class DiamondPool : MonoBehaviour
{
    public static DiamondPool instance;

    public GameObject diamondPrefab;
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
            GameObject obj = Instantiate(diamondPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetDiamond(Vector3 position, Quaternion rotation)
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

        GameObject newObj = Instantiate(diamondPrefab, position, rotation);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnDiamond(GameObject obj)
    {
        obj.transform.SetParent(null);
        obj.SetActive(false);
    }
}