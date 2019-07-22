using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    //When the button is tapped the ready variable is set to tru and the parent of the object is disabled 
    //which in turn disabled the button too, parent in this case is the initial text screen
    private void OnTriggerEnter(Collider col)
    {
        SelectVideo.ready = true;
        this.transform.parent.gameObject.SetActive(false);
    }
}
