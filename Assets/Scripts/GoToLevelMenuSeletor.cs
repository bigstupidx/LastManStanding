using UnityEngine;
using System.Collections;

public class GoToLevelMenuSeletor : MonoBehaviour {

    public string menuSeletorTag;

    public string level;

	void OnTouchDown ()
    {
        GameObject.FindGameObjectWithTag(menuSeletorTag).SendMessage("LoadLevel", level, SendMessageOptions.DontRequireReceiver);
	}
}
