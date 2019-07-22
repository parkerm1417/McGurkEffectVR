using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

//THIS SCRIPT SHOULD BE ATTACHED TO THE VIDEO PLAYER
public class SelectVideo : MonoBehaviour
{
    //Tells if a new video is to be loaded
    public static bool newVideo;
    //This is the VideoPlayer object that the script is attached to
    VideoPlayer player;
    //Equivalent to if the participant is focused
    public static bool focused;
    //Equivalent to if a video is currently playing
    public static bool VideoPlaying;
    //This is the parent object of the audio source. The audio rotates around this object
    public GameObject origin;
    //This is the audio source
    public AudioSource Audio;
    //Equivalent to if the start button has been pressed at the beginning of the experience
    public static bool ready;
    //Equivalent to the number of azimuth angles
    public int Angles;
    //Equivalent to the number of videos
    public int TotalVideos;
    //Equivalent to which index is accessed to determine which video is played and what the audio angle is
    public static int counter;
    //List populated in the order that the videos will be played in the experience
    private List<int> VideoNum;
    //List populated in the order for which angles the audio will be placed at
    public static List<int> AudioNum;
    //List populated by the index values of elevation videos for which the audio angle was greater than the number of elevation angles i.e. 5 azimuth angles are used but only 3 elevations, so 4 and 5 for elevation are invalid
    public List<int> removal;
    //Equivalent to the video number that should be played during the audio only section
    private int AudioOnly;
    //This is the screen at the end that tells participants they are finished and should remove the headset
    public GameObject End;
    //Equivalent to if the continue button has been pressed when the video section ended
    public static bool ready2;
    //This is the screen in the middle that tells participants the next section will be audio only, this variable can be accessed by other scripts
    public static GameObject middleScreen;
    //This is the screen in the middle that tells participants the next section will be audio only, this variable cannot be accessed by other scripts
    public GameObject middle;
    //This array is made up of all the possible azimuth angles
    private int[] PossibleAngles = {90,120,180,240,270};

    // Start is called before the first frame update
    void Start()
    {
        //Sets the audio only section to start with video 1
        AudioOnly = 1;
        //Populates the player variable with the Video Player component
        player = GetComponent<VideoPlayer>();
        //Sets the video player to not loop
        player.isLooping = false;
        counter = 0;
        //Creates a random list of numbers corresponding to video numbers
        VideoNum = VideoSelect(Angles, TotalVideos);
        //Creates a random list of number corresponding to audio angle indexes in PossibleAngles
        AudioNum = AudioSelect(Angles, TotalVideos, VideoNum);
        //Removes all instances of angle numbers that are invalid for the elevation videos
        ListAdjust(VideoNum, AudioNum);
        //Adds an index to the audio list, this helps the program figure out when it is time to start the audio only section
        AudioNum.Add(-1);
        //Loads the first video
        StartCoroutine(LoadVideo());
        //Sets various variables 
        VideoPlaying = false;
        ready = false;
        ready2 = false;
        //Sets the static variable to be equivalent to the nonstatic variable
        middleScreen = middle;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the start button has been pressed at the beginning of the experience
        if (ready)
        {
            //Checks if a new video should be loaded and that the end of the audio section has not been reached
            if (newVideo && (AudioOnly <= 6))
            {
                //Loads the next video
                StartCoroutine(LoadVideo());
                //Prevents another video loading next frame
                newVideo = false;
            }

            //Checks if the participant is focused, this variable is set in the Focus script
            if (focused)
            {
                //Begins playing the clip
                player.Play();
                //Sets variable to notify other scripts a clip is playing
                VideoPlaying = true;
                //Resets variable
                focused = false;
                //Starts coroutine that monitors when the video is done playing
                StartCoroutine(PlayTime());
            }

            //Checks if new video is to be loaded and that the audio only section is finished
            if (newVideo && (AudioOnly > 6))
            {
                //Enables the end screen
                End.SetActive(true);
            }
        }
    }

    //Loads new video
    IEnumerator LoadVideo()
    {
        //Places the audio directly in front of participant, seems unnecessary but if this is not done the audio starts going places it should not
        origin.transform.eulerAngles = new Vector3(0, 0, 0);
        //Stops the player to ensure previous video does not affect playing of new video
        player.Stop();
        //Checks if audio only section has been reached and the continue button has been pressed
        if(AudioNum[counter] == -1 && ready2)
        {
            //Sets the audio distance from the participant
            Audio.transform.position = new Vector3(-2, 0, 0);
            //Ensures audio only section audio is right in front of participant by setting the angle to 0
            origin.transform.eulerAngles = new Vector3(0, 0, 0);
            //Loads a video clip to the video player based on AudioOnly variable
            player.clip = Resources.Load<VideoClip>("Video" + AudioOnly);
            //Pauses video
            player.Pause();
            //Sets the audio source of the video to the audio source in the scene
            player.SetTargetAudioSource(0, Audio);
            //Plays video
            player.Play();
            //Video is now playing so variable must be adjusted
            VideoPlaying = true;
            //Starts coroutine to know when the video ends
            StartCoroutine(PlayTime());
            //Logs relevant information to Debug Log
            Debug.Log("@AO  ");
            Debug.Log("@" + AudioOnly + "   ");
            Debug.Log("@0   ");
            Debug.Log("@0   ");
            //Increments the AudioOnly variable
            AudioOnly++;
        }

        //Checks if audio only section has not been reached
        if(AudioNum[counter] != -1)
        {
            //Adjusts volume of video based on the speaker (Matthew was all even videos, Liam was all odd)
            if (VideoNum[counter] % 2 == 0)
            {
                Audio.volume = 0.2f;
            }
            else
                Audio.volume = 0.35f;

            //Checks if the video is not an elevation video
            if (VideoNum[counter] <= 6)
            {
                //Sets audio distance from participant
                Audio.transform.position = new Vector3(-2, 0, 0);
                //Sets elevation angle
                origin.transform.eulerAngles = new Vector3(0, PossibleAngles[AudioNum[counter]-1], 0);
                //Loads video based on the the VideoNum list
                player.clip = Resources.Load<VideoClip>("Video" + VideoNum[counter]);
                //Plays video
                player.Play();
                //Waits for 0.1 seconds to allow video to load
                yield return new WaitForSecondsRealtime(0.1f);
                //Pauses video
                player.Pause();
                //Sets output audio source for video
                player.SetTargetAudioSource(0, Audio);
                //Resets focused variable back to false 
                focused = false;
                //Resets video playing variable back to false
                VideoPlaying = false;
            }
            else
            {
                //Checks if the video is one of the FAR videos
                if (VideoNum[counter] > 8)
                    //If the videos are far then the audio distance is changed to 4 m
                    Audio.transform.position = new Vector3(-4, 0, 0);
                else
                    //If the video is not far the distance is 2 m
                    Audio.transform.position = new Vector3(-2, 0, 0);

                //From horizontal, rotating downwards is positive so 240 is almost directly above the participant. the code below sets the elevation angle of the sound source
                if (AudioNum[counter] < 3)
                {
                    origin.transform.eulerAngles = new Vector3(0, 0, 240 + 60 * AudioNum[counter]);
                }
                else
                {
                    origin.transform.eulerAngles = new Vector3(0, 0, 60);
                }
                    
                    //Loads relevant video clip
                    player.clip = Resources.Load<VideoClip>("Video" + VideoNum[counter]);
                    //Plays video
                    player.Play();
                    //Delay of 0.1 sec to allow video to load
                    yield return new WaitForSecondsRealtime(0.1f);
                    //Pauses video
                    player.Pause();
                    //Sets audio source for video
                    player.SetTargetAudioSource(0, Audio);
                    focused = false;
                    VideoPlaying = false;
            }

            //Logs necessary information based on what video is currently playing
            if (AudioNum[counter] <= (1 + (Angles / 2)) && VideoNum[counter] <= 6)
            {
                //This is for positive azimuth angles
                Debug.Log("@AV  ");
                Debug.Log("@" + VideoNum[counter] + "   ");
                Debug.Log("@" + PossibleAngles[AudioNum[counter]-1] + "   ");
                Debug.Log("@0   ");
            }
            else
            {
                if (VideoNum[counter] <= 6)
                {
                    //THis is for negative azimuth angles
                    Debug.Log("@AV  ");
                    Debug.Log("@" + VideoNum[counter] + "   ");
                    Debug.Log("@" + (PossibleAngles[AudioNum[counter]-1]-360) + "   ");
                    Debug.Log("@0   ");
                }

                else
                {
                    //This is for elevations
                    Debug.Log("@AV  ");
                    Debug.Log("@" + VideoNum[counter] + "   ");
                    Debug.Log("@0   ");
                    Debug.Log("@"+(120-60*AudioNum[counter])+"   ");
                }
            }
            //Increments counter to proceed to the next index when playing the next video
            counter++;
        }   
    }

    //Waits for length of video before changing variable values
    IEnumerator PlayTime()
    {
        //Waits length of video and accounts for 0.1 sec play during loading time
        yield return new WaitForSecondsRealtime((float)player.length-0.1f);
        //Pauses player
        player.Pause();
        //Turns on the buttons
        EnableButtons.buttonOn = true;
    }

    //Randomization of video order
    public static List<int> VideoSelect(int a, int b)
    {

        int limit = a * b;
        int amount = a;
        int upper_bound = b;
        var unique_random_numbers = new List<int>();
        System.Random rand = new System.Random();

        if (amount > limit)
            limit = amount;

        while (unique_random_numbers.Count < limit)
        {
            int random_number = (int)Mathf.Ceil((float)rand.NextDouble() * upper_bound);
            int c = 0;
            if (unique_random_numbers.IndexOf((int)random_number) == -1)
            {
                unique_random_numbers.Add(random_number);
            }

            else
            {
                foreach (int number in unique_random_numbers)
                    if (random_number == number)
                    {
                        c = c + 1;
                    }
                if (c < amount)
                {
                    unique_random_numbers.Add(random_number);
                }
            }
        }
        return unique_random_numbers;
    }

    //Randomization of audio order
    public static List<int> AudioSelect(int b, int a, List<int> c)
    {

        int limit = a * b;
        int amount = a;
        int upper_bound = b;
        var unique_random_numbers = new List<int>();
        List<int> talker = c;
        System.Random rand = new System.Random();

        if (amount > limit)
            limit = amount;

        while (unique_random_numbers.Count < limit)
        {
            int random_number = (int)Mathf.Ceil((float)rand.NextDouble() * (upper_bound));
            List<int> check = new List<int>();

            if (unique_random_numbers.IndexOf(random_number) == -1)
            {
                unique_random_numbers.Add(random_number);
            }

            else
            {
                for (int i = 0; i < unique_random_numbers.Count; i++)
                {
                    if (random_number == unique_random_numbers[i])
                    {
                        check.Add(talker[i]);
                    }
                }
                if (check.Count < amount)
                {
                    int w = unique_random_numbers.Count;
                    if (check.IndexOf(talker[w]) == -1)
                    {
                        unique_random_numbers.Add(random_number);
                    }
                }
            }
        }
        return unique_random_numbers;
    }

    //Removes invalid indexes, Ex. Elevation videos with angle indexes of 4 or 5 which are valid azimuths but not elevations
    public void ListAdjust(List<int> Video, List<int> Audio)
    {
        for(int i = 0; i < Video.Count; i++)
        {

            if(Video[i] > 6 && Audio[i] > 3)
            {
                removal.Add(i);
            }
        }
        //Sorts the list from least to greatest
        removal.Sort();
        //Reverse list to make it greatest to least, this prevents removal of indexes from impacting other indexes still to be removed
        removal.Reverse();

        //Removes necessary indexes from both the video and audio list
        foreach(int number in removal)
        {
            Video.RemoveAt(number);
            Audio.RemoveAt(number);
        }
    }
}
