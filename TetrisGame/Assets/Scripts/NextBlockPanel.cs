using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlockPanel : MonoBehaviour
{
    GameObject nextBlockPanel;
    GameObject nextBlock;
    // Start is called before the first frame update
    void Start()
    {
        nextBlockPanel = GameObject.Find("NBP");
        nextBlock = GameObject.FindObjectOfType<Spawner>()._nextBlock;
    }

    // Update is called once per frame
    void Update()
    {
        /*nextBlock = GameObject.FindObjectOfType<Spawner>()._nextBlock;
        if (nextBlock.GetComponent<Group>().mode == Group.GroupMode.Queued)
        {
            nextBlock.layer = LayerMask.NameToLayer("UI");
            BoxCollider2D[] tempBoxColliders = nextBlock.GetComponentsInChildren<BoxCollider2D>();
            for (int i = 0; i < nextBlock.GetComponentsInChildren<BoxCollider2D>().Length; i++)
                nextBlock.GetComponentInChildren<BoxCollider2D>().enabled = false;
            nextBlock.transform.position = nextBlockPanel.transform.position;
            Debug.Log(tempBoxColliders);
        }*/
    }
}
