using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;

    private TMP_Text tmp;
    private Color startColor;
    private float elapsed = 0f;

    void Awake()
    {
        // TMP_Text works for both TextMeshPro and TextMeshProUGUI variants
        tmp = GetComponentInChildren<TMP_Text>();

        // Darken the text color by 30% to ensure visibility against light backgrounds
        Color darkerColor = tmp.color * 0.7f;
        darkerColor.a = 1f;
        tmp.color = darkerColor;
        startColor = tmp.color;
    }

    void Update()
    {
        // Always face the camera so the text is readable from any angle
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0);

        elapsed += Time.deltaTime;

        // Move upwards over time
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Gradually fade out the text over the fade duration
        float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // Destroy the object once the fade is complete
        if (elapsed >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}