using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject utopicTileMap;
    public GameObject dystopicTilemap;
    void Start()
    {
        dystopicTilemap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWorlds() {
        dystopicTilemap.SetActive(!dystopicTilemap.activeSelf);
        utopicTileMap.SetActive(!utopicTileMap.activeSelf);
    }
}
