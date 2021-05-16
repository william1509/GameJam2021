using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    static string menuMusic = "Music/MainMenu";
    AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = Resources.Load(menuMusic) as AudioClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
