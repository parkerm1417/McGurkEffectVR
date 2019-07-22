using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SHOULD BE ATTACHED TO EVERY BUTTON
public class ButtonPress : MonoBehaviour
{

    // Start is called before the first frame update, this ensures that the button the script is attached to is colored white initially
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    //When the button is tapped, it turns green
    private void OnTriggerEnter(Collider col)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    //When the button is tapped and the object used to tapped is moved away from the button this is triggered
    private void OnTriggerExit(Collider other)
    {
        //Determines if the experience is entering the audio only section. "if" statement is true if the videos just finished playing
        if (SelectVideo.AudioNum[SelectVideo.counter] == -1 && !SelectVideo.ready2)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;

            //Logs the text attached to the button
            Debug.Log("@" + GetComponentInChildren<TextMesh>().text + "  ");

            //Turns buttons off
            EnableButtons.buttonOn = false;

            //Enables screen that appears in between the video/audio and audio only sections
            SelectVideo.middleScreen.SetActive(true);

            //Changes the sky background from the video player to a nice cloudy sky
            RenderSettings.skybox = Resources.Load<Material>("sky5X2");
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;

            //Logs the text attached to the button
            Debug.Log("@" + GetComponentInChildren<TextMesh>().text + "  ");

            //Tells SelectVideo script to load a new video
            SelectVideo.newVideo = true;

            //Tells EnableButtons script to diable all buttons
            EnableButtons.buttonOn = false;

        }
    }
}
