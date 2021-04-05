using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class countDown : MonoBehaviour
{
    
    public float timeStart = 60;
    public Text textBox;

    private AudioSource timerTick;
    private float timerTickInterval = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        textBox.text = timeStart.ToString();
        timerTick = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeStart <= 5f && timerTickInterval <= 0.0f)
        {
            timerTick.Play();
            timerTickInterval = 1.0f;
        }

        textBox.text = timeStart.ToString();
        timeStart -= Time.deltaTime;
        timerTickInterval -= Time.deltaTime;
        textBox.text = Mathf.Round(timeStart).ToString();
        if(timeStart <= 0){
            timeStart = 0;
            SceneManager.LoadScene("gameOver");
        }
    }

    public void AddTime(float timeToAdd)
    {
        timeStart += timeToAdd;
        textBox.color = Color.green;
        Invoke("TurnWhite", 1.0f);
    }

    private void TurnWhite()
    {
        textBox.color = Color.white;
    }
}
