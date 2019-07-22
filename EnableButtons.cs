using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButtons : MonoBehaviour
{
    public static bool buttonOn;

    // Start is called before the first frame update, this sets the variable to ensure all buttons are turned off at the beginning of the experience
    void Start()
    {
        buttonOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if buttons should be on
        if(buttonOn)
        {
            //If the buttons should be on then they are turned on
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            //If they are not supposed to be on then they are turned off
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
