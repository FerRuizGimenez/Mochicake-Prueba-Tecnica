using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour
{
    public Color[] colors; 
    void Start()
    {
        StartCoroutine("ColorChanger");
    }
    IEnumerator ColorChanger()
    {
        while (true)
        {
            int randomColor = Random.Range(0,6);
            Camera.main.backgroundColor = colors[randomColor];
            yield return new WaitForSeconds(10);
        }
    }
}
