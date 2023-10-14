using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

//nate was here

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    [Header("The other shit")]
    public State currentState = State.Walk;
    public float maxSpeed = 3.4f;
    public float jumpHeight = 12f;
    public float gravityScale = 1.5f;
    public AnimationCurve accelerationCurve;
    public Camera mainCamera;
    public LayerMask notPlayer;
    public CapsuleCollider2D mainCollider;

    [Header("Acceleration Settings")]
    public float accelerationStrength = 1.0f;
    public float decelerationStrength = 1.0f;
    public float turnbackStrength = 1.0f;
    public float airStrafeStrength = 1.0f;
    public float airStrafeDecelerationStrength = 1.0f;

    [Header("Clamber Settings")]
    public float upClamberStrength = 1.0f;
    public float sideClamberStrength = 1.0f;
    public float clamberDelay = 1.0f;

    [Header("Wall Grab Settings")]
    public float wallholdGravityScalar = 0.33f;
    int wallholdDirection = 0;
    public float walljumpStrength = 1.0f;
    public float walljumpDelay = 1.0f;
    bool walljumpLock = false;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    int jumpAmmount = 1;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    Animator animator;

    Transform t;
    float accelerationTimer = 0f;
    bool clamberLock = false;
    bool jumpInput = false;

    //Block kick variables
    Group[] allBlocks;
    Group activeBlock;
    [Header("Skill Cooldown Settings")]
    public float kickCooldown = 0f;
    public float punchCooldown = 0f;
    public float freezeTimeCooldown = 0f;

    bool doCamShift = false;

    public enum State
    {
        Walk,
        Jump,
        Fall,
        Clamber,
        Wallhold,
        Walljump
    }

    void DoWalk()
    {
        if(Math.Abs(r2d.velocity.x) < maxSpeed && moveDirection != 0)
        {
            r2d.AddForce(new Vector2(accelerationStrength * moveDirection, 0f));
        }
        else if(Math.Abs(r2d.velocity.x) > 0)
        {
            r2d.AddForce(new Vector2(-r2d.velocity.x * decelerationStrength, 0f));
        }
        
    }

    void DoAirStrafe()
    {
        if(Input.GetKey(KeyCode.A) && Math.Abs(r2d.velocity.x) < maxSpeed)
        {
            r2d.AddForce(new Vector2(-airStrafeStrength, 0f));
        }
        else if(Input.GetKey(KeyCode.D) && Math.Abs(r2d.velocity.x) < maxSpeed)
        {
            r2d.AddForce(new Vector2(airStrafeStrength, 0f));
        }
        else if(Math.Abs(r2d.velocity.x) > 0)
        {
            r2d.AddForce(new Vector2(-r2d.velocity.x * airStrafeDecelerationStrength, 0f));
        }
    }

    void DoJump()
    {
        jumpAmmount--;
        r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        animator.SetTrigger("Jump");
    }

    void DoClamber()
    {
        clamberLock = true;

        animator.SetTrigger("Clamber");

        if (feetLeftRay())
        {

            StartCoroutine(clamber(-1));
        }
        else
        {
            StartCoroutine(clamber(1));
        }
    }

    void DoBlockKick(bool direction)
    {
        if (direction && activeBlock && kickCooldown <= 0f && midRightRayResult().collider != null && midRightRayResult().transform.parent == activeBlock.transform)
        {
            activeBlock.moveRight();
            kickCooldown = 5f;
            activeBlock = null;
        }
        else if(activeBlock && kickCooldown <= 0f && midLeftRayResult().collider != null && midLeftRayResult().transform.parent == activeBlock.transform)
        {
            activeBlock.moveLeft();
            kickCooldown = 5f;
            activeBlock = null;
        }
    }

    void DoWallhold()
    {
        r2d.gravityScale = gravityScale * wallholdGravityScalar;
    }

    void DoWalljump()
    {
        walljumpLock = true;
        StartCoroutine(walljump(wallholdDirection));
    }
    void DoBlockPunch()
    {
        if (activeBlock && punchCooldown <= 0f && HeadUpRayResult().collider != null && HeadUpRayResult().transform.parent == activeBlock.transform)
        {
            activeBlock.Rotate();
            punchCooldown = 5f;
            activeBlock = null;
        }
    }

    RaycastHit2D HeadUpRayResult()
    {
        return Physics2D.Raycast(transform.position, Vector2.up, .9f, notPlayer);
    }

    #region LeftRays
    bool headLeftRay()
    {
        return Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector2.left, .4f, notPlayer);
    }

    bool midLeftRay()
    {
        return Physics2D.Raycast(transform.position, Vector2.left, .4f, notPlayer);
    }
    
    RaycastHit2D midLeftRayResult()
    {
        return Physics2D.Raycast(transform.position, Vector2.left, .4f, notPlayer);
    }

    bool feetLeftRay()
    {
        return Physics2D.Raycast(transform.position - Vector3.up * 0.6f, Vector2.left, .4f, notPlayer);
    }
    #endregion

    #region RightRays
    bool headRightRay()
    {
        return Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector2.right, .4f, notPlayer);
    }

    bool midRightRay()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, .4f, notPlayer);
    }

    RaycastHit2D midRightRayResult()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, .4f, notPlayer);
    }

    bool feetRightRay()
    {
        return Physics2D.Raycast(transform.position - Vector3.up * 0.6f, Vector2.right, .4f, notPlayer);
    }
    #endregion

    IEnumerator clamber(int direction)
    {
        r2d.velocity = new Vector2(0, 0);
        r2d.AddForce(new Vector2(0f, upClamberStrength), ForceMode2D.Impulse);
        yield return new WaitForSeconds(clamberDelay);
        r2d.AddForce(new Vector2(sideClamberStrength * direction, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        currentState = State.Fall;
        clamberLock = false;
    }

    IEnumerator walljump(int direction)
    {
        r2d.AddForce(new Vector2(walljumpStrength * -direction, walljumpStrength * 1.5f), ForceMode2D.Impulse);
        r2d.gravityScale = gravityScale;
        wallholdDirection = 0;
        //jumpAmmount++;
        yield return new WaitForSeconds(walljumpDelay);
        walljumpLock = false;
        currentState = State.Fall;
    }

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
        animator = GetComponentInChildren<Animator>();

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Kick and punch cooldown, slowly decrementing from 5
        kickCooldown -= Time.deltaTime;
        punchCooldown -= Time.deltaTime;

        if ((int)(Time.time % 5) >= 3)
        {
            doCamShift = true;
        }

        if ((int)(Time.time % 5) == 0 && doCamShift)
        {
            doCamShift = false;
            cameraPos.y += 1;
        }

        //Check if active block ref is stale, if so, null it out. Otherwise check for the most recent active block.
        if ((activeBlock != null) && (activeBlock.mode != Group.GroupMode.Active))
            activeBlock = null;
        else if (activeBlock == null)
        {
            allBlocks = FindObjectsOfType<Group>();
            for (int i = 0; i < allBlocks.Length; i++)
                if (allBlocks[i].mode == Group.GroupMode.Active)
                    activeBlock = allBlocks[i];
        }

        //set moveDirection
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;

        }
        else
        {
            moveDirection = 0;
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
            }
        }

        // Change sprite facing direction
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && (jumpAmmount >= 1))
        {
            jumpInput = true;
        }

        // Camera follow
        if (mainCamera)
        {
            if (transform.position.y >= cameraPos.y)
                cameraPos.y = transform.position.y;
            mainCamera.transform.position = new Vector3(10, cameraPos.y, -10);
        }

        animator.SetBool("IsFalling", currentState == State.Fall);
        animator.SetBool("IsOnWall", currentState == State.Wallhold);
        animator.SetBool("IsRunning", r2d.velocity.x != 0 && isGrounded);
    }

    void FixedUpdate()
    {
        //new isGrounded system v1.013451
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position - Vector3.up * 0.4f, 0.35f, notPlayer);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && jumpAmmount != 1)
        {
            jumpAmmount = 2;
        }

        //state mahcine :3
        switch (currentState)
        {
            case State.Walk:

                DoWalk();

                if (jumpInput && (jumpAmmount >= 1))
                {
                    currentState = State.Jump; break;
                }

                if (!isGrounded)
                {
                    currentState = State.Fall; break;
                }

                if (feetLeftRay() && !headLeftRay() && moveDirection == -1)
                {
                    currentState = State.Clamber; break;
                }
                if (feetRightRay() && !headRightRay() && moveDirection == 1)
                {
                    currentState = State.Clamber; break;
                }
                break;

            case State.Jump:

                DoJump();

                currentState = State.Fall;

                break;

            case State.Fall:

                DoAirStrafe();

                if(Input.GetKey(KeyCode.F) && facingRight)
                {
                    DoBlockKick(facingRight);
                }
                else if(Input.GetKey(KeyCode.F) && !facingRight)
                {
                    DoBlockKick(facingRight);
                }

                //to jump
                if (jumpInput && (jumpAmmount >= 1))
                {
                    currentState = State.Jump; break;
                }

                //to walk
                if (isGrounded)
                {
                    currentState = State.Walk;
                    jumpAmmount = 2;
                    break;
                }

                //to clamber
                if (feetLeftRay() && !headLeftRay() && moveDirection == -1)
                {
                    currentState = State.Clamber; break;
                }
                if (feetRightRay() && !headRightRay() && moveDirection == 1)
                {
                    currentState = State.Clamber; break;
                }

                //to wallhold
                if(headLeftRay() && midLeftRay() && feetLeftRay() && Input.GetKey(KeyCode.A))
                {
                    wallholdDirection = -1;
                    r2d.velocity = new Vector2(r2d.velocity.x, 0);
                    currentState = State.Wallhold; break;
                }

                if (headRightRay() && midRightRay() && feetRightRay() && Input.GetKey(KeyCode.D))
                {
                    wallholdDirection = 1;
                    r2d.velocity = new Vector2(r2d.velocity.x, 0);
                    currentState = State.Wallhold; break;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    DoBlockPunch();
                }

                break;

            case State.Clamber:

                if (clamberLock == false)
                {
                    DoClamber();
                }

                break;

            case State.Wallhold:
                DoWallhold();

                //to fall
                if (!Input.GetKey(KeyCode.A) && wallholdDirection == -1)
                {
                    wallholdDirection = 0;
                    r2d.gravityScale = gravityScale;
                    currentState = State.Fall; break;
                }

                if (!Input.GetKey(KeyCode.D) && wallholdDirection == 1)
                {
                    wallholdDirection = 0;
                    r2d.gravityScale = gravityScale;
                    currentState = State.Fall; break;
                }

                //to walljump
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
                {
                    currentState = State.Walljump; break;
                }

                break;

            case State.Walljump:
                if(walljumpLock == false)
                {
                    DoWalljump();
                }
                
                break;
        }

        jumpInput = false;

        // Simple debug
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
}