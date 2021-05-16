using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
class DeathZones : MonoBehaviour
{
    public AudioClip deathClip;
    public bool alreadyPlayedOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        Debug.Log("deathzones active");

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // WHEN DETECTS PLAYER INSIDE
        if (!alreadyPlayedOnce)
        { 
            GetComponent<AudioSource>().PlayOneShot(deathClip);
            // Animation plays maybe
            // Menu pops up
            alreadyPlayedOnce = true;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
