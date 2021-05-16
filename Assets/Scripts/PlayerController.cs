using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    private Rigidbody2D playerRB;



    private List<NPCController> NPCs;
    private NPCController availableNPC_;



    public enum Type { NONE, HAND, FOOT };
    public enum Side { NONE, LEFT, RIGHT };

    public int jumpsLeft = 1;



    private Side direction_ = Side.RIGHT;
    private Side wallAtHand_ = Side.NONE;
    private Side wallAtFoot_ = Side.NONE;
    private Side walledOn_ = Side.NONE;

    private float wallJumpTimer_ = 0;



    public bool isRunning_ = false;
    public bool isGrounded_ = false;
    public bool isAired_ = true;
    public bool isFrozen_ = false;

    public int contactCount_ = 0;

    // Sounds
    static string jumpSound = "Sounds/Jump";
    AudioSource jumpAudioSource;



    enum AnimClip
    {
        idle, running, jumping, walled
    }
    AnimClip currentAnimation = AnimClip.idle;



    private Vector2 wallJumpPower_ = new Vector2(5f, 7f);
    private Vector2 jumpPower_ = new Vector2(0, 7f);
    private const float maxSpeed = 5f;
    private const float groundedAcceleration = 30f;
    private const float airedAcceleration = 15f;



    private Vector2 startingPosition;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRB = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

        GameObject utopicNPCs = GameObject.Find("UtopicNPCs");
        GameObject dystopicNPCs = GameObject.Find("DystopicNPCs");

        NPCs = new List<NPCController>();
        if (utopicNPCs != null)
            foreach (NPCController NPC in utopicNPCs.GetComponentsInChildren<NPCController>())
            {
                NPC.SetDimension(GameManager.State.UTOPIA);
                NPCs.Add(NPC);
                if (NPC.GetType() == typeof(SurvivorController))
                {
                    NPCController droppedItem = (NPC as SurvivorController).droppedItem;
                    if (droppedItem != null)
                    {
                        droppedItem.gameObject.SetActive(true);
                        droppedItem.SetDimension(GameManager.State.UTOPIA);
                        droppedItem.gameObject.SetActive(false);
                        NPCs.Add(droppedItem);
                    }
                }
            }
        if (dystopicNPCs != null)
            foreach (NPCController NPC in dystopicNPCs.GetComponentsInChildren<NPCController>())
            {
                NPC.SetDimension(GameManager.State.DYSTOPIA);
                NPCs.Add(NPC);
                if (NPC.GetType() == typeof(SurvivorController))
                {
                    NPCController droppedItem = (NPC as SurvivorController).droppedItem;
                    if (droppedItem != null)
                    {
                        droppedItem.gameObject.SetActive(true);
                        droppedItem.SetDimension(GameManager.State.DYSTOPIA);
                        droppedItem.gameObject.SetActive(false);
                        NPCs.Add(droppedItem);
                    }
                }
            }

        SetState();

        jumpAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource.clip = Resources.Load(jumpSound) as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { gameManager.TogglePause(); }



        if (!gameManager.GetIsPaused())
        {
            if (Input.GetKeyDown(KeyCode.Q)) { Interact(); }



            if (!isFrozen_)
            {
                if (gameManager.GetCanSwitch())
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        gameManager.SwitchState();
                        SetState();
                    }



                bool moving = false;
                // Can not move if walled or on corner, falling (bug)
                if (!isWalled() && !(isAired_ && contactCount_ > 0))
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
                        WallJump(walledOn_);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                        Jump();



                    setRunning(moving);
                    if (moving)
                    {
                        bool ok = playerRB.velocity.x == 5;

                        if (isAired_)
                            playerRB.velocity = playerRB.velocity + new Vector2(dir * airedAcceleration * Time.deltaTime, 0);
                        else
                            playerRB.velocity = playerRB.velocity + new Vector2(dir * groundedAcceleration * Time.deltaTime, 0);

                        if (dir * playerRB.velocity.x > maxSpeed)
                            playerRB.velocity = new Vector2(dir * maxSpeed, playerRB.velocity.y);
                    }
                    else if (isGrounded_)
                    {
                        playerRB.velocity = new Vector2(0, playerRB.velocity.y);
                    }

                    // Set flip
                    GetComponent<SpriteRenderer>().flipX = (direction_ == Side.LEFT) ? true : false;
                }



                // Set walled if going the right direction
                if (isAired_ && wallAtHand_ == direction_ && wallAtFoot_ == direction_ && wallJumpTimer_ == 0)
                {
                    if (!isWalled())
                        WallOn(direction_);
                }
                else if (isWalled())
                    WallOn(Side.NONE);



                // Reduce jump timer
                if (wallJumpTimer_ > 0)
                    wallJumpTimer_ = Mathf.Max(wallJumpTimer_ - Time.deltaTime, 0);
            }



            if (!isAired_)
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);



            if (isFrozen_)
            {
                if (currentAnimation != AnimClip.idle)
                    Animate(AnimClip.idle);
            }
            else
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
    }




    private void SetState()
    {
        Animate(currentAnimation);

        // Switch NPCs
        foreach (var NPC in NPCs)
        {
            if (NPC.GetType() == typeof(CollectibleController))
                (NPC as CollectibleController).SetVisibility(gameManager.GetState());
            else
                NPC.SetVisibility(gameManager.GetState());
        }
    }


    private void Animate(AnimClip clip)
    {
        bool isDystopia = gameManager.GetState() == GameManager.State.DYSTOPIA;
        bool isUtopia = gameManager.GetState() == GameManager.State.UTOPIA;

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
            GetComponent<Animator>().Play(name, 0, frameTime);
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
        playerRB.velocity = new Vector2(playerRB.velocity.x + jumpPower.x, jumpPower.y);
        isAired_ = true;
        jumpsLeft--;

        jumpAudioSource.Play();
    }
    private void Jump()
    {
        if (jumpsLeft > 0)
            JumpImpulsion(jumpPower_);
    }
    private void WallJump(Side directionOfWall)
    {
        if (jumpsLeft > 0)
        {
            // Jump opposite to wall
            direction_ = (directionOfWall == Side.LEFT) ? Side.RIGHT : Side.LEFT;
            // Jump
            JumpImpulsion(new Vector2(wallJumpPower_.x * SideToDir(direction_), wallJumpPower_.y));
            wallJumpTimer_ = 0.1f;
        }
    }



    int SideToDir(Side side) { return (side == Side.LEFT) ? -1 : 1; }



    private void WallAtHand(Side side) { wallAtHand_ = side; }
    private void WallAtFoot(Side side) { wallAtFoot_ = side; }
    private void NoWallAtHand(Side side) { wallAtHand_ = (wallAtHand_ == side) ? Side.NONE : wallAtHand_; }
    private void NoWallAtFoot(Side side) { wallAtFoot_ = (wallAtFoot_ == side) ? Side.NONE : wallAtFoot_; }



    private void WallOn(Side side)
    {
        walledOn_ = side;

        // If walled
        if (isWalled())
        {
            playerRB.gravityScale = 0.25f;
            jumpsLeft = gameManager.GetMaxJumps();
            // Stop movement
            playerRB.velocity = Vector2.zero;
        }
        else
        {
            playerRB.gravityScale = 1f;
            //if (!isGrounded_)
            //    jumpsLeft = 0;
        }
    }



    private void Ground(bool isGrounded)
    {
        isGrounded_ = isGrounded;

        isAired_ = !isGrounded;
        if (isGrounded)
            jumpsLeft = gameManager.GetMaxJumps();
    }

    private void Die()
    {
        transform.position = startingPosition;
    }





    public void Freeze(bool frozen) { isFrozen_ = frozen; }



    public void GiveAbility(GameManager.Ability ability)
    {
        switch (ability)
        {
            case GameManager.Ability.SWITCH:
                gameManager.SetCanSwitch(true);
                break;
            case GameManager.Ability.DOUBLE_JUMP:
                gameManager.SetMaxJumps(2);
                jumpsLeft = gameManager.GetMaxJumps();
                break;
        }
    }



    private void Interact()
    {
        if (availableNPC_ != null)
        {
            if (availableNPC_.canFlip)
                availableNPC_.GetComponent<SpriteRenderer>().flipX = availableNPC_.gameObject.transform.position.x > transform.position.x;

            if (availableNPC_.GetType() == typeof(CollectibleController))
                (availableNPC_ as CollectibleController).Interact(this);
            else if (availableNPC_.GetType() == typeof(SurvivorController))
                (availableNPC_ as SurvivorController).Interact(this);
            else
                availableNPC_.Interact(this);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision) { contactCount_++; }
    private void OnCollisionExit2D(Collision2D collision) { contactCount_--; }



    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (availableNPC_ == null)
        {
            NPCController NPC = collider.GetComponent<NPCController>();
            if (NPC != null)
                if (NPCs.Contains(NPC))
                {
                    availableNPC_ = NPC;
                    availableNPC_.ShowQButton(true);
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (availableNPC_ != null)
            if (collider.GetComponent<NPCController>() == availableNPC_)
            {
                if (!availableNPC_.freezePlayerOnInteract)
                    availableNPC_.StopInteraction(this);

                availableNPC_.ShowQButton(false);
                availableNPC_ = null;
            }
    }
}
