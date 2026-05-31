using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    public GameObject diamondPrefab;
    private Renderer platformRenderer;

    void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        ColorManager.OnColorChanged += UpdateColor;
    }

    void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (ColorManager.currentPlatformColor != default)
        {
            platformRenderer.material.color = ColorManager.currentPlatformColor;
        }

        if (DiamondPool.instance == null) return;

        int randDiamond = Random.Range(0, 5);
        if (randDiamond < 1)
        {
            Vector3 diamondPos = transform.position;
            diamondPos.y += 2;
            GameObject diamondInstance = DiamondPool.instance.GetDiamond(diamondPos, diamondPrefab.transform.rotation);
            diamondInstance.transform.SetParent(transform);
        }
    }

    void UpdateColor(Color newColor)
    {
        if (platformRenderer != null && gameObject.activeInHierarchy)
            StartCoroutine(TransitionColor(newColor));
    }

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
        ColorManager.OnColorChanged -= UpdateColor;
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