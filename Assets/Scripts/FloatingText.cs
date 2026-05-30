using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;

    private TMP_Text tmp;  // ← TMP_Text funciona para ambas variantes
    private Color startColor;
    private float elapsed = 0f;

    void Awake()
    {
        tmp = GetComponentInChildren<TMP_Text>();
        // Oscurece el color un 30%
        Color darkerColor = tmp.color * 0.7f;
        darkerColor.a = 1f; // mantiene el alpha al 100%
        tmp.color = darkerColor;
        startColor = tmp.color;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (elapsed >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}