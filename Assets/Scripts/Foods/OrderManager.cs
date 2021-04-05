using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{ 
    // NOTE! The number here reflects how many ingredients go BETWEEN the
    // top and bottom bun, since the top and bottom bun will ALWAYS show the
    // start and end of an order. 
    //public static Text PrintOrder;
    public static Text ingredientOne;
    public static Text ingredientTwo;
    public static Text ingredientThree;
    public static Text ingredientFour;
    public static List<string> reversedCurrentOrder;
    private int difficulty;

    private int ingredientCount;
    private bool currentOrderExists;
    private string[] ingredients;

    private OrderDetection orderDetector;

    private ScoreManager scoreManager;

    private GhostVisualOrder ghostOrder;

    private displayVisualOrder displayOrder;

    private List<string> currentOrder;
    //private List<string> reversedCurrentOrder;

    private List<string> PrintOrderList;
    

    private float lastTimeOrderChecked;

    private bool checkedInFirstOrder = false;

    void Start()
    {
        orderDetector = GameObject.Find("OrderDetector").GetComponent<OrderDetection>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        ghostOrder = GameObject.Find("GhostOrder").GetComponent<GhostVisualOrder>();
        displayOrder = GameObject.Find("PrintOrder").GetComponent<displayVisualOrder>();
        ingredients = Enum.GetNames(typeof(FoodItem.Food));
        ingredientCount = ingredients.Length;
        currentOrderExists = false;
        currentOrder = new List<string>();
        reversedCurrentOrder = new List<string>();
        difficulty = DifficultyStatic.difficulty;
        //PrintOrder = GameObject.Find("PrintOrder").GetComponent<Text>();
        /*
        ingredientOne = GameObject.Find("ingredientOne").GetComponent<Text>();
        ingredientTwo = GameObject.Find("ingredientTwo").GetComponent<Text>();
        ingredientThree = GameObject.Find("ingredientThree").GetComponent<Text>();
        ingredientFour = GameObject.Find("ingredientFour").GetComponent<Text>();
        */
        Physics.gravity = new Vector3(0.0f, DifficultyStatic.fallingSpeed, 0.0f);
        lastTimeOrderChecked = Time.time + 3.0f;
    } 

    void Update()
    {
        // If theres no order, create one
        if(!currentOrderExists)
        {
            InitializeOrder();
        }

        /*
        
        for (int i = 0; i <= 3; ++i){
            if (i == 0 && reversedCurrentOrder[i] != null){
                ingredientOne.GetComponent<Text>().text = reversedCurrentOrder[i];
                //panIter.MoveNext();
            }
             if (i == 1 && reversedCurrentOrder[i] != null){
                ingredientTwo.GetComponent<Text>().text = reversedCurrentOrder[i];
                //panIter.MoveNext();
            }
             if (i == 2 && reversedCurrentOrder[i] != null){
                ingredientThree.GetComponent<Text>().text = reversedCurrentOrder[i];
                //panIter.MoveNext();
            }
             if (i == 3 && reversedCurrentOrder[i] != null){
                ingredientFour.GetComponent<Text>().text = reversedCurrentOrder[i];
            }
    
        }
        
     */

        // Order comparison
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && Time.time > lastTimeOrderChecked + 1.2f)
        {
            (int, int) orderCorrectness = CompareOrderToPan(orderDetector.GetWhatsOnPan());
            // Item1 = good items
            // Item2 = bad items
            scoreManager.AddScore(orderCorrectness.Item1, orderCorrectness.Item2);
            orderDetector.RemoveAllPanItems();
            orderDetector.ResetOrderDetectorTrigger();
            currentOrderExists = false;
            lastTimeOrderChecked = Time.time;

            if(DifficultyStatic.playfabScoreboard == "NONE" && !checkedInFirstOrder)
            {
                GameObject.Find("Main Camera").GetComponent<Tutorial>().ActivateIndex(4);
            }
            checkedInFirstOrder = true;
        }  
    }

    //print this to screen..all items are added to a list so print the list
    //items stored in current order
    void InitializeOrder()
    {
        // Clear out the current order list
        currentOrder.Clear();
        // All orders will start with a bottom bun
        currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), 1));
        // Grab random ingredients and add to list
        for(int i = 0; i < difficulty; ++i)
        {
            int index = UnityEngine.Random.Range(2, ingredientCount);
            currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), index));
        } 
        // All orders end with a top bun
        currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), 0));
        // Initialize reversed list for printing
        reversedCurrentOrder = new List<string>(currentOrder);
        reversedCurrentOrder.Reverse();
        currentOrderExists = true;

        //Tell ghost order to update
        ghostOrder.UpdateGhostFoods(GetCurrentOrder());
        displayOrder.UpdateVisualFoods(reversedCurrentOrder);
    }

    void LogCurrentOrder()
    {
        string printThis = "here's my order: ";
        foreach(string s in currentOrder)
            printThis += s + ", ";
        Debug.Log(printThis);
    }

    public List<string> GetCurrentOrder()
    {
        return currentOrder;
    }

    // Gets the next item expected
    public string GetNextExpectedItem()
    {
        // Get the current pan
        List<string> currentPan = orderDetector.GetWhatsOnPan();
        // If the sizes are equal, there is no next expected item
        if(currentPan.Count >= currentOrder.Count)
        {
            return "Nothing";
        }
        // If the sizes aren't equal, get the item at the index of the currentPan's count
        else
        {
            int index = currentPan.Count;
            return currentOrder[index];
        }
    }

    // First int is amount of good orders (in order)
    // Second int is amount of bad items (out of order)
    public (int, int) CompareOrderToPan(List<string> pan)
    {
        int goodItems = 0, badItems = 0, goodItemsReversed = 0, badItemsReversed = 0;

        // If this is true, then there are items that are going to be incorrect regardless of comparison
        // We can add the absolute difference to the badItems count.
        if(pan.Count != currentOrder.Count)
        {
            int incorrectCount = Mathf.Abs(pan.Count - currentOrder.Count);
            badItems += incorrectCount;
            badItemsReversed += incorrectCount;
        }

        // This section iterates the pan from the BOTTOM UP, looking for items in the exact order

        List<string>.Enumerator panIter = pan.GetEnumerator();
        List<string>.Enumerator orderIter = currentOrder.GetEnumerator();
        
        
        while(panIter.MoveNext() && orderIter.MoveNext())
        {
            string panItem = panIter.Current;
            string orderItem = orderIter.Current;

            if(panItem == orderItem)
            {
                ++goodItems;
            }
            else
            {
                ++badItems;
            }
        }

        // This section iterates the pan from the TOP DOWN, looking for items in the exact order.
   
        List<string> reversedPan = new List<string>(pan);
        List<string> reversedOrder = new List<string>(currentOrder);
        reversedPan.Reverse();
        reversedOrder.Reverse();

        panIter = reversedPan.GetEnumerator();
        orderIter = reversedOrder.GetEnumerator();

        while(panIter.MoveNext() && orderIter.MoveNext())
        {
            string panItem = panIter.Current;
            string orderItem = orderIter.Current;

            if(panItem == orderItem)
            {
                ++goodItemsReversed;
            }
            else
            {
                ++badItemsReversed;
            }
        }

        // Takes the max of the good and the minumum of the bad items from the reversed and
        // normal lists to get the proper amount of good and bad items.
        int returnedGood = Mathf.Max(goodItems, goodItemsReversed);
        int returnedBad = Mathf.Min(badItems, badItemsReversed);

        Debug.Log("FINAL GOOD: " + returnedGood + " FINAL BAD: " + returnedBad);

        return (returnedGood, returnedBad);
    }
}