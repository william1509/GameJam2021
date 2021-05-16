using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct WallJump
{
    public bool stickingToWall;
    public Vector2 jumpVector;

    public WallJump(bool stickingToWall, Vector2 jumpVector)
    {
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



    private Dictionary<Side, Collider2D> sideColliders;
    public Collider2D leftCollider;
    public Collider2D rightCollider;



    private NPCController availableNPC_;



    public enum Side { NONE, LEFT, RIGHT };
    private Side direction_ = Side.RIGHT;
    private Side wallAt_ = Side.NONE;
    private Side walledOn_ = Side.NONE;



    public bool isRunning_ = false;
    public bool isGrounded_ = false;
    public bool isAired_ = false;



    enum AnimClip
    {
        idle, running, jumping, walled
    }
    AnimClip currentAnimation = AnimClip.idle;



    private Vector2 wallJumpPower_ = new Vector2(5f, 7f);
    private Vector2 jumpPower_ = new Vector2(0, 7f);



    [SerializeField] float playerSpeed = 5f;

    private Vector2 startingPosition;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

        sideColliders = new Dictionary<Side, Collider2D>()
        {
            { Side.LEFT, leftCollider },
            { Side.RIGHT, rightCollider }
        };
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameManager.SwitchState();
            Animate(currentAnimation);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Interact();
        }



        bool moving = false;
        if (!isAired_)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            { direction_ = Side.LEFT; moving = true; }
            else if (Input.GetKey(KeyCode.RightArrow))
            { direction_ = Side.RIGHT; moving = true; }
        }
        int dir = SideToDir(direction_);



        if (isWalled())
        {
            if (Input.GetKeyDown(KeyCode.Space))
                WallJump(direction_);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();



            setRunning(moving);
            if (moving)
                playerRB.velocity = new Vector2(dir * playerSpeed, playerRB.velocity.y);

            // Set flip
            GetComponent<SpriteRenderer>().flipX = (direction_ == Side.LEFT) ? true : false;
        }



        // Set walled if going the right direction
        if (isAired_ && wallAt_ == direction_)
            WallOn(direction_);
        else if (isWalled())
            WallOn(Side.NONE);



        // Animation
        switch (currentAnimation)
        {
            case AnimClip.idle:
                if (isRunning_)
                    Animate(AnimClip.running);
                else if (isAired_)
                    Animate(AnimClip.jumping);
                break;

            case AnimClip.running:
                if (!isRunning_)
                    Animate(AnimClip.idle);
                else if (isAired_)
                    Animate(AnimClip.jumping);
                break;

            case AnimClip.jumping:
                if (isWalled())
                    Animate(AnimClip.walled);
                else if (isGrounded_)
                {
                    if (isRunning_)
                        Animate(AnimClip.running);
                    else
                        Animate(AnimClip.idle);
                }
                break;

            case AnimClip.walled:
                if (!isWalled())
                {
                    if (isAired_)
                        Animate(AnimClip.jumping);
                    else
                        Animate(AnimClip.idle);
                }
                break;
        }
    }



    private void Animate(AnimClip clip)
    {
        bool isDystopia = gameManager.getState() == GameManager.State.DYSTOPIA;
        bool isUtopia = gameManager.getState() == GameManager.State.UTOPIA;

        string name = "";
        switch (clip)
        {
            case AnimClip.idle:
                if (isDystopia)
                    name = "dysto_idle";
                else if (isUtopia)
                    name = "uto_idle";
                break;
            case AnimClip.running:
                if (isDystopia)
                    name = "dysto_running";
                else if (isUtopia)
                    name = "uto_running";
                break;
            case AnimClip.jumping:
                if (isDystopia)
                    name = "dysto_jumping";
                else if (isUtopia)
                    name = "uto_jumping";
                break;
            case AnimClip.walled:
                if (isDystopia)
                    name = "dysto_wall";
                else if (isUtopia)
                    name = "uto_wall";
                break;
        }

        if (clip == currentAnimation)
        {
            // Start at same frame
            AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            float completion = stateInfo.normalizedTime;
            float duration = stateInfo.length;
            float frameTime = (completion % duration);
            //Debug.Log("Wanted : " + frameTime);
            GetComponent<Animator>().Play(name, 0, frameTime);
            //Debug.Log("Got : " + GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
            //Debug.Log("");
        }
        else
            // Simply play from start
            GetComponent<Animator>().Play(name);

        currentAnimation = clip;
    }



    private void setRunning(bool isRunning) { isRunning_ = isRunning; }
    private void setAired(bool isAired) { isAired_ = isAired; }
    private bool isWalled() { return walledOn_ != Side.NONE; }



    private void JumpImpulsion(Vector2 jumpPower)
    {
        if (canJump)
        {
            playerRB.AddForce(jumpPower, ForceMode2D.Impulse);
            isAired_ = true;
            canJump = false;
        }
    }
    private void Jump()
    {
        if (canJump)
            JumpImpulsion(jumpPower_);
    }
    private void WallJump(Side direction)
    {
        if (canJump)
        {
            // Switch direction
            direction_ = (direction_ == Side.LEFT) ? Side.RIGHT : Side.LEFT;
            // Jump
            JumpImpulsion(new Vector2(wallJumpPower_.x * SideToDir(direction_), wallJumpPower_.y));
        }
    }



    int SideToDir(Side side) { return (direction_ == Side.LEFT) ? -1 : 1; }



    private void WallAt(Side side)
    {
        wallAt_ = side;
    }
    private void NoWallAt(Side side)
    {
        wallAt_ = (wallAt_ == side) ? Side.NONE : wallAt_;
    }
    private void WallOn(Side side)
    {
        walledOn_ = side;

        // Walled
        if (isWalled())
        {
            GetComponent<Rigidbody2D>().gravityScale = 0.1f;
            canJump = true;
        }
        // NOT walled
        else
             GetComponent<Rigidbody2D>().gravityScale = 1;
    }



    private void Ground(bool isGrounded)
    {
        isGrounded_ = isGrounded;

        isAired_ = !isGrounded;
        if (isGrounded)
            canJump = true;
    }

    private void PlayerDied() {
        transform.position = startingPosition;
    }

    private void Interact()
    {
        if (availableNPC_ != null)
        {
            availableNPC_.GetComponent<SpriteRenderer>().flipX = availableNPC_.gameObject.transform.position.x > transform.position.x;
            availableNPC_.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (availableNPC_ == null)
        {
            NPCController NPC = collider.GetComponent<NPCController>();
            if (NPC != null)
                availableNPC_ = NPC;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (availableNPC_ != null)
            if (collider.GetComponent<NPCController>() == availableNPC_)
            {
                availableNPC_.StopInteraction();
                availableNPC_ = null;
            }
    }
}
