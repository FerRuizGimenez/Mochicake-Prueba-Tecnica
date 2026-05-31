using System.Collections;
using UnityEngine;

// Ensures this script initializes before Platform scripts that depend on currentPlatformColor
[DefaultExecutionOrder(-1)]
public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    // Stores the current platform color so newly spawned platforms can match it
    public static Color currentPlatformColor;

    public Color[] colors;

    // Event fired when the platform color changes, platforms subscribe to this
    public static event System.Action<Color> OnColorChanged;

    private Coroutine colorCoroutine;

    void Awake()
    {
        instance = this;
        // Reset color on scene load to avoid carrying over color from previous session
        currentPlatformColor = default;
    }

    // Start the color change cycle, called when the game starts
    public void StartColorChange()
    {
        colorCoroutine = StartCoroutine(ColorChanger());
    }

    // Stop the color change cycle, called on game over
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

            // Pick a random color from the array and notify all subscribers
            Color targetColor = colors[Random.Range(0, colors.Length)];
            currentPlatformColor = targetColor;
            OnColorChanged?.Invoke(targetColor);
            GameManager.instance.PlaySound(4, 0.05f);
        }
    }
}