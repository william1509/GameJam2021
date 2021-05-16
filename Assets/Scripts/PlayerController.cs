﻿using System.Collections;
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
    public GameManager gameManager;
    private Rigidbody2D playerRB;
    private bool canJump = false;
    private WallJump wallJump;
    private bool canMove = true;

    [SerializeField] float playerSpeed = 5f; 

    private Vector2 startingPosition;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            gameManager.ChangeWorlds();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(canJump)
            {
                playerRB.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            } else if(wallJump.stickingToWall)
            {
                playerRB.velocity = new Vector2(0, 0);
                playerRB.AddForce(wallJump.jumpVector, ForceMode2D.Impulse);
                canMove = false;
            }
            
        } 
        if(!canMove) {
            return;
        }
        if(Input.GetKey(KeyCode.LeftArrow)) {
            playerRB.velocity = new Vector2(-1 * playerSpeed, playerRB.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRB.velocity = new Vector2(1 * playerSpeed, playerRB.velocity.y);
        }
    }

    public void SetJumpStatus(bool status)
    {
        canJump = status;
        float friction = status ? 5 : 0;
        Debug.Log(friction);
        GetComponent<Collider2D>().sharedMaterial.friction = friction;
    }

    private void SetWallJumpStatus(WallJump wj)
    {
        wallJump = wj;
        float friction = wj.stickingToWall ? 0 : 5;
        GetComponent<Collider2D>().sharedMaterial.friction = 0.0f;
    }

    private void ReactivateControls() {
        canMove = true;
        
    }

    private void PlayerDied() {
        transform.position = startingPosition;
    }

}
