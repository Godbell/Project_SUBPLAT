using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField]
    private Animator pauseScreenAnimator;
    [SerializeField]
    private Animator bookAnimator;

    private bool pauseScreenCheck = false;
    private bool exitWindowCheck = false;

    private bool characterCheck;
    private bool skillCheck;
    private bool collectionCheck;
    private bool optionCheck;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !(exitWindowCheck))
        {
            pauseScreenCheck = !(pauseScreenCheck);
            pauseScreenAnimator.SetBool("pauseScreenOn", pauseScreenCheck);
            if (pauseScreenCheck)
            {
                characterOn();
            }
            else
            {
                emptyPage();
            }
        }

        bookAnimator.SetBool("CharacterOn", characterCheck);
        bookAnimator.SetBool("SkillOn", skillCheck);
        bookAnimator.SetBool("CollectionOn", collectionCheck);
        bookAnimator.SetBool("OptionOn", optionCheck);
    }

    public void exitWindowOn()
    {
        exitWindowCheck = true;
    }

    public void exitWindowOff()
    {
        exitWindowCheck = false;
    }

    public void characterOn()
    {
        emptyPage();
        characterCheck = true;
    }

    public void skillOn()
    {
        emptyPage();
        skillCheck = true;
    }

    public void collectionOn()
    {
        emptyPage();
        collectionCheck = true;
    }

    public void optionOn()
    {
        emptyPage();
        optionCheck = true;
    }

    public void emptyPage()
    {
        characterCheck = false;
        skillCheck = false;
        collectionCheck = false;
        optionCheck = false;
    }

    public void MoveTitle()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlayBgm("Title");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
