using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS TO BE ATTACHED TO THE CAMERA
public class Focus : MonoBehaviour
{
    private bool Started;
    private bool FocusCheck;

    // Start is called before the first frame update, this makes the initial state of the started variable false which correlates with if the FocusTimerStart coroutine has begun
    void Start()
    {
        Started = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if a video is currently playing and if the start button has been pressed at the beginning of the experience
        if(!SelectVideo.VideoPlaying && SelectVideo.ready)
        {
            //Checks if the headset is facing in the correct direction based off of the rotation value. It also checks if the FocusTimerStart coroutine has begun
            if (Mathf.Abs(transform.rotation.y + .7071068f) < .0833333333 && !Started)
            {
                //Starts the coroutine below
                StartCoroutine(FocusTimerStart());
                //Tells the script that the coroutine has started
                Started = true;
                //Tells the computer the participant is focusing
                FocusCheck = true;
            }

            //Checks if the headset is facing the wrong direction and that a coroutine is currently running
            if(Mathf.Abs(transform.rotation.y + .7071068f) > .08333333333 && Started)
            {
                //stops all FocusTimerStart coroutines from running
                StopAllCoroutines();
                //Since coroutine is not running anymore Started is now false
                Started = false;
                //Tells the computer the participant is not focusing
                FocusCheck = false;
            }
        }
    }

    IEnumerator FocusTimerStart()
    {
        //This is a 2 second delay that begins when the participant begins focusing
        yield return new WaitForSecondsRealtime(2f);
        //At the end of the delay, it checks if the participant is still focusing
        if (FocusCheck)
        {
            //Because the participant has focused, the focused variable in the SelectVideo script is set to true
            SelectVideo.focused = true;
            //Because the coroutine is ending, the started variable is set to false
            Started = false;
        }
    }
}
