using UnityEngine;
using System.Collections;

public class SeletorFinalScore : MonoBehaviour
{

    public AudioClip menu_enter;
    public AudioClip menu_select;

    private AudioSource source;

    private bool freezeControls;

    //public GameObject menuContainer;
    //private Animator menuAnimator;

    public GameObject seletorMenu;
    public GameObject seletorRetry;

    private enum RETRYMENU
    {
        RETRY,
        MENU
    }

    private RETRYMENU selecao;

    private GameObject menu_btn;
    private GameObject retry_btn;

    private string gameLevel = "Game";
    private string menuLevel = "Menu";


    void Start()
    {
        //menuAnimator = menuContainer.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        /*
        gameObject.transform.position =
            new Vector3(
                gameObject.transform.position.x,
                play_btn.transform.position.y,
                gameObject.transform.position.z
                );
        */
        SetSeletores(false, true);

        selecao = RETRYMENU.RETRY;
    }

    void SetSeletores(bool menu, bool retry)
    {
        seletorMenu.SetActive(menu);
        seletorRetry.SetActive(retry);
    }

    void Update()
    {
        if (!freezeControls)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                if (selecao != RETRYMENU.MENU)
                {
                    SetSeletores(true, false);
                    selecao = RETRYMENU.MENU;
                    source.PlayOneShot(menu_select, 1);
                }

            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                if (selecao != RETRYMENU.RETRY)
                {
                    SetSeletores(false, true);
                    selecao = RETRYMENU.RETRY;
                    source.PlayOneShot(menu_select, 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetAxis("Enter") > 0)
            {
                switch (selecao)
                {
                    case RETRYMENU.MENU:
                        LoadLevel(menuLevel);
                        break;
                    case RETRYMENU.RETRY:
                        LoadLevel(gameLevel);
                        break;
                }
            }
        }
    }

    IEnumerator Load(string level)
    {
        source.PlayOneShot(menu_enter, 1);
        freezeControls = true;
        GameObject.FindGameObjectWithTag("FinalScoreObject").GetComponent<Animator>().SetTrigger("ShutDown");
        //menuAnimator.SetTrigger("ShutDown");
        yield return new WaitForSeconds(.75f);
        Application.LoadLevel(level);
    }

    void LoadLevel(string level)
    {
        if (!freezeControls)
        {
            if (level.Equals("Game")) SetSeletores(false, true);
            else if (level.Equals("Menu")) SetSeletores(true, false);
            StartCoroutine(Load(level));
        }
    }
}
