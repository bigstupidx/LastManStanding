using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour
{
	private float _totalTime;

    public bool _ativo;

	private string _totalTimeFormatted;

	private GameController controller;

	// Use this for initialization
	void Start ()
	{
        EventManager.onSetAtivo += onSetAtivo;

		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		_totalTime = 0;
		guiText.text = "0''00";
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(_ativo)
        {
		    _totalTime += Time.deltaTime;

			_totalTimeFormatted = string.Format("{0}''{1}{2}", (int) _totalTime, ((int)(_totalTime * 10)) % 10, ((int)(_totalTime * 100)) % 10);
		    guiText.text = _totalTimeFormatted;
        }
	}

	public float GetTotalTime()
	{
		return _totalTime;
	}

    void onSetAtivo(bool ativo)
    {
        _ativo = ativo;

		if (!ativo)
		{
			controller.SetTime(_totalTimeFormatted);
		}
    }
}
