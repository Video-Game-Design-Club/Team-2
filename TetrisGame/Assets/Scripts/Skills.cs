using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skills : MonoBehaviour
{
    Transform blockPunchText;
    Transform blockKickText;
    Transform freezeTimeText;
    // Start is called before the first frame update
    void Start()
    {
        blockPunchText = GameObject.Find("BlockPunch").transform.GetChild(0);
        blockKickText = GameObject.Find("BlockKick").transform.GetChild(0);
        freezeTimeText = GameObject.Find("FreezeTime").transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player").GetComponent<CharacterController2D>().punchCooldown >= 0)
            blockPunchText.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(GameObject.Find("Player").GetComponent<CharacterController2D>().punchCooldown).ToString();
        
        if(GameObject.Find("Player").GetComponent<CharacterController2D>().kickCooldown >= 0)
            blockKickText.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(GameObject.Find("Player").GetComponent<CharacterController2D>().kickCooldown).ToString();
        
        if(GameObject.Find("Player").GetComponent<CharacterController2D>().freezeTimeCooldown >= 0)
            freezeTimeText.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(GameObject.Find("Player").GetComponent<CharacterController2D>().freezeTimeCooldown).ToString();
    }
}
