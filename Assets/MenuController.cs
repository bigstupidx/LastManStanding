using UnityEngine;
using System.Collections;

public class MenuController : BaseClass
{
    public GameObject menuContainer;
    public GameObject credits;
    public GameObject game;
    public GameObject tutorial;

    private AudioSource source;

    public AudioClip menu_enter;
    public AudioClip menu_select;

    private bool freezeControls;

    private enum MENU
    {
        PLAY,
        CREDITS,
        TUTORIAL
    }

    private MENU selecao;

    private string gameLevel = "Game";
    private string creditsLevel = "Credits";
    private string tutorialLevel = "Tutorial";

    void Start()
    {
        source = this.audioSourceCache;
        StartCoroutine(WaitAndSet(selecao));

        credits.SetActive(true);
        game.SetActive(true);
        tutorial.SetActive(true);

        credits.GetComponent<Animator>().enabled = false;
        tutorial.GetComponent<Animator>().enabled = false;
        game.GetComponent<Animator>().enabled = false;


        credits.GetComponent<SpriteRenderer>().enabled = false;
        tutorial.GetComponent<SpriteRenderer>().enabled = false;
        game.GetComponent<SpriteRenderer>().enabled = false;

        //SetSeletor(selecao);
    }

    IEnumerator WaitAndSet(MENU selecao)
    {
        yield return new WaitForSeconds(.5f);
        SetSeletor(selecao);
    }

    void Update()
    {
        if (!freezeControls)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                if (selecao != MENU.CREDITS)
                {
                    source.PlayOneShot(menu_select, .7f);
                    SetSeletor(MENU.CREDITS);
                }
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                if (selecao != MENU.TUTORIAL)
                {
                    source.PlayOneShot(menu_select, .7f);
                    SetSeletor(MENU.TUTORIAL);
                }
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                if (selecao != MENU.PLAY)
                {
                    SetSeletor(MENU.PLAY);
                    source.PlayOneShot(menu_select, .7f);
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetAxis("Enter") > 0)
            {
                switch (selecao)
                {
                    case MENU.PLAY:
                        LoadLevel(gameLevel);
                        break;
                    case MENU.CREDITS:
                        LoadLevel(creditsLevel);
                        break;
                    case MENU.TUTORIAL:
                        LoadLevel(tutorialLevel);
                        break;
                }
            }
        }
    }

    IEnumerator NewScene(string level)
    {
        source.PlayOneShot(menu_enter, 1);
        freezeControls = true;
        //menuContainer.GetComponent<Animator>().SetTrigger("ShutDown");
        menuContainer.GetComponent<Animator>().SetTrigger("ShutDown");
        yield return new WaitForSeconds(1.2f);

        if (level.Equals("Game"))
        {
            GameObject.FindGameObjectWithTag("StaticManager").SendMessage("ChangeScene", "Game", SendMessageOptions.DontRequireReceiver);
        }

        Application.LoadLevel(level);
    }

    void SetSeletor(MENU menu)
    {
        switch (menu)
        {
            case MENU.PLAY:
                SetComponents(false, false, true);
                selecao = MENU.PLAY;
                break;
            case MENU.CREDITS:
                SetComponents(true, false, false);
                selecao = MENU.CREDITS;
                break;
            case MENU.TUTORIAL:
                SetComponents(false, true, false);
                selecao = MENU.TUTORIAL;
                break;
        }
    }

    void SetComponents(bool creditsValue, bool tutorialValue, bool gameValue)
    {
        credits.GetComponent<Animator>().enabled = creditsValue;
        credits.GetComponent<SpriteRenderer>().enabled = creditsValue;
        
        tutorial.GetComponent<Animator>().enabled = tutorialValue;
        tutorial.GetComponent<SpriteRenderer>().enabled = tutorialValue;
        
        game.GetComponent<Animator>().enabled = gameValue;
        game.GetComponent<SpriteRenderer>().enabled = gameValue;
    }

    void LoadLevel(string level)
    {
        if (!freezeControls)
        {
            if (level.Equals(gameLevel))
            {
                SetComponents(false, false, true);
                game.GetComponent<Animator>().SetTrigger("pressed");
            }
            else if (level.Equals(creditsLevel))
            {
                SetComponents(true, false, false);
                credits.GetComponent<Animator>().SetTrigger("pressed");
            }
            else if (level.Equals(tutorialLevel))
            {
                SetComponents(false, true, false);
                tutorial.GetComponent<Animator>().SetTrigger("pressed");
            }

            //source.PlayOneShot(menu_enter, 1);
            freezeControls = true;

            if (level.Equals("Credits"))
            {
                GameObject.FindGameObjectWithTag("StaticManager").SendMessage("ChangeScene", level, SendMessageOptions.DontRequireReceiver);
            }

            StartCoroutine(NewScene(level));
            //menuContainer.GetComponent<Animator>().SetTrigger("ShutDown");
            //yield return new WaitForSeconds(1.2f);
            //Application.LoadLevel(level);
        }
    }
}
