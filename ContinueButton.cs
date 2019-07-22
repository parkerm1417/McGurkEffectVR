using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    //When a collider enters the continue button, a new video is loaded and
    //the parent of the button is turned off. In this case it is a text screen.
    private void OnTriggerEnter(Collider col)
    {
        SelectVideo.ready2 = true;
        SelectVideo.newVideo = true;
        this.transform.parent.gameObject.SetActive(false);
    }
}
