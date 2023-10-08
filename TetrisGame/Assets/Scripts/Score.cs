using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;
    GameObject scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("ScoreValue");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
