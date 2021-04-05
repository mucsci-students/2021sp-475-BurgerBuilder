using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabErrorText : MonoBehaviour
{
    private float lifetime = 5.0f;
    private float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
