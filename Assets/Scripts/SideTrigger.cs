using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    public PlayerController.Side side;
    public PlayerController.Type type;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Tile"))
        {
            if (type == PlayerController.Type.HAND)
                transform.parent.SendMessage("WallAtHand", side);
            else if (type == PlayerController.Type.FOOT)
                transform.parent.SendMessage("WallAtFoot", side);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Tile"))
        {
            if (type == PlayerController.Type.HAND)
                transform.parent.SendMessage("NoWallAtHand", side);
            else if (type == PlayerController.Type.FOOT)
                transform.parent.SendMessage("NoWallAtFoot", side);
        }
    }
}
