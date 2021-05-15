using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    public PlayerController.Side side;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        transform.parent.SendMessage("WallAt", side);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        transform.parent.SendMessage("NoWallAt", side);
    }
}
