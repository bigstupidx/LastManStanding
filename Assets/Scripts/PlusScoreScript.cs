using UnityEngine;
using System.Collections;

public class PlusScoreScript : MonoBehaviour
{
    private int plusPoints = 0;

    public int pointsPerMob = 100;

    private float showTime;
    private float maxShowTime = 2f;
    
    /*
    //-----EVENT MANAGER-----
    public delegate void PointsHandler(int points);
    public static event PointsHandler onPointsChange;
    //-----------------------
	*/

	public GameObject labelTemplate;

    void Start ()
    {
        //EventManager.onMobDie += onMobDie;
        Mob.onMobDie += onMobDie;
	}

    void onMobDie(object sender, MobDeathEventArgs args)
    {
        plusPoints += pointsPerMob;
        EventManager.Instance.onPointsChangeEvent(pointsPerMob);
        showTime = maxShowTime;

		Vector3 trapPosition = args.GameObject.transform.position;
        /*
		GameObject label = (GameObject)Instantiate(labelTemplate);
		label.guiText.text = "123";
		label.transform.position = new Vector3(trapPosition.x, trapPosition.y, -2);
        */
        //StartCoroutine(DestroyLabel(label));
	}

	IEnumerator DestroyLabel(GameObject label)
	{
		yield return new WaitForSeconds(2);

		Destroy(label);
	}

    /*void Update()
    {
        if(showTime > 0)
        {
            showTime -= Time.deltaTime;

            //guiText.text = "+" + plusPoints;
            guiText.text = "||+" + plusPoints;
        }
        else
        {
            plusPoints = 0;
            //guiText.text = "";
        }
    }
    */
    /*
    IEnumerator ShowPlusPoints()
    {
        Debug.Log("plus: "+ plusPoints);
        guiText.text = "+" + plusPoints;

        yield return new WaitForSeconds(2);

        plusPoints = 0;
        guiText.text = "";
    }
    */
}
