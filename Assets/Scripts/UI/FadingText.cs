using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    public float fadeTime;

    private TextMesh text;
    
    private Color faded;

    void Start()
    {
        text = GetComponent<TextMesh>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 2, Space.World);
        StartCoroutine(FadeTo(0.0f, fadeTime));
        if(text.color.a <= 0.0f)
            Destroy(gameObject);
    }

    IEnumerator FadeTo(float value, float time)
    {
        // Fade text over "time"
        float alpha = text.color.a;
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime/time)
        {
            Color newColor = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(alpha, value, t));
            text.color = newColor;
            yield return null;
        }
    }
}
