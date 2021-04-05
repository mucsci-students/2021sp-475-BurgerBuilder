using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayVisualOrder : MonoBehaviour
{

    public Transform rowParent;
    public GameObject rowPrefab;

    public Sprite topBunPicture;
    public Sprite bottomBunPicture;
    public Sprite pattyPicture;
    public Sprite cheesePicture;
    public Sprite lettucePicture;

    private float topBunDesiredHeight = 100;
    private float bottomBunDesiredHeight = 55;
    private float cheeseLettuceDesiredHeight = 40;
    private float pattyDesiredHeight = 48;
   
    public void UpdateVisualFoods(List<string> order)
    {
        foreach(Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }

        foreach(string food in order)
        {
            GameObject newFoodRow = Instantiate(rowPrefab, rowParent);
            Text foodText = newFoodRow.GetComponent<Text>();
            Image foodImage = newFoodRow.transform.Find("FoodImage").GetComponent<Image>();

            foodText.text = food;
            foodImage.sprite = DetermineFoodSprite(food);
            foodImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(foodImage.gameObject.GetComponent<RectTransform>().rect.width, DetermineOptimalSpriteHeight(food));
        }
    }

    public Sprite DetermineFoodSprite(string food)
    {
        switch (food)
        {
            case "BottomBun":
                return bottomBunPicture;
            case "TopBun":
                return topBunPicture;
            case "Cheese":
                return cheesePicture;
            case "Lettuce":
                return lettucePicture;
            case "Patty":
                return pattyPicture;
            default:
                return bottomBunPicture;
        }   
    }

    public float DetermineOptimalSpriteHeight(string food)
    {
        switch (food)
        {
            case "BottomBun":
                return bottomBunDesiredHeight;
            case "TopBun":
                return topBunDesiredHeight;
            case "Cheese":
                return cheeseLettuceDesiredHeight;
            case "Lettuce":
                return cheeseLettuceDesiredHeight;
            case "Patty":
                return pattyDesiredHeight;
            default:
                return pattyDesiredHeight;
        }  
    }
}
