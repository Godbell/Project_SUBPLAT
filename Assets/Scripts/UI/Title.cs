using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject loby;
    [SerializeField]
    private GameObject option;

    private Vector3 distance;
    private bool OptionOpen = false;

    [SerializeField]
    private Text ScreenSizeText;

    public SaveLoadManager SLManager;

    private int ResolutionNumber = 0;
    private int[] ScreenWidth = { 0, 640, 800, 1024, 1280, 1920 };
    private string[] ResolutionString = { "Full Screen", "640x360", "800x450", "1024x576", "1280x720", "1920x1080" };

    void Start()
    {
        distance = mainCamera.transform.position - loby.transform.position;
        ResolutionNumber = SLManager.LoadResolution();
        Resolution_Ctr(ResolutionNumber);
        ScreenSizeText.text = ResolutionString[ResolutionNumber];
    }

    void Update()
    {
        if (OptionOpen)
        {
            if (mainCamera.transform.position.z > 100)
            {
                option.GetComponent<CanvasGroup>().interactable = true;
            }
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, option.transform.position + distance, 4f * Time.deltaTime);
        }
        else
        {
            if (mainCamera.transform.position.z < -8)
            {
                loby.GetComponent<CanvasGroup>().interactable = true;
            }
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, loby.transform.position + distance, 4f * Time.deltaTime);
        }
    }

    public void Option_Open()
    {
        OptionOpen = true;
    }

    public void Option_Close()
    {
        OptionOpen = false;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
        SoundManager.instance.PlayBgm("Stage1");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void FullScreen_Ctr()
    {
        Resolution[] allResolutions = Screen.resolutions;
        Resolution maxResolution = allResolutions[allResolutions.Length - 1];
        Screen.SetResolution(maxResolution.width, maxResolution.height, true);
    }

    public void Resolution_Ctr(int _ResolutionNumber)
    {
        if(_ResolutionNumber != 0)
        {
            int ScreenHeight = ScreenWidth[_ResolutionNumber] * 9 / 16;
            Screen.SetResolution(ScreenWidth[_ResolutionNumber], ScreenHeight, false);
        }
        else
        {
            FullScreen_Ctr();
        }
        ScreenSizeText.text = ResolutionString[_ResolutionNumber];
        ResolutionNumber = _ResolutionNumber;
        SLManager.SaveResolution(ResolutionNumber);
    }
}
