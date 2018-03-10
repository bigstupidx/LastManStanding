using UnityEngine;
using System.Collections;

public class MenuSeletor : MonoBehaviour
{

    public AudioClip menu_enter;
    public AudioClip menu_select;

    private AudioSource source;

    private bool freezeControls;

    public GameObject menuContainer;
    private Animator menuAnimator;

    private enum MENU
    {
        PLAY,
        CREDITS
    }

    private MENU selecao;

    private GameObject play_btn;
    private GameObject score_btn;

    private string gameLevel = "Game";
    private string creditsLevel = "Credits";


    void Start()
    {
        menuAnimator = menuContainer.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();

        play_btn = GameObject.FindGameObjectWithTag("Play_Btn");
        score_btn = GameObject.FindGameObjectWithTag("Score_Btn");

        gameObject.transform.position =
            new Vector3(
                gameObject.transform.position.x,
                play_btn.transform.position.y,
                gameObject.transform.position.z
                );

        selecao = MENU.PLAY;
    }


    void Update()
    {
        if (!freezeControls)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                if (selecao != MENU.PLAY)
                {
                    SetSeletor(MENU.PLAY);
                    source.PlayOneShot(menu_select, 1);
                }

            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                if (selecao != MENU.CREDITS)
                {
                    SetSeletor(MENU.CREDITS);
                    source.PlayOneShot(menu_select, 1);
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
                }
            }
        }
    }

    IEnumerator Load(string level)
    {
        source.PlayOneShot(menu_enter, 1);
        freezeControls = true;
        menuAnimator.SetTrigger("ShutDown");
        yield return new WaitForSeconds(1.2f);
        Application.LoadLevel(level);
    }

    void SetSeletor(MENU menu)
    {
        switch (menu)
        {
            case MENU.PLAY:
                gameObject.transform.position =
                        new Vector3(
                            gameObject.transform.position.x,
                            play_btn.transform.position.y,
                            gameObject.transform.position.z
                            );
                selecao = MENU.PLAY;
                break;
            case MENU.CREDITS:
                gameObject.transform.position =
                        new Vector3(
                            gameObject.transform.position.x,
                            score_btn.transform.position.y,
                            gameObject.transform.position.z
                            );
                selecao = MENU.CREDITS;
                break;
        }
    }

    void LoadLevel(string level)
    {
        if (!freezeControls)
        {
            if (level.Equals("Game")) SetSeletor(MENU.PLAY);
            else if (level.Equals("Credits")) SetSeletor(MENU.CREDITS);

            StartCoroutine(Load(level));
        }
    }
}
