using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Spawning : MonoBehaviour
{

    //introduce all ingredients
    public GameObject[] ingredients;
    public GameObject[] badIngredients;
    private GameObject food;
    private string[] ingredientNames;
    private OrderManager orderManager;

    //variables for spawning food
    public float spawnRate = 1f; //change the speed that food spawns
    private float lastSpawned; //used for creating the delay in spawning the food
    public float graceChance = 50f; //chance for spawning food item that player currently need (out of 100)

    
    void Start()
    {
        ingredientNames = new string[ingredients.Length];
        orderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>(); //initialize ordermanager to be referenced
        for (int i=0; i<ingredients.Length; i++) //fill array full of strings of ingredient names
        {
            ingredientNames[i] = ingredients[i].name;
        }
        spawnRate = DifficultyStatic.foodSpawnRate;
    }
    //spawns food at Random X between -10 and 10, 30 Y, and -25 Z
    void Spawn(GameObject prefab)
    {
        Instantiate(prefab, new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 30.0f, -25.0f), Quaternion.identity);
    }

    //same method as spawn but used to spawn bad food (work around to spawn bad food above normal food)
    void BadSpawn(GameObject prefab)
    {
        Instantiate(prefab, new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 32.0f, -25.0f), Quaternion.Euler(0, 90, 0));
    }

    //destroys food on collision with the DestroyPlane
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }


    void Update()
    {
        //randomly selecting which ingredient to spawn
        var randomNumber = UnityEngine.Random.Range(0, ingredients.Length);
        var randomNumber2 = UnityEngine.Random.Range(1, 100);
        var randomNumber3 = UnityEngine.Random.Range(0, badIngredients.Length);
        
        //process for creating the delay in spawning the food
        if (Time.time > lastSpawned + spawnRate)
        {
            Spawn(ingredients[randomNumber]);

            //roll chance for spawning food item that you need
            int index = Array.IndexOf(ingredientNames, orderManager.GetNextExpectedItem());
            if (randomNumber2 < graceChance && index > -1)
                Spawn(ingredients[index]);

            //roll chance for spawning bad item
            if (randomNumber2 < DifficultyStatic.trashChance)
                BadSpawn(badIngredients[randomNumber3]);

            lastSpawned = Time.time;
        }

    }
}