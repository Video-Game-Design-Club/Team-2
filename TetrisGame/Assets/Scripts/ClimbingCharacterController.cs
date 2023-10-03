using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

//nate was here

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 12f;
    public float gravityScale = 1.5f;
    public AnimationCurve accelerationCurve;
    public Camera mainCamera;
    public LayerMask notPlayer;

    public float accelerationStrength = 1.0f;
    public float decelerationStrength = 1.0f;
    public float turnbackStrength = 1.0f;

    public float upClamberStrength = 1.0f;
    public float sideClamberStrength = 1.0f;
    public float clamberDelay = 1.0f;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    int jumpAmmount = 1;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    public CapsuleCollider2D mainCollider;
    Transform t;
    float accelerationTimer = 0f;
    bool clamberLock = false;

    IEnumerator clamberRight()
    {
        r2d.velocity = new Vector2 (0, 0);
        r2d.AddForce(new Vector2(0f, upClamberStrength), ForceMode2D.Impulse);
        yield return new WaitForSeconds(clamberDelay);
        r2d.AddForce(new Vector2(sideClamberStrength, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        clamberLock = false;
        Debug.Log("working right");
        
    }

    IEnumerator clamberLeft()
    {
        r2d.velocity = new Vector2(0, 0);
        r2d.AddForce(new Vector2(0f, upClamberStrength), ForceMode2D.Impulse);
        yield return new WaitForSeconds(clamberDelay);
        r2d.AddForce(new Vector2(-sideClamberStrength, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        clamberLock = false;
        Debug.Log("working left");
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

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if ((Input.GetKey(KeyCode.D)))
        {
            if (accelerationTimer <= 1)
            {
                if (accelerationTimer <= 0)
                {
                    accelerationTimer += turnbackStrength * Time.deltaTime;
                }
                else
                accelerationTimer += accelerationStrength * Time.deltaTime;
            }
        }
        
        if((Input.GetKey(KeyCode.A)))
        {
            if (accelerationTimer >= -1)
            {
                if (accelerationTimer >= 0)
                {
                    accelerationTimer -= turnbackStrength * Time.deltaTime;
                }
                else
                accelerationTimer -= accelerationStrength * Time.deltaTime;
            }
        }
        
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (accelerationTimer > 0f)
            {
                accelerationTimer -= decelerationStrength * Time.deltaTime;
            }
            else if (accelerationTimer < 0f)
            {
                accelerationTimer += decelerationStrength * Time.deltaTime;
            }
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

        // Jumping
        if(isGrounded && jumpAmmount != 1)
        {
            jumpAmmount = 1;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && (jumpAmmount >= 1))
        {
            jumpAmmount--;
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, t.position.y, cameraPos.z);
        }
    }

    void FixedUpdate()
    {
        //new isGrounded system v1.013451
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position - Vector3.up * 0.4f, 0.37f, notPlayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //wall collision detection
        if (Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector2.left, .4f, notPlayer) || //head
            Physics2D.Raycast(transform.position - Vector3.up * 0.6f, Vector2.left, .4f, notPlayer) ||  //feet
            Physics2D.Raycast(transform.position, Vector2.left, .4f, notPlayer))                        //center
            {
                if (moveDirection < 0)
                {
                    accelerationTimer = 0;
                }
            }

        if (Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector2.right, .4f, notPlayer) || //head
            Physics2D.Raycast(transform.position - Vector3.up * 0.6f, Vector2.right, .4f, notPlayer) ||  //feet
            Physics2D.Raycast(transform.position, Vector2.right, .4f, notPlayer))                        //center
            {
                if (moveDirection > 0)
                {
                    accelerationTimer = 0;
                }
            }

        //clamber tech
        if (Physics2D.Raycast(transform.position - Vector3.up * 0.6f, Vector2.left, .4f, notPlayer) && !Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector2.left, .4f, notPlayer) && moveDirection == -1)
        {
            clamberLock = true;
            StartCoroutine(clamberLeft());
        }

        if (Physics2D.Raycast(transform.position - Vector3.up * 0.6f, Vector2.right, .4f, notPlayer) && !Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector2.right, .4f, notPlayer) && moveDirection == 1)
        {
            clamberLock = true;
            StartCoroutine(clamberRight());
        }


        // Apply movement velocity
        if (!clamberLock)
        {
            r2d.velocity = new Vector2(maxSpeed * accelerationCurve.Evaluate(accelerationTimer), r2d.velocity.y);
        }

        // Simple debug
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
}
