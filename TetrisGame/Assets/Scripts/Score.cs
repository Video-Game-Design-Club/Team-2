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
        int hiScore = PlayerPrefs.GetInt("hiScore", 0);
        heightText = GameObject.Find("HeightValue");
        scoreText = GameObject.Find("HiScore");
        scoreText.GetComponent<TextMeshProUGUI>().text = ("HI SCORE\n\n" + hiScore);
    }

    // Update is called once per frame
    void Update()
    {
        heightText.GetComponent<TextMeshProUGUI>().text = (lastHeight * 10).ToString();

        if ((int)(GameObject.Find("Player").transform.position.y) > lastHeight)
            lastHeight = (int)(GameObject.Find("Player").transform.position.y);
    }

    private void OnDestroy()
    {
        if(PlayerPrefs.GetInt("hiScore", 0) < (lastHeight * 10))
        PlayerPrefs.SetInt("hiScore", (lastHeight * 10));
    }
}
