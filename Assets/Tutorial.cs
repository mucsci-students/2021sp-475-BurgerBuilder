using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject[] tutorialTexts;

    public GameObject escKey;

    void Start()
    {
        StartCoroutine("TutorialMain");
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            SceneManager.LoadScene("MainMenu");
        }
            
    }

    public void ActivateEscUI()
    {
        escKey.SetActive(true);
    }

    IEnumerator TutorialMain()
    {
        yield return new WaitForSeconds(0.9f);
        ActivateIndex(1);
        yield return new WaitForSeconds(0.1f);
        ActivateIndex(2);
        yield return new WaitForSeconds(1.8f);
        ActivateIndex(3);  
    }

    public void DeactivateIndex(int i)
    {
        tutorialTexts[i].SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
    }

    public void ActivateIndex(int i)
    {
        tutorialTexts[i].SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.visible = true;
    }
}
