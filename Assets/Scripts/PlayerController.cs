using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct WallJump
{
    public bool stickingToWall;
    public Vector2 jumpVector;

    public WallJump(bool stickingToWall, Vector2 jumpVector) {
        this.stickingToWall = stickingToWall;
        this.jumpVector = jumpVector;
    }
}

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rigidbody;
    private bool canJump = false;
    private WallJump wallJump;
    private bool canMove = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Changing worlds");
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(canJump)
            {
                rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            } else if(wallJump.stickingToWall)
            {
                rigidbody.velocity = new Vector2(0, 0);
                rigidbody.AddForce(wallJump.jumpVector, ForceMode2D.Impulse);
                canMove = false;
                Invoke("ReactivateControls", 0.5f);
            }
            
        } 
        if(!canMove) {
            return;
        }
        if(Input.GetKey(KeyCode.LeftArrow)) {
            rigidbody.velocity = new Vector2(-2, rigidbody.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity = new Vector2(2, rigidbody.velocity.y);
        }
    }

    public void SetJumpStatus(bool status)
    {
        canJump = status;
    }

    private void SetWallJumpStatus(WallJump wj)
    {
        wallJump = wj;
    }

    private void ReactivateControls() {
        canMove = true;
    }

}
