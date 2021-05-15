using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomColliderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.sharedMaterial = Resources.Load("GroundMaterial") as PhysicsMaterial2D;
        Debug.Log(other.sharedMaterial);
        transform.parent.SendMessage("SetJumpStatus", true);
        transform.parent.SendMessage("ReactivateControls", true);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        transform.parent.SendMessage("SetJumpStatus", false);
    }

}
