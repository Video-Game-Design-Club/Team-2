using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    // Time since last gravity tick
    float lastFall = 0;
    public float blockFallTime;

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
        /*if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire");
            if(Time.timeScale <= 99)
            Time.timeScale += 1f;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Alt-Fire");
            Time.timeScale = 1;
        }*/

        // Rotate
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
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

        // Move Downwards and Fall
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ||
         Time.time - lastFall >= blockFallTime)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Playfield.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
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
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
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
}
