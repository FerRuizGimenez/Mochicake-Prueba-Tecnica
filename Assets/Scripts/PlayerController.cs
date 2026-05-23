using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    bool isMovingRight = true;
    bool firstInput = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            Move();
            CheckInput();
        }
    }

    void Move()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    void CheckInput()
    {
        //if first input then ignore
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
        if (isMovingRight)
        {
            isMovingRight = false;
            transform.rotation = Quaternion.Euler(0,-90,0);
        }
        else
        {
            isMovingRight = true;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }
}
