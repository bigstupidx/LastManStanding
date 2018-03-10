using UnityEngine;
using System.Collections;

public class NGUIGameController : MonoBehaviour
{
    public GameObject gameElements;
    private Animator gameElementsAnimator;

    public GameObject scoreObject;
    private ScoreScript scoreClass;
    private int _points = 0;
    private float _time;

    private AudioSource source;
    public AudioSource musicSource;

    public AudioClip start_game;
    public AudioClip start_game2;
    public AudioClip end_game;

    public GameObject scoreScreen;
    public GameObject gameScreen;
    public GameObject scoreManager;
    public GameObject endGameBtns;

    public GameObject scoreValues;

    //public GameObject finalTimeSecondsValueObject;
    //public GameObject finalTimeMillisValueObject;

    public GameObject bestTimeSecondsValueObject;
    public GameObject bestTimeMillisValueObject;
    
    /*
    //-----EVENT MANAGER-----
    public delegate void GameHandler(bool ativo);
    public static event GameHandler onSetAtivo;

    public delegate void LoadSceneHandler();
    public static event LoadSceneHandler onLoadNewScene;
    //-----------------------
    */
    void Start()
    {
        EventManager.onPlayerDeath += onPlayerDeath;

        source = gameObject.GetComponent<AudioSource>();
        gameElementsAnimator = gameElements.GetComponent<Animator>();
        StartCoroutine(StartGame());
    }

    void Load(string level)
    {
        EventManager.onPlayerDeath -= onPlayerDeath;

        //onLoadNewScene();
        EventManager.Instance.onLoadNewSceneEvent();
        Application.LoadLevel(level);
    }

	void OnApplicationQuit()
	{
		PersistenceHelper.Instance.Save();
        PlayerPrefs.Save();
	}
	
	void OnApplicationPause()
	{
		PersistenceHelper.Instance.Save();
        PlayerPrefs.Save();
	}

    IEnumerator StartGame()
    {
        //gameElementsAnimator.SetTrigger("Start");
        //play start sound
        source.PlayOneShot(start_game2);

        yield return new WaitForSeconds(.75f);
        //source.PlayOneShot(start_game);
        //onSetAtivo(true);
        EventManager.Instance.onSetAtivoEvent(true);
        musicSource.Play();
        yield return new WaitForSeconds(1.6f);
    }

    public void SetPoints(int points)
    {
        this._points = points;
    }

    public void SetTime(float time)
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
        yield return new WaitForSeconds(1.5f);
        scoreManager.SetActive(false);
        scoreScreen.SetActive(true);
        endGameBtns.SetActive(true);
        gameScreen.SetActive(false);
        scoreValues.SetActive(true);

        //Debug.Log("time: " + _time);

        float highScore = GetHiScore();//PersistenceHelper.Instance.ReadFloat(PersistenceHelper.HIGHTIME_KEY);

        if (_time > highScore)
        {
            highScore = _time;
            SetHiScore(highScore);
            //PersistenceHelper.Instance.PersistFloat(PersistenceHelper.HIGHTIME_KEY, highScore);
        }
        scoreManager.gameObject.SetActive(false);
        //segundos
        scoreValues.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<UILabel>().text = string.Format("{0}", (int)_time); ;
        //milesimos
        scoreValues.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<UILabel>().text = string.Format("{0}{1}", ((int)(_time * 10)) % 10, ((int)(_time * 100)) % 10);;
        //scoreManager.SendMessage("ShowFinalTime", null, SendMessageOptions.DontRequireReceiver);

        //finalTimeSecondsValueObject.GetComponent<UILabel>().text = string.Format("{0}", (int)_time);
        //Debug.Log("secs: " + string.Format("{0}", (int)_time));
        //Debug.Log("secs.text: " + finalTimeSecondsValueObject.GetComponent<UILabel>().text);
        //finalTimeMillisValueObject.GetComponent<UILabel>().text = string.Format("{0}{1}", ((int)(_time * 10)) % 10, ((int)(_time * 100)) % 10);
        //Debug.Log("millis: " + string.Format("{0}{1}", ((int)(_time * 10)) % 10, ((int)(_time * 100)) % 10));
        //Debug.Log("millis.text: " + finalTimeMillisValueObject.GetComponent<UILabel>().text);


        bestTimeSecondsValueObject.GetComponent<UILabel>().text = string.Format("{0}", (int)highScore);
        bestTimeMillisValueObject.GetComponent<UILabel>().text = string.Format("{0}{1}", ((int)(highScore * 10)) % 10, ((int)(highScore * 100)) % 10);
    }

    public void SetHiScore(float value)
    {
        //PersistenceHelper.Instance.PersistFloat(PersistenceHelper.HIGHTIME_KEY, value);
        PlayerPrefs.SetFloat("Record", value);
        PlayerPrefs.Save();
    }

    public float GetHiScore()
    {
        float score = PlayerPrefs.GetFloat("Record");
        PlayerPrefs.Save();
        return score;
        //return PersistenceHelper.Instance.ReadFloat(PersistenceHelper.HIGHTIME_KEY);
    }
}
