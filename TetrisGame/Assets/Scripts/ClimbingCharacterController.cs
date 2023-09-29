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

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    int jumpAmmount = 1;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    public CapsuleCollider2D mainCollider;
    Transform t;
    float accelerationTimer = 0f;

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log(isGrounded);
        }
        if (collision.gameObject.tag != "Ground")
        {
            isGrounded = false;
            Debug.Log(isGrounded);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log(isGrounded);
        }
        if (collision.gameObject.tag != "Ground")
        {
            isGrounded = false;
            Debug.Log(isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            Debug.Log(isGrounded);
        }
    }*/

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
            //Nate: Got rid of the "isGrounded" parameter
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

        // Change facing direction
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
        //Debug.Log(jumpAmmount);
        //Debug.Log(isGrounded);

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
       

        //movement acceleration
/*        if (moveDirection != 0)
        {
            if (accelerationTimer <= 1)
            {
                accelerationTimer += 1f * Time.deltaTime;
            }
        }
        else
        {
            if (accelerationTimer > 0)
            {
                accelerationTimer -= 2f * Time.deltaTime;
            }
        }*/
    }

    void FixedUpdate()
    {
        /*
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        */

        Debug.DrawRay(transform.position, Vector2.right*1f, Color.red, Time.fixedDeltaTime);
        

        //new isGrounded system v1.013451
        if (Physics2D.Raycast(transform.position, Vector2.down, .9f, notPlayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Debug.Log(isGrounded);


        if (Physics2D.Raycast(transform.position, Vector2.left, .4f, notPlayer))
        {
            // Debug.Log("Working");
            if(moveDirection < 0)
            {
                accelerationTimer = 0;
            }
        }

        if (Physics2D.Raycast(transform.position, Vector2.right, .4f, notPlayer))
        {
            // Debug.Log("Working");
            if (moveDirection > 0) 
            {
                accelerationTimer = 0;
            }
        }



        //check if player is touching an object to the side


        // Apply movement velocity
        r2d.velocity = new Vector2(maxSpeed * accelerationCurve.Evaluate(accelerationTimer), r2d.velocity.y);
        //r2d.AddForce(new Vector2((moveDirection) * maxSpeed, r2d.velocity.y));


        // Simple debug
        Debug.Log(accelerationTimer);
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
}
