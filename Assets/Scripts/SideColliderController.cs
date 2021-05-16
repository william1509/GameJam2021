using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideColliderController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 jumpVector;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        WallJump wj = new WallJump(true, jumpVector);
        transform.parent.SendMessage("SetWallJumpStatus", wj);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        WallJump wj = new WallJump(false, jumpVector);
        transform.parent.SendMessage("SetWallJumpStatus", wj);
    }
}
