using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject diamondPrefab;
    private Renderer platformRenderer;

    void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        CameraColor.OnColorChanged += UpdateColor;
    }

    void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (CameraColor.currentPlatformColor != default)
        {
            platformRenderer.material.color = CameraColor.currentPlatformColor;
        }

        if (DiamondPool.instance == null) return; // ← si el pool no existe todavía, no spawneamos diamante

        int randDiamond = Random.Range(0, 5);
        if (randDiamond < 1)
        {
            Vector3 diamondPos = transform.position;
            diamondPos.y += 2;
            GameObject diamondInstance = DiamondPool.instance.GetDiamond(diamondPos, diamondPrefab.transform.rotation);
            diamondInstance.transform.SetParent(transform);
        }
    }

    void Start() { }

    void UpdateColor(Color newColor)
    {
        if (platformRenderer != null)
            platformRenderer.material.color = newColor;
    }

    void OnDestroy()
    {
        CameraColor.OnColorChanged -= UpdateColor;
        CancelInvoke(); // ← cancela todos los Invoke pendientes
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Fall", 0.4f);
        }
    }

    void Fall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Invoke("ReturnToPool", 2f);
    }

    void ReturnToPool()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Diamond"))
            {
                child.SetParent(null);
                DiamondPool.instance.ReturnDiamond(child.gameObject);
            }
        }

        PlatformPool.instance.ReturnPlatform(gameObject);
    }
}