using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject diamondPrefab;
    void Start()
    {
        int randDiamond = Random.Range(0,5);

        Vector3 diamondPos = transform.position;
        diamondPos.y += 2;

        if(randDiamond < 1)
        {
            //spawn diamond
            GameObject diamondInstance = Instantiate(diamondPrefab, diamondPos, diamondPrefab.transform.rotation);
            diamondInstance.transform.SetParent(gameObject.transform);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("Fall", 0.4f);       
        }
    }
    void Fall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 2f);
    }
}
