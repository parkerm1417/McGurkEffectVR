using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVideo : MonoBehaviour
{
    public GameObject origin;
    public AudioSource Audio;
    public Text otext;
    public Text atext;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.RawButton.A))
        {
            origin.transform.Rotate(0, 0, .5f);
        }
        if (OVRInput.Get(OVRInput.RawButton.B))
        {
            origin.transform.Rotate(0, 0, -.5f);
        }
        if (OVRInput.Get(OVRInput.RawButton.X))
        {
            Audio.transform.Rotate(0, 0, -.5f);
        }
        if (OVRInput.Get(OVRInput.RawButton.Y))
        {
            Audio.transform.Rotate(0, 0, .5f);
        }
        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.5)
        {
            origin.transform.Rotate(0, -.5f, 0);
        }
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.5)
        {
            origin.transform.Rotate(0, .5f, 0);
        }

        Quaternion o = origin.transform.rotation;
        float x = -o.z;
        Quaternion a = Audio.transform.rotation;
        otext.text = o.x.ToString()+","+ o.y.ToString()+","+x.ToString();
        atext.text = a.x.ToString() + "," + a.y.ToString() + ","+a.z.ToString();
    }
}
