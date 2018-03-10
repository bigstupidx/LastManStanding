using UnityEngine;
using System.Collections;

public class EndGameMenuController : MonoBehaviour
{
    public GameObject menuContainer;
    public GameObject retry;
    public GameObject menu;

    private AudioSource source;

    public AudioClip menu_enter;
    public AudioClip menu_select;

    private bool freezeControls = false;

    private enum MENU
    {
        RETRY,
        MENU,
    }

    private MENU selecao;

    private string gameLevel = "Game";
    private string menuLevel = "Menu";

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        selecao = MENU.RETRY;
        SetSeletor(selecao);
        //StartCoroutine(WaitAndSet(selecao));

        menu.SetActive(true);
        menu.GetComponent<Animator>().enabled = false;
        menu.GetComponent<SpriteRenderer>().enabled = false;

        retry.SetActive(true);
        retry.GetComponent<Animator>().enabled = false;
        retry.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator WaitAndSet(MENU selecao)
    {
        yield return new WaitForSeconds(0f);
        SetSeletor(selecao);
    }

    void Update()
    {
        if (!freezeControls)
        {
            if ((Input.GetAxis("Horizontal") > 0) || (Input.GetAxis("Vertical") < 0))
            {
                if (selecao != MENU.MENU)
                {
                    source.PlayOneShot(menu_select, .7f);
                    SetSeletor(MENU.MENU);
                }
            }
            else if ((Input.GetAxis("Vertical") > 0) || (Input.GetAxis("Horizontal") < 0))
            {
                if (selecao != MENU.RETRY)
                {
                    SetSeletor(MENU.RETRY);
                    source.PlayOneShot(menu_select, .7f);
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetAxis("Enter") > 0)
            {
                switch (selecao)
                {
                    case MENU.RETRY:
                        LoadLevel(gameLevel);
                        break;
                    case MENU.MENU:
                        LoadLevel(menuLevel);
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
        yield return new WaitForSeconds(1f);

        if (!level.Equals("Game"))
        {
            GameObject.FindGameObjectWithTag("StaticManager").SendMessage("ChangeScene", "Menu", SendMessageOptions.DontRequireReceiver);
        }

        Application.LoadLevel(level);
    }

    void SetSeletor(MENU value)
    {
        retry.GetComponent<Animator>().enabled = false;
        retry.GetComponent<SpriteRenderer>().enabled = false;

        menu.GetComponent<Animator>().enabled = false;
        menu.GetComponent<SpriteRenderer>().enabled = false;

        switch (value)
        {
            case MENU.RETRY:
                retry.GetComponent<Animator>().enabled = true;
                retry.GetComponent<SpriteRenderer>().enabled = true;
                selecao = MENU.RETRY;
                break;
            case MENU.MENU:
                menu.GetComponent<Animator>().enabled = true;
                menu.GetComponent<SpriteRenderer>().enabled = true;
                selecao = MENU.MENU;
                break;
        }
    }


    void LoadLevel(string level)
    {
        if (!freezeControls)
        {
            if (level.Equals(gameLevel))
            {
                menu.GetComponent<Animator>().enabled = false;
                menu.GetComponent<SpriteRenderer>().enabled = false;

                retry.GetComponent<Animator>().enabled = true;
                retry.GetComponent<SpriteRenderer>().enabled = true;
                retry.GetComponent<Animator>().SetTrigger("pressed");
            }
            else if (level.Equals(menuLevel))
            {
                retry.GetComponent<Animator>().enabled = false;
                retry.GetComponent<SpriteRenderer>().enabled = false;

                menu.GetComponent<Animator>().enabled = true;
                menu.GetComponent<SpriteRenderer>().enabled = true;
                menu.GetComponent<Animator>().SetTrigger("pressed");
            }

            freezeControls = true;

            StartCoroutine(NewScene(level));
        }
    }
}
