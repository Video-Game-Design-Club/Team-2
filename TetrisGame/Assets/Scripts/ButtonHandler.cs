using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    Button pauseButton;
    [SerializeField]
    Button volumeButton;
    [SerializeField]
    Button helpButton;
    [SerializeField]
    Button restartButton;

    bool togglePause = false;
    bool toggleSlider = false;
    bool toggleHelp = false;

    [SerializeField]
    GameObject volumeSlider;
    [SerializeField]
    GameObject helpPopup;
    [SerializeField]
    GameObject goPopup;
    [SerializeField]
    GameObject pausePopup;

    [SerializeField]
    Transform player;//I'm sorry dont hurt me

    [SerializeField]
    float gameOverTime = 1.5f;
    float goTimer = 0f;

    [SerializeField]
    AudioClip goClip;

    bool audioOnce = true;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        helpPopup.gameObject.SetActive(false);
        volumeSlider.gameObject.SetActive(false);
        goPopup.gameObject.SetActive(false);
        pausePopup.gameObject.SetActive(false);
        float vol = PlayerPrefs.GetFloat("Vol", 0.5f);
        volumeSlider.GetComponent<Slider>().onValueChanged.AddListener(volumeChange);
        AudioListener.volume = vol;
        volumeSlider.GetComponent<Slider>().value = vol;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y < Camera.main.transform.position.y - 10)
        {
            goTimer += Time.deltaTime;
        }
        else
        {
            goTimer = 0f;
        }

        if (goTimer > gameOverTime)
        {
            //Collossal L
            Time.timeScale = 0;
            goPopup.gameObject.SetActive(true);
            if (audioOnce)
            {
                audioOnce = false;
                Camera.main.GetComponent<AudioSource>().Stop();
                Camera.main.GetComponent<AudioSource>().pitch = 1;
                Camera.main.GetComponent<AudioSource>().PlayOneShot(goClip);
            }
        }
    }

    private void OnDestroy()
    {
        if(volumeSlider != null)
            volumeSlider.GetComponent<Slider>().onValueChanged.RemoveListener(volumeChange);
    }

    void volumeChange(float vol)
    {
        AudioListener.volume = volumeSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Vol", AudioListener.volume);
    }

    public void PauseGame()
    {
        if (!togglePause)
        {
            //add blur
            Time.timeScale = 0;
            togglePause = true;
            pausePopup.gameObject.SetActive(true);
        }
        else
        {
            //remove blur
            Time.timeScale = 1;
            togglePause = false;
            pausePopup.gameObject.SetActive(false);
        }
    }

    public void VolumePopup()
    {
        if (!toggleSlider)
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

    public void HelpPopup()
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

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
