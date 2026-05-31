using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject pickupEffect;
    public float moveSpeed;
    
    private bool isMovingRight = true;
    private bool firstInput = true;
    private bool isDead = false;

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
        GameManager.instance.PlaySound(1, 0.05f); // ← tap
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