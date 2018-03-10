using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject gameElements;
    private Animator gameElementsAnimator;

    public GameObject scoreObject;
    private ScoreScript scoreClass;
    private int _points = 0;
	private string _time;

    private AudioSource source;
    public AudioSource musicSource;

    public AudioClip start_game;
    public AudioClip start_game2;
    public AudioClip end_game;

    public GameObject finalScoreObject;
    public GameObject finalScoreValueObject;
    private GUIText finalScoreValue;

	public GameObject finalTimeValueObject;
	private GUIText finalTimeValue;

	public GameObject bestScoreValueObject;
	private GUIText bestScoreValue;
    /*
    //-----EVENT MANAGER-----
    public delegate void GameHandler(bool ativo);
    public static event GameHandler onSetAtivo;

	public delegate void LoadSceneHandler();
	public static event LoadSceneHandler onLoadNewScene;
    //-----------------------
	*/
    void Start ()
    {
        EventManager.onPlayerDeath += onPlayerDeath;

        source = gameObject.GetComponent<AudioSource>();
        gameElementsAnimator = gameElements.GetComponent < Animator >();
        StartCoroutine(StartGame());
	}

	void Load(string level)
	{
        EventManager.onPlayerDeath -= onPlayerDeath;

		//onLoadNewScene();
        EventManager.Instance.onLoadNewSceneEvent();
		Application.LoadLevel(level);
	}

	IEnumerator StartGame()
	{
        //gameElementsAnimator.SetTrigger("Start");
        //play start sound
        source.PlayOneShot(start_game2);

        yield return new WaitForSeconds(.75f);
        source.PlayOneShot(start_game);
        //onSetAtivo(true);
        EventManager.Instance.onSetAtivoEvent(true);
        yield return new WaitForSeconds(1.6f);
        musicSource.Play();
    }

    public void SetPoints(int points)
    {
        this._points = points;
    }

	public void SetTime(string time)
	{
		this._time = time;
	}

    void onPlayerDeath()
    {
        EventManager.onPlayerDeath -= onPlayerDeath;
        musicSource.Stop();

        source.PlayOneShot(end_game);
        gameElementsAnimator.SetTrigger("End");
		
        //onSetAtivo(false);
        EventManager.Instance.onSetAtivoEvent(false);
        StartCoroutine(ShowFinalScore());
    }

    IEnumerator ShowFinalScore()
    {
        yield return new WaitForSeconds(.7f);
		
		finalScoreObject.SetActive(true);

		finalScoreValue = finalScoreValueObject.GetComponent<GUIText>();
		finalScoreValue.text = this._points+"";

		finalTimeValue = finalTimeValueObject.GetComponent<GUIText>();
		finalTimeValue.text = this._time;

        int highScore = PersistenceHelper.Instance.ReadInteger(PersistenceHelper.HIGHSCORE_KEY);

        if (_points > highScore)
		{
            highScore = _points;
            PersistenceHelper.Instance.PersistInteger(PersistenceHelper.HIGHSCORE_KEY, highScore);
		}

		bestScoreValue = bestScoreValueObject.GetComponent<GUIText>();
		bestScoreValue.text = highScore + "";
    }

    public void SetHiScore(int value)
    {
        PersistenceHelper.Instance.PersistInteger(PersistenceHelper.HIGHSCORE_KEY, value);
    }

    public int GetHiScore()
    {
        return PersistenceHelper.Instance.ReadInteger(PersistenceHelper.HIGHSCORE_KEY);
    }
}
