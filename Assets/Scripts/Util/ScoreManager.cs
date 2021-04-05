using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//call method in score manager but do logic in score UI
public class ScoreManager : MonoBehaviour
{
    public static int currentScore;
    [Range(50, 200)]
    public int goodItemsFactor;
    public float multiplierIncrement = 0.2f;
    [Range(0.001f, 0.01f)]
    public float scoreToTimerRatio = 0.005f;
    
    public GameObject textPopup;
    public GameObject smallTextPopup;

    public AudioClip badScore;
    public AudioClip goodScore;
    public AudioClip greatScore;
    public AudioClip amazingScore;

    private float multiplier = 1.0f;

    private countDown timer;
    
    Text ScoreZero;

    void Start()
    {
        currentScore = 0;
        timer = GameObject.Find("Main Camera").GetComponent<countDown>();
        ScoreZero = GameObject.Find("ScoreZero").GetComponent<Text>();
    }
    
    // "correctItems" is defined as the food items in the correct sequence 
    // "badItems" is defined as the amount of bad food items in the given burger
    public void AddScore(int correctItems, int badItems)
    {
        // If there are no bad items, then increment the multiplier by the increment amount and display it
        if(badItems == 0)
        {
            multiplier += multiplierIncrement;
            DisplayMultiplier(multiplier);
        }
        // There was a bad item, which means the multiplier must be reset
        else
        {
            multiplier = 1.0f;
        }

        int calculatedScore = 0;
        calculatedScore = Mathf.CeilToInt((((correctItems * goodItemsFactor) + (correctItems + badItems)) / (1 + (badItems/5))) * DifficultyStatic.difficulty * multiplier);

        // If there was no correct items, then just make it zero.
        if(correctItems == 0)
        {
            calculatedScore = 0;
        }

        currentScore += calculatedScore;

        // In order to add time, the player must have a certain number of ingredients on the pan
        // So for difficulty=2, the player needs more than 1, difficulty=3, player needs more than 2, etc...
        if(correctItems > DifficultyStatic.difficulty - 1)
        {
            timer.AddTime(Mathf.Clamp((calculatedScore * scoreToTimerRatio), 0.0f, 10.0f));
            DisplayAddedTime(Mathf.Clamp((calculatedScore * scoreToTimerRatio), 0.0f, 10.0f));
        }
            
        
        DisplayAddedScore(calculatedScore, correctItems, badItems);  
        ScoreZero.GetComponent<Text>().text = currentScore + "";
    }

    private void DisplayAddedTime(float time)
    {
        time = Mathf.Round(time);
        Vector3 spawnLocation = GameObject.Find("PlateHand").transform.position;
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y + 3.1f, spawnLocation.z);
        GameObject text = Instantiate(smallTextPopup, spawnLocation, Quaternion.identity);
        TextMesh mesh = text.GetComponent<TextMesh>();
        mesh.text = "+" + time.ToString() + "s";
    }

    private void DisplayMultiplier(float currentMultiplier)
    {
        Vector3 spawnLocation = GameObject.Find("PlateHand").transform.position;
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y + 1.5f, spawnLocation.z);
        GameObject text = Instantiate(smallTextPopup, spawnLocation, Quaternion.identity);
        TextMesh mesh = text.GetComponent<TextMesh>();
        mesh.color = new Color(1f, 0.7f, 0.0f);
        mesh.text = "x" + currentMultiplier.ToString();
    }

    private void DisplayAddedScore(int score, int good, int bad)
    {
        // Determine spawn location for text and instantiate
        Vector3 spawnLocation = GameObject.Find("PlateHand").transform.position;
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        GameObject text = Instantiate(textPopup, spawnLocation, Quaternion.identity);

        // Emit particle relative to amount of points earned, clamp it so not too many are emitted
        text.GetComponent<ParticleSystem>().Emit(Mathf.Clamp((good * 20), 0, 200));

        // Figure out which audio clip to play based on the score and how many bad items are there
        text.GetComponent<AudioSource>().clip = DetermineSoundToPlay(bad, score);
        text.GetComponent<AudioSource>().Play();

        // Determine color of text based on amount of bad items
        float redValue = 0.0f;
        float greenValue = 1.0f;
        TextMesh mesh = text.GetComponent<TextMesh>();
        // Each bad item increases the red by a factor of .25 and decreased green by a factor of .10
        for(int i = 0; i < bad; ++i)
        {
            redValue += 0.25f;
            greenValue -= 0.15f;
            mesh.color = new Color(redValue, greenValue, mesh.color.b, 1.0f);
        }

        // Append the text with the score
        mesh.text = "+" + score.ToString();
    }

    private AudioClip DetermineSoundToPlay(int bad, int score)
    {
        // Worst possible scores
        if(score <= 20)
        {
            return badScore;
        }
        // Perfect order
        if(bad == 0)
        {
            return amazingScore;
        }
        // Between 1 and 3 bad items
        else if(bad > 0 && bad < 4)
        {
            return greatScore;
        }
        // 4 or more bad items
        else
        {
            return goodScore;
        }
    }
}
