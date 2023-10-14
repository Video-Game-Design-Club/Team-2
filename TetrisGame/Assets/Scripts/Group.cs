using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public enum GroupMode
    {
        // active means it is falling and in play.
        Active,
        // queued means that it is not currently in play, but will be soon.
        Queued,
        // set means that it is part of the playfield
        Set
    }
    // Time since last gravity tick
    float lastFall = 0;
    public GroupMode mode = GroupMode.Queued;
    public float blockFallTime;
    bool firstFrame = false;

    public static int height = 0;

    // Start is called before the first frame update
    void Start()
    {
        /*// Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // this group member should not fall or respond to key press yet
        if (mode != GroupMode.Active)
        {
            return;
        }

        // Move Downwards and Fall
        if (Time.timeScale != 0 && Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ||
         Time.time - lastFall >= blockFallTime)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
                if (transform.position.y <= (Camera.main.transform.position.y - 10f))
                {
                    gameObject.GetComponent<Group>().mode = GroupMode.Set;
                    FindObjectOfType<Spawner>().spawnNext();
                    enabled = false;
                }
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                int blockHeight = (int)transform.position.y;

                if (blockHeight > height)
                    height = blockHeight;

                // Spawn next Group
                gameObject.GetComponent<Group>().mode = GroupMode.Set;
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
                if (!firstFrame)
                    DestroyImmediate(gameObject);
            }
            lastFall = Time.time;
            firstFrame = true;
        }
    }

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            // Not inside Border?
            if (!Playfield.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform && 
                Playfield.grid[(int)v.x, (int)v.y].parent.GetComponent<Group>().mode != GroupMode.Active)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public void moveLeft()
    {
        // Move Leftposition
        transform.position += new Vector3(-1, 0, 0);

        // See if it's valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // Its not valid. revert.
            transform.position += new Vector3(1, 0, 0);
    }

    public void moveRight()
    {
        // Move Right
        transform.position += new Vector3(1, 0, 0);

        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            transform.position += new Vector3(-1, 0, 0);
    }

    // Rotate code moved to function we can call from character script.
    public void Rotate()
    {
        transform.Rotate(0, 0, -90);

        //Rotate the sprite of the blocks so they always face up
        transform.GetChild(0).Rotate(0, 0, 90);
        transform.GetChild(1).Rotate(0, 0, 90);
        transform.GetChild(2).Rotate(0, 0, 90);
        transform.GetChild(3).Rotate(0, 0, 90);

        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
        {
            // It's not valid. revert.
            transform.Rotate(0, 0, 90);
            transform.GetChild(0).Rotate(0, 0, -90);
            transform.GetChild(1).Rotate(0, 0, -90);
            transform.GetChild(2).Rotate(0, 0, -90);
            transform.GetChild(3).Rotate(0, 0, -90);
        }
    }    

}