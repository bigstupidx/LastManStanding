using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    private int _totalScore = 0;
    private float timeOffset = .1f;
    private float _totalTime;

    private GameController controller;

    public bool _ativo;

	// Use this for initialization
	void Start ()
	{
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        EventManager.onPointsChange += onPointsChange;
        EventManager.onSetAtivo += onSetAtivo;
        EventManager.onLoadNewScene += onLoadNewScene;

		_totalTime = 0;
		guiText.text = "0000";
	}

	void onLoadNewScene()
	{
        EventManager.onPointsChange -= onPointsChange;
        EventManager.onSetAtivo -= onSetAtivo;
        EventManager.onLoadNewScene -= onLoadNewScene;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(_ativo)
        {
		    _totalTime += Time.deltaTime;

            if (_totalTime > timeOffset)
            {
                _totalScore += 2;
                _totalTime = 0;
            }
        }
		guiText.text = _totalScore+"";
	}

	public float GetTotalTime()
	{
		return _totalTime;
	}

    void onSetAtivo(bool ativo)
    {
        _ativo = ativo;

		if(!ativo)
		{
			controller.SetPoints(_totalScore);
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

    public int GetTotalScore()
    {
        return _totalScore;
    }
}
