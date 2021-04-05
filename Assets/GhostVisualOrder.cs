using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GhostVisualOrder : MonoBehaviour
{
    public GameObject[] foods;

    void Start()
    {

    }

    public void UpdateGhostFoods(List<string> order)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float yOffset = 0.0f;

        foreach(string s in order)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            GameObject ghost = Instantiate(foods[FoodIndexOf(s)], spawnPosition, Quaternion.identity);
            yOffset += ghost.transform.localScale.y * 1.8f;
            ghost.transform.parent = transform;
        }
    }

    public int FoodIndexOf(string food)
    {
        switch (food)
        {
            case "BottomBun":
                return 0;
            case "TopBun":
                return 1;
            case "Cheese":
                return 2;
            case "Lettuce":
                return 3;
            case "Patty":
                return 4;
            default:
                return 0;
        }
            
    }
}
