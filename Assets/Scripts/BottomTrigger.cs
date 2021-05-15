using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        transform.parent.SendMessage("LandUnder", true);
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        transform.parent.SendMessage("LandUnder", false);
    }
}
