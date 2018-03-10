using UnityEngine;
using System.Collections;

public class GoToLevelScript : BaseClass
{
	public string level;

    void Start()
    {
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(0f);

        OnTouchDown();
    }

	void OnTouchDown()
	{
        this.animatorCache.SetTrigger("ShutDown");

        StartCoroutine(Load(level));		
	}

    IEnumerator Load(string level)
    {
        yield return new WaitForSeconds(.1f);
        Application.LoadLevel(level);
    }
}
