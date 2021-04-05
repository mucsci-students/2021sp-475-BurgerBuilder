using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameValidater : MonoBehaviour
{
    public Button submit;

    private InputField input;

    void Start()
    {
        input = GetComponent<InputField>();
    }

    void Update()
    {
        if(input.text == "")
        {
            submit.interactable = false;
        }
        else
        {
            submit.interactable = true;
        }
    }
}
