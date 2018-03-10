using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    private float _totalTime;
    private float _hiScoreTime;
    public int _totalScore { get; set; }

    public bool _ativo;
    private bool isNewRecord = true;

    public AudioClip newRecord;

    public GameObject timeSegundos;
    public GameObject timeMilesimos;

    private UILabel timeSegundosLabel;
    private UILabel timeMilesimosLabel;

    public GameObject hiscoreSegundos;
    public GameObject hiscoreMilesimos;

    private string _totalTimeFormatted;

    private NGUIGameController controller;

    void onLoadNewScene()
    {
        EventManager.onPointsChange -= onPointsChange;
        EventManager.onSetAtivo -= onSetAtivo;
        EventManager.onLoadNewScene -= onLoadNewScene;
    }

    void Start()
    {
        EventManager.onSetAtivo += onSetAtivo;
        EventManager.onLoadNewScene += onLoadNewScene;
        EventManager.onPointsChange += onPointsChange;

        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<NGUIGameController>();

        InicializarTimer();
        InicializarHiScore();

    }

    void InicializarTimer()
    {
        _totalTime = 0;

        if (timeSegundos != null)
        {
            timeSegundosLabel = timeSegundos.GetComponent<UILabel>();
            timeSegundosLabel.text = "0";
        }
        if (timeMilesimos != null)
        {
            timeMilesimosLabel = timeMilesimos.GetComponent<UILabel>();
            timeMilesimosLabel.text = "00";
        }
    }

    void InicializarHiScore()
    {
        _hiScoreTime = controller.GetHiScore();

        hiscoreSegundos.GetComponent<UILabel>().text = string.Format("{0}", (int)_hiScoreTime);
        hiscoreMilesimos.GetComponent<UILabel>().text = string.Format("{0}{1}", ((int)(_hiScoreTime * 10)) % 10, ((int)(_hiScoreTime * 100)) % 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (_ativo)
        {
            _totalTime += Time.deltaTime;

            timeSegundosLabel.text = string.Format("{0}", (int)_totalTime);
            timeMilesimosLabel.text = string.Format("{0}{1}", ((int)(_totalTime * 10)) % 10, ((int)(_totalTime * 100)) % 10);
        }
        CheckNewRecord();
    }

    void CheckNewRecord()
    {
        if (_totalTime > _hiScoreTime)
        {
            hiscoreSegundos.GetComponent<UILabel>().text = timeSegundosLabel.text;
            hiscoreMilesimos.GetComponent<UILabel>().text = timeMilesimosLabel.text;

            if (isNewRecord)
            {
                //gameObject.GetComponent<AudioSource>().PlayOneShot(newRecord, 1);
                isNewRecord = false;
            }
        }
    }

    public float GetTotalTime()
    {
        return _totalTime;
    }

    public void ShowFinalTime()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).position =
            new Vector3(
                -1.555189f,
                0.8747941f,
                0
                );
    }

    void onSetAtivo(bool ativo)
    {
        _ativo = ativo;

        if (!ativo)
        {
            controller.SetTime(_totalTime);//_totalTimeFormatted);
            //gameObject.SetActive(ativo);
        }
    }

    public int GetFinalScore()
    {
        return this._totalScore;
    }

    void onPointsChange(int points)
    {
        this._totalScore += points;
    }
}
