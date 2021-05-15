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



    void Start()
    {
        dystopicTilemap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState()
    {
        if (state_ == State.DYSTOPIA)
            state_ = State.UTOPIA;
        else if (state_ == State.UTOPIA)
            state_ = State.DYSTOPIA;

        dystopicTilemap.SetActive(state_ == State.DYSTOPIA);
        utopicTileMap.SetActive(state_ == State.UTOPIA);
    }
}
