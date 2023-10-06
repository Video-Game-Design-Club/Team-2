using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]


public class StateMachine : MonoBehaviour
{
    public State currentState;
    Rigidbody2D r2d;

    // Start is called before the first frame update
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            currentState = State.Test0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentState= State.Test1;
        }

        switch (currentState)
        {
            case State.Test0:
                Debug.Log("Test 0");
                break;

            case State.Test1:
                Debug.Log("Test 1");
                break;
        }
    }
}

public enum State
{
    Test0,
    Test1,
    Test2
}
