using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuOptions : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject menuLayer;
    public GameObject settingsLayer;
    public GameObject blurBackground;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    public bool disableOnStart, isHomeScreen;

    public void Start()
    {
        if (disableOnStart)
        {
            //  pauseMenuUI.gameObject.SetActive(false);
            menuLayer.SetActive(false);
            settingsLayer.gameObject.SetActive(false);

            if (isHomeScreen == false)
            {
                blurBackground.gameObject.SetActive(false);
            }
        } 

        resolutions = Screen.resolutions;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
        }


        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height )
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {


            if (GameIsPaused)
            {
                Resume();

            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
     //   pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        menuLayer.SetActive(false);
        settingsLayer.gameObject.SetActive(false);
        if (isHomeScreen == false)
        {
            blurBackground.gameObject.SetActive(false);
        }
    }
    void Pause()

    {
        if (isHomeScreen == false)
        {
            blurBackground.gameObject.SetActive(true);
        }
        pauseMenuUI.gameObject.SetActive(true);
        menuLayer.SetActive(true);
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

 

    public void OnClick_Continue()
    {
        Resume();
    }

    public void OnClick_Settings()
    {
        menuLayer.SetActive(false);
        settingsLayer.gameObject.SetActive(true);
    }

    public void OnClick_Credits()
    {
        menuLayer.SetActive(false);
    }
    public void OnClick_Quit()
    {
        Debug.Log("Quit Game...");
        Application.Quit();
    }


    public void OnClick_Back()
    {
        menuLayer.SetActive(false);
    }


    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); //0 - 5
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    public void setResolution (int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void onclick_Back()
    {
        menuLayer.SetActive(true);
        settingsLayer.SetActive(false);
    }

}
