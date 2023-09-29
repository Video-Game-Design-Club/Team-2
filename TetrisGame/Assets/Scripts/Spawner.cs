using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const float NEXT_BLOCK_Y_OFFSET = 8;
    // Groups
    public GameObject[] groups;
    public Sprite[] sprites;
    public GameObject player;

    private GameObject _nextBlock;

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

        if (_nextBlock == null)
        {
            GenerateNext();
        }

        // Spawn Group at current Position
        int x = Random.Range(2, 9);
        int y = (int)(player.transform.position.y + 14);
        Vector3 pos = transform.position;
        pos.x = x;
        pos.y = y;
        _nextBlock.transform.SetPositionAndRotation(pos, transform.rotation);
        _nextBlock.GetComponent<Group>().mode = Group.GroupMode.Active;

        GenerateNext();

    }

    public void GenerateNext()
    {
        // Random Index
        int i = Random.Range(0, groups.Length);

        //Get random sprite index
        int j = Random.Range(0, sprites.Length);
        int k = Random.Range(0, sprites.Length);
        int l = Random.Range(0, sprites.Length);
        int m = Random.Range(0, sprites.Length);

        // we set the next block outside the playfield but still visible
        // TODO: put this on the ui layer ?
        _nextBlock = Instantiate(groups[i],
                    new Vector3(-2, player.transform.position.y + NEXT_BLOCK_Y_OFFSET, 1),
                    Quaternion.identity);

        //Here we set the sprites of the objects
        _nextBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[j];
        _nextBlock.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[k];
        _nextBlock.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = sprites[l];
        _nextBlock.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = sprites[m];

        //Create a new scale var and use it to set the scales of the sprites and the collisions
        _nextBlock.transform.GetChild(0).transform.localScale = new Vector3(0.05f, 0.05f, 1);
        _nextBlock.transform.GetChild(1).transform.localScale = new Vector3(0.05f, 0.05f, 1);
        _nextBlock.transform.GetChild(2).transform.localScale = new Vector3(0.05f, 0.05f, 1);
        _nextBlock.transform.GetChild(3).transform.localScale = new Vector3(0.05f, 0.05f, 1);

    }



}
