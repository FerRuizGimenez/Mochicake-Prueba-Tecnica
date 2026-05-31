using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    public static Color currentPlatformColor;

    public Color[] colors;

    public static event System.Action<Color> OnColorChanged;

    private Coroutine colorCoroutine;

    void Awake()
    {
        instance = this;
        currentPlatformColor = default;
    }

    public void StartColorChange()
    {
        colorCoroutine = StartCoroutine(ColorChanger());
    }

    public void StopColorChange()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
            colorCoroutine = null;
        }
    }

    IEnumerator ColorChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            Color targetColor = colors[Random.Range(0, colors.Length)];
            currentPlatformColor = targetColor;
            OnColorChanged?.Invoke(targetColor);
            GameManager.instance.PlaySound(4, 0.05f);
        }
    }
}