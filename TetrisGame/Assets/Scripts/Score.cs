using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;
    int lastHeight = 0;
    GameObject scoreText;
    GameObject heightText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("ScoreValue");
        heightText = GameObject.Find("HeightValue");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = (lastHeight * 10).ToString();
        heightText.GetComponent<TextMeshProUGUI>().text = lastHeight.ToString();

        if ((int)(GameObject.Find("Player").transform.position.y) > lastHeight)
            lastHeight = (int)(GameObject.Find("Player").transform.position.y);
    }
}
