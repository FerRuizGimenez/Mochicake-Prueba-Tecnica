using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(3f, 10f, 3f);

    void Update()
    {
        transform.position = target.position + offset;
    }
}