using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject utopicTileMap;
    private GameObject dystopicTilemap;

    public enum State { UTOPIA, DYSTOPIA }
    private State state_ = State.UTOPIA;

    public State GetState() { return state_; }

    public enum Ability { NONE, SWITCH, DOUBLE_JUMP, GOAL }


    public bool canSwitch_ = false;
    public void SetCanSwitch(bool canSwitch) { canSwitch_ = canSwitch; }
    public bool GetCanSwitch() { return canSwitch_; }


    public int maxJumps_ = 1;
    public void SetMaxJumps(int maxJumps) { maxJumps_ = maxJumps; }
    public int GetMaxJumps() { return maxJumps_; }


    private bool isPaused_ = false;



    List<string> scenes = new List<string>()
    {
        "MainMenu",
        "Level-1",
        "Level-2",
        "Level-3"
    };
    public int sceneIndex = 0;



    // Sounds
    AudioSource audioSource;
    static string jumpSound = "Sounds/Jump";
    static string deathSound = "Sounds/Death";
    static string switchSound = "Sounds/Timeshift";


    // Music
    static string utopianMusic = "Music/UtopianMusic";
    static string distopianMusic = "Music/DistopianMusic";
    private MusicManager musicManager;

    void Start()
    {
        if (sceneIndex > 0)
            InitMusic();

        audioSource = gameObject.AddComponent<AudioSource>();

        utopicTileMap = GameObject.Find("UtopicTilemap");
        dystopicTilemap = GameObject.Find("DystopicTilemap");

        if (utopicTileMap != null)
            utopicTileMap.SetActive(true);
        if (dystopicTilemap != null)
            dystopicTilemap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void PlaySound(string path)
    {
        audioSource.clip = Resources.Load(path) as AudioClip;
        audioSource.Play();
    }



    private GameObject GetCanvas()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas")) as GameObject;
            canvas.name = "Canvas";
        }
        return canvas;
    }



    private GameObject GetPauseMenu()
    {
        GameObject pauseMenu = GameObject.Find("PauseMenu");
        if (pauseMenu == null)
        {
            pauseMenu = Instantiate(Resources.Load<GameObject>("Prefabs/PauseMenu")) as GameObject;
            pauseMenu.name = "PauseMenu";
            pauseMenu.transform.SetParent(GetCanvas().transform);
            pauseMenu.transform.position = GetCanvas().GetComponent<RectTransform>().sizeDelta / 2;
        }
        return pauseMenu;
    }



    public void ShowControls()
    {

    }

    public void TogglePause()
    {
        isPaused_ = !isPaused_;

        if (isPaused_)
            GetPauseMenu().GetComponent<PauseMenu>().Open();
        else
            GetPauseMenu().GetComponent<PauseMenu>().Close();
    }

    public bool GetIsPaused() { return isPaused_; }



    private void LoadScene(int index)
    {
        sceneIndex = index;
        SceneManager.LoadScene(scenes[sceneIndex]);

        utopicTileMap = GameObject.Find("UtopicTilemap");
        dystopicTilemap = GameObject.Find("DystopicTilemap");

        if (utopicTileMap != null)
            utopicTileMap.SetActive(false);
        if (dystopicTilemap != null)
            dystopicTilemap.SetActive(false);
    }

    public void GoToMainMenu() { LoadScene(0); }

    public void RestartLevel()
    {
        LoadScene(sceneIndex);
    }

    public void NextLevel()
    {
        if (sceneIndex == 2)
            LoadScene(0);
        else
            LoadScene(sceneIndex + 1);
    }



    public void Die()
    {
        PlaySound(deathSound);
        RestartLevel();
    }

    public void Jump()
    {
        PlaySound(jumpSound);
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

        PlaySound(switchSound);

        // Change the music
        musicManager.ToggleMusic();
    }
}
