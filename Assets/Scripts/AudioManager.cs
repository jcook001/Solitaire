using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioManager : MonoBehaviour
{
    private GameObject mainMenuFunctions;
    private MainMenu mainMenu;
    private AudioSource audioSource;

    public AudioClip[] backgroundMusic;
    private TextMeshPro musicInformationText;
    private int StartTrackIndex = 5; //track 5 Happy Relaxing Piano Loop
    private int currentTrack;
    private float trackDelay = 0.1f; //time in seconds before playing a new track
    public float volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        mainMenuFunctions = GameObject.Find("Main Menu Functions");
        mainMenu = mainMenuFunctions.GetComponent<MainMenu>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic[StartTrackIndex];
        audioSource.loop = true;
        currentTrack = StartTrackIndex;
        audioSource.volume = volume;
        mainMenu.UpdateTrackName(backgroundMusic[currentTrack].name);
    }

    // Update is called once per frame
    void Update()
    {
        //Add some volume controls here?
    }

    public void PreviousTrack()
    {
        ChangeTrack(-1);
    }

    public void NextTrack()
    {
        ChangeTrack(1);
    }

    private void ChangeTrack(int trackChange)
    {
        //check that incrementing the track index doesn't exceed the list values
        int nextTrack = currentTrack + trackChange;
        if (nextTrack >= backgroundMusic.Length)
        {
            currentTrack = 0;
        }
        else if(nextTrack < 0)
        {
            currentTrack = backgroundMusic.Length - 1; //subtract 1 because arrays start counting at 0
        }
        else
        {
            currentTrack = currentTrack + trackChange;
        }

        audioSource.clip = backgroundMusic[currentTrack];
        audioSource.PlayDelayed(trackDelay);
        mainMenu.UpdateTrackName(backgroundMusic[currentTrack].name);
        //Does the volume reset on track change?
    }

    public void PlayPause()
    {
        //Check if the track is playing
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();// might need to be audioSource.Unpause()?
        }

        //Change the icon appropriately
    }
}
