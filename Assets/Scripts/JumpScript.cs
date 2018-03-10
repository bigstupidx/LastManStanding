using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour
{

	// Update is called once per frame
	void OnTouchDown()
	{
		GameObject.FindGameObjectWithTag("Player").SendMessage("PressedJumpButton", null, SendMessageOptions.DontRequireReceiver);
	}
}
