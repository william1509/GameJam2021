using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Tile"))
            transform.parent.SendMessage("Ground", true);
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Tile"))
            transform.parent.SendMessage("Ground", false);
    }
}
