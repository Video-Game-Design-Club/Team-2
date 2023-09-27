using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Groups
    public GameObject[] groups;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn initial Group
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnNext()
    {
        // Random Index
        int i = Random.Range(0, groups.Length);
        
        //Get random sprite index
        int j = Random.Range(0, sprites.Length);
        int k = Random.Range(0, sprites.Length);
        int l = Random.Range(0, sprites.Length);
        int m = Random.Range(0, sprites.Length);

        // Spawn Group at current Position
        GameObject spawnedBlock = Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);

        //Here we set the sprites of the objects
        spawnedBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[j];
        spawnedBlock.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[k];
        spawnedBlock.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = sprites[l];
        spawnedBlock.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = sprites[m];
        spawnedBlock.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        spawnedBlock.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = null; 
        spawnedBlock.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        spawnedBlock.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        //Create a new scale var and use it to set the scales of the sprites and the collisions
        spawnedBlock.transform.GetChild(0).transform.localScale = new Vector3(0.05f, 0.05f, 1);
        spawnedBlock.transform.GetChild(1).transform.localScale = new Vector3(0.05f, 0.05f, 1);
        spawnedBlock.transform.GetChild(2).transform.localScale = new Vector3(0.05f, 0.05f, 1);
        spawnedBlock.transform.GetChild(3).transform.localScale = new Vector3(0.05f, 0.05f, 1);

        spawnedBlock.transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(19, 19, 1);
        spawnedBlock.transform.GetChild(1).GetChild(0).transform.localScale = new Vector3(19, 19, 1);
        spawnedBlock.transform.GetChild(2).GetChild(0).transform.localScale = new Vector3(19, 19, 1);
        spawnedBlock.transform.GetChild(3).GetChild(0).transform.localScale = new Vector3(19, 19, 1);
    }
}
