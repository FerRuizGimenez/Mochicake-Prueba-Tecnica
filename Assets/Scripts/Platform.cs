using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    public GameObject diamondPrefab;
    private Renderer platformRenderer;

    void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        // Subscribe to color change event to update platform color when triggered
        ColorManager.OnColorChanged += UpdateColor;
    }

    void OnEnable()
    {
        // Reset rigidbody state when retrieved from the pool
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Apply the current platform color when spawned
        if (ColorManager.currentPlatformColor != default)
        {
            platformRenderer.material.color = ColorManager.currentPlatformColor;
        }

        // Skip diamond spawn if the pool is not ready yet (e.g. on scene reload)
        if (DiamondPool.instance == null) return;

        // 1 in 5 chance to spawn a diamond on this platform
        int randDiamond = Random.Range(0, 5);
        if (randDiamond < 1)
        {
            Vector3 diamondPos = transform.position;
            diamondPos.y += 2;
            GameObject diamondInstance = DiamondPool.instance.GetDiamond(diamondPos, diamondPrefab.transform.rotation);
            diamondInstance.transform.SetParent(transform);
        }
    }

    // Only start the transition if the platform is active in the scene
    void UpdateColor(Color newColor)
    {
        if (platformRenderer != null && gameObject.activeInHierarchy)
            StartCoroutine(TransitionColor(newColor));
    }

    // Smoothly transition the platform color over 1 second
    IEnumerator TransitionColor(Color targetColor)
    {
        Color startColor = platformRenderer.material.color;
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            platformRenderer.material.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }

        platformRenderer.material.color = targetColor;
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks on destroyed platforms
        ColorManager.OnColorChanged -= UpdateColor;
    }

    // When the player leaves the platform, trigger the fall after a short delay
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Fall", 0.4f);
        }
    }

    // Enable gravity so the platform falls naturally
    void Fall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Invoke("ReturnToPool", 2f);
    }

    void ReturnToPool()
    {
        // Return any diamond children to the pool before returning the platform
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