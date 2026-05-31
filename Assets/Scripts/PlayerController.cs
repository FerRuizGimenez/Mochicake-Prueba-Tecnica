using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject pickupEffect;
    public float moveSpeed;

    private bool isMovingRight = true;
    private bool firstInput = true; // Ignores the first input to prevent immediate direction change on game start
    private bool isDead = false;
    private Coroutine squashCoroutine;
    private Vector3 originalScale;

    void Start()
    {
        // Store the original scale to use as reference for squash/stretch animation
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            Move();
            CheckInput();
        }

        // Trigger game over when player falls below the threshold
        if (transform.position.y <= -0.5f && !isDead)
        {
            isDead = true;
            GameManager.instance.GameOver();
        }
    }

    void Move()
    {
        // Move the player along its right axis at a constant speed
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    void CheckInput()
    {
        // Skip the first input to avoid changing direction immediately when the game starts
        if (firstInput)
        {
            firstInput = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        GameManager.instance.PlaySound(1, 0.03f);

        // Cancel any ongoing squash animation before starting a new one
        if (squashCoroutine != null)
        {
            StopCoroutine(squashCoroutine);
            transform.localScale = originalScale;
        }
        squashCoroutine = StartCoroutine(SquashStretch());

        // Toggle direction between right (X axis) and forward (Z axis)
        if (isMovingRight)
        {
            isMovingRight = false;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            isMovingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator SquashStretch()
    {
        // Flatten the Y axis while keeping X and Z unchanged
        Vector3 squashedScale = new Vector3(originalScale.x, originalScale.y * 0.6f, originalScale.z);

        float elapsed = 0f;
        float duration = 0.1f;

        // Squash: lerp from original scale to squashed scale
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, squashedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;

        // Stretch back: lerp from squashed scale back to original scale
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = originalScale;
        squashCoroutine = null;
    }

    // Called by GameManager to gradually increase difficulty over time
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diamond")
        {
            GameManager.instance.CollectDiamonds(other.transform.position);

            // Spawn pickup effect slightly above the diamond position
            Vector3 effectPos = other.transform.position;
            effectPos.y += 1.5f;
            Instantiate(pickupEffect, effectPos, pickupEffect.transform.rotation);

            other.gameObject.SetActive(false);
        }
    }
}