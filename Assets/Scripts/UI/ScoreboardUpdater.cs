using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class ScoreboardUpdater : MonoBehaviour
{
    // The row prefab that contains two text fields: one for the name and one for the score
    public GameObject rowPrefab;
    // The transform/object that will act as the parent to all the rows. It's also responsible for spacing them out nicely.
    public Transform rowParent;
    public GameObject loadingIcon;

    public void UpdateScoreboard(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            // Create a new row and set it's parent to the row parent object.
            GameObject newRow = Instantiate(rowPrefab, rowParent);
            // Get all the text fields in the new row 
            Text[] texts = newRow.GetComponentsInChildren<Text>();
            // Set them to be the values from the leaderboard
            texts[0].text = item.DisplayName;
            texts[1].text = item.StatValue.ToString();
        }

        loadingIcon.SetActive(false);
    }

    public void EnableLoadingIcon()
    {
        loadingIcon.SetActive(true);
    }

    public void ClearScoreboard()
    {
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }
    }
}
