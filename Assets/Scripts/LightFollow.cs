using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public Transform target;
    // Offset in world space to keep the light ahead and above the player
    public Vector3 offset = new Vector3(3f, 10f, 3f);

    void Update()
    {
        // Follow the target maintaining a fixed world space offset
        transform.position = target.position + offset;
    }
}