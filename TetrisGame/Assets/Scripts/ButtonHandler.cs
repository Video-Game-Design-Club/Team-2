using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    Button pauseButton;
    Button volumeButton;
    Button helpButton;

    bool togglePause = false;
    bool toggleSlider = false;
    bool toggleHelp = false;
    
    GameObject volumeSlider;
    GameObject helpPopup;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = GameObject.Find("VolumeSlider");
        helpPopup = GameObject.Find("HelpTextPopup");

        pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        volumeButton = GameObject.Find("VolumeButton").GetComponent<Button>();
        helpButton = GameObject.Find("HelpButton").GetComponent<Button>();

        pauseButton.onClick.AddListener(PauseGame);
        volumeButton.onClick.AddListener(VolumePopup);
        helpButton.onClick.AddListener(HelpPopup);

        helpPopup.gameObject.SetActive(false);
        volumeSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volumeSlider.GetComponent<Slider>().value;
    }

    void PauseGame()
    {
        if (!togglePause)
        {
            //add blur
            Time.timeScale = 0;
            togglePause = true;
        }
        else
        {
            //remove blur
            Time.timeScale = 1;
            togglePause = false;
        }
    }

    void VolumePopup()
    {
        if(!toggleSlider)
        {
            volumeSlider.gameObject.SetActive(true);
            toggleSlider = true;
        }
        else
        {
            volumeSlider.gameObject.SetActive(false);
            toggleSlider = false;
        }
    }

    void HelpPopup()
    {
        if (!toggleHelp)
        {
            PauseGame();
            helpPopup.gameObject.SetActive(true);
            toggleHelp = true;
        }
        else
        {
            PauseGame();
            helpPopup.gameObject.SetActive(false);
            toggleHelp = false;
        }
    }
}
