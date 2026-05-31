using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject pickupEffect;
    public float moveSpeed;

    private bool isMovingRight = true;
    private bool firstInput = true;
    private bool isDead = false;
    private Coroutine squashCoroutine;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            Move();
            CheckInput();
        }
        if (transform.position.y <= -0.5f && !isDead)
        {
            isDead = true;
            GameManager.instance.GameOver();
        }
    }

    void Move()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    void CheckInput()
    {
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
        GameManager.instance.PlaySound(1, 0.05f);

        if (squashCoroutine != null)
        {
            StopCoroutine(squashCoroutine);
            transform.localScale = originalScale;
        }
        squashCoroutine = StartCoroutine(SquashStretch());

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
        Vector3 squashedScale = new Vector3(originalScale.x, originalScale.y * 0.6f, originalScale.z);

        float elapsed = 0f;
        float duration = 0.1f;

        // Squash
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, squashedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;

        // Stretch back
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = originalScale;
        squashCoroutine = null;
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diamond")
        {
            GameManager.instance.CollectDiamonds(other.transform.position);
            Instantiate(pickupEffect, other.transform.position, pickupEffect.transform.rotation);
            other.gameObject.SetActive(false);
        }
    }
}