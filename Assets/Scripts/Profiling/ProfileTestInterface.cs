using UnityEngine;
using System.Collections;

public class ProfileTestInterface : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Profile.StartTimer("All of Start");
		OuterProcess();
		Profile.EndTimer();
		Profile.WriteResults();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OuterProcess() {
		for(int i = 0; i < 3; i++) {
			Profile.StartTimer("Process");
			Process();
			Profile.EndTimer();
		}
	}

	private void Process() {
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/100f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
	
	private void LargeMethod() {
		Profile.StartTimer("LargeMethod");
		ProcessA();
		ProcessB();
		ProcessC();
		ProcessD();
		ProcessE();
		ProcessF();
		Profile.EndTimer();
	}


	void ProcessA ()
	{
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/300f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
	void ProcessB ()
	{
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/600f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
	void ProcessC ()
	{
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/600f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
	void ProcessD ()
	{
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/300f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
	void ProcessE ()
	{
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/10f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
	void ProcessF ()
	{
		double t = double.MinValue;
		for(int i = 0; i < int.MaxValue/500f; i++) {
			t = Mathf.Sqrt(i);
		}
	}
}
