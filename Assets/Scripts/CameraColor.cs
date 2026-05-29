using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class CameraColor : MonoBehaviour
{
    public static CameraColor instance;
    public static Color currentPlatformColor;
    
    public Color[] colors;
    public float transitionSpeed = 1.5f;

    public static event System.Action<Color> OnColorChanged;

    void Awake()
    {
        instance = this;
        currentPlatformColor = default;
    }

    public void StartColorChange()
    {
        StartCoroutine(ColorChanger());
    }

    IEnumerator ColorChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            Color targetColor = colors[Random.Range(0, colors.Length)];
            currentPlatformColor = targetColor;
            OnColorChanged?.Invoke(targetColor);
            yield return StartCoroutine(TransitionColor(targetColor));
        }
    }

    IEnumerator TransitionColor(Color targetColor)
    {
        Color startColor = Camera.main.backgroundColor;
        
        float h, s, v;
        Color.RGBToHSV(targetColor, out h, out s, out v);
        Color lighterColor = Color.HSVToRGB(h, s * 0.5f, Mathf.Min(v + 0.3f, 1f));
        
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * transitionSpeed;
            Camera.main.backgroundColor = Color.Lerp(startColor, lighterColor, elapsed);
            yield return null;
        }

        Camera.main.backgroundColor = lighterColor;
    }
}