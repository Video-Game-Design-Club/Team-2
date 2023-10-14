using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    //int playTimeS = 0;
    //int playTimeM = 0;
    //int playTimeH = 0;
    GameObject secondsText;
    GameObject minutesText;
    GameObject hoursText;
    //bool doOnce = true;
     

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
        //Adam I'm so sorry it had to be done
        /*playTimeS = (int)Time.time % 60;
        if (playTimeS == 30)
            doOnce = true;
        secondsText.GetComponent<TextMeshProUGUI>().text = playTimeS.ToString();
        if(playTimeS == 59 && doOnce)
        {
            playTimeM++;
            secondsText.GetComponent<TextMeshProUGUI>().text = playTimeS.ToString();
            doOnce = false;
        }
        if(playTimeM == 59)
        {
            playTimeM = 0;
            playTimeH++;
            hoursText.GetComponent<TextMeshProUGUI>().text = playTimeH.ToString();
        }*/

        int playTimeS = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).Seconds;
        int playTimeM = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).Minutes;
        int playTimeH = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).Hours;

        string timeS = (playTimeS < 10)?"0" + playTimeS : "" + playTimeS;
        string timeM = (playTimeM < 10)?"0" + playTimeM : "" + playTimeM;
        string timeH = (playTimeH < 10)?"0" + playTimeH : "" + playTimeH;

        secondsText.GetComponent<TextMeshProUGUI>().text = timeS;
        minutesText.GetComponent<TextMeshProUGUI>().text = timeM;
        hoursText.GetComponent<TextMeshProUGUI>().text = timeH;
    }

    float normalizeTime()
    {
        return 0f;
    }
}
