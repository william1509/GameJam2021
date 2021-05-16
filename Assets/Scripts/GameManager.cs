using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject utopicTileMap;
    public GameObject dystopicTilemap;

    public enum State { UTOPIA, DYSTOPIA }
    private State state_ = State.UTOPIA;
    public State getState() { return state_; }

    // Music
    static string utopianMusic = "Music/UtopianMusic";
    static string distopianMusic = "Music/DistopianMusic";
    private MusicManager musicManager;

    void Start()
    {
        InitMusic();
        dystopicTilemap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitMusic()
    {
        // Create MusicManager and AudioSources for simple integration
        musicManager = gameObject.AddComponent<MusicManager>();
        AudioSource utopianSource = gameObject.AddComponent<AudioSource>();
        AudioSource distopianSource = gameObject.AddComponent<AudioSource>();

        // Set AudioClip for each AudioSource
        utopianSource.clip = Resources.Load(utopianMusic) as AudioClip;
        distopianSource.clip = Resources.Load(distopianMusic) as AudioClip;

        // Give AudioSources to MusicManager
        musicManager.SetAudioSources(utopianSource, distopianSource);
    }

    public void SwitchState()
    {
        if (state_ == State.DYSTOPIA)
            state_ = State.UTOPIA;
        else if (state_ == State.UTOPIA)
            state_ = State.DYSTOPIA;

        dystopicTilemap.SetActive(state_ == State.DYSTOPIA);
        utopicTileMap.SetActive(state_ == State.UTOPIA);

        // Change the music
        musicManager.ToggleMusic();
    }
}
