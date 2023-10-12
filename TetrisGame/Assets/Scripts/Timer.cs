using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    int playTimeS = 0;
    int playTimeM = 0;
    int playTimeH = 0;
    GameObject secondsText;
    GameObject minutesText;
    GameObject hoursText;
    bool doOnce = true;
     

    // Start is called before the first frame update
    void Start()
    {
        secondsText = GameObject.Find("TimeValueS");
        minutesText = GameObject.Find("TimeValueM");
        hoursText = GameObject.Find("TimeValueH");
    }

    // Update is called once per frame
    void Update()
    {
        //playTimeS = (int)Time.realtimeSinceStartup % 60;
        playTimeS = (int)Time.time % 60;
        if (playTimeS == 30)
            doOnce = true;
        secondsText.GetComponent<TextMeshProUGUI>().text = playTimeS.ToString();
        if(playTimeS == 59 && doOnce)
        {
            playTimeM++;
            minutesText.GetComponent<TextMeshProUGUI>().text = playTimeM.ToString();
            doOnce = false;
        }
        if(playTimeM == 59)
        {
            playTimeM = 0;
            playTimeH++;
            hoursText.GetComponent<TextMeshProUGUI>().text = playTimeH.ToString();
        }
    }

    float normalizeTime()
    {
        return 0f;
    }
}
