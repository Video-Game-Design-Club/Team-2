using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const float NEXT_BLOCK_Y_OFFSET = 8;
    // Groups
    public GameObject[] groups;
    public Sprite[] sprites;
    public GameObject player;
    float fallTime = 1;
    int lastTime = 0;
    int blockType = 0;

    private GameObject _nextBlock;

    GameObject nextBlockPanel;

    // Start is called before the first frame update
    void Start()
    {
        //Find the object in the scene that has the name NBP.
        nextBlockPanel = GameObject.Find("NBP");

        // Spawn initial Group
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextBlock.GetComponent<Group>().mode == Group.GroupMode.Queued)
        {
            //Set the block's position to the UI panel's position.
            _nextBlock.transform.position = nextBlockPanel.transform.position;
        }
    }

    public void spawnNext()
    {
        int timeCheck = (int)Time.time; //Throw time into an int so we can drop the numbers after the decimal.
        if ((timeCheck > 1 && ((timeCheck % 10) == 0)) || ((timeCheck - lastTime) > 100) ) //Check to see if 10 seconds passed, if so then make blocks fall faster. 
        {
            timeCheck++;
            fallTime -= .1f;
            lastTime += 100;
        }

        if (_nextBlock == null)
        {
            GenerateNext();
        }
        _nextBlock.layer = LayerMask.NameToLayer("Default");
        _nextBlock.transform.SetParent(null);

        // Spawn Group at current Position
        int playfieldEdge = Playfield.w - 1;
        int x = Random.Range(1, playfieldEdge); //Range of 1 to Playfield.w - 1, 1 is the left of our playfield and 19 is the right of the playfield.
        switch (blockType)
        {
            case 0:             //I block
                //This block doesn't need to be moved over, leave it where it is.
                break;
            case 1:             //J block
                if (x == 1)
                    x++; //This block's center will spawn it out of bounds only on the left wall, so move it right one block.
                break;
            case 2:             //L block
                if (x == playfieldEdge)
                    x--; //This block's center will spawn it out of bounds only on the right wall, so move it left one block.
                break;
            case 3:             //O block
                if (x == playfieldEdge)
                    x--; //This block's center will spawn it out of bounds only on the right wall, so move it left one block.
                break;
            case 4:             //S  block
                if (x == 1)
                    x++; //This block's center will spawn it out of bounds on the left wall, so move it right one block.
                else if (x == playfieldEdge)
                    x--; //This block's center will spawn it out of bounds on the right wall, so move it left one block.
                break;
            case 5:             //T block
                if (x == 1)
                    x++; //This block's center will spawn it out of bounds on the left wall, so move it right one block.
                else if (x == playfieldEdge)
                    x--; //This block's center will spawn it out of bounds on the right wall, so move it left one block.
                break;
            case 6:             //Z block
                if (x == 1)
                    x++; //This block's center will spawn it out of bounds on the left wall, so move it right one block.
                else if (x == playfieldEdge)
                    x--; //This block's center will spawn it out of bounds on the right wall, so move it left one block.
                break;
            default:
                break;
        }
        int y = (int)(player.transform.position.y + 14);
        Vector3 pos = transform.position;
        pos.x = x;
        pos.y = y;
        _nextBlock.transform.SetPositionAndRotation(pos, transform.rotation);
        _nextBlock.GetComponent<Group>().mode = Group.GroupMode.Active;
        _nextBlock.GetComponent<Group>().blockFallTime = fallTime; //Needed since we want the block fall time to vary. 

        GenerateNext();
    }

    public void GenerateNext()
    {
        // Random Index
        int i = Random.Range(0, groups.Length);
        blockType = i;

        // we set the next block into the ui panel
        _nextBlock = Instantiate(groups[i],
                    nextBlockPanel.transform.position,
                     Quaternion.identity);

        //If the block is Queued then we want to move it to the UI layer.
        if (_nextBlock.GetComponent<Group>().mode == Group.GroupMode.Queued)
            _nextBlock.layer = LayerMask.NameToLayer("UI");
            _nextBlock.transform.SetParent(nextBlockPanel.transform);

        //Get random sprite index
        int j = Random.Range(0, sprites.Length);
        int k = Random.Range(0, sprites.Length);
        int l = Random.Range(0, sprites.Length);
        int m = Random.Range(0, sprites.Length);

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
