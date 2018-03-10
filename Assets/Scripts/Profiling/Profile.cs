using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

/// <summary>
/// Fixing slow code check list:
/// 	1. Is it required to run at all?
/// 	2. Is it required to run as often as it does?
/// 	3. Is it required to run in as many places as it does?
/// 	4. Can we make it run more efficiently?
/// </summary>
public class Profile {
	
	static int timerNameCounter = 0;
	
	static Stack<int> timerIndex = new Stack<int>();
	
	static List<TimeEntry> timeEntries = new List<TimeEntry>();


	class TimeEntry {
		public string name;
		public float time;
		public int startIndex;
		public int indent;

		public TimeEntry(string name, int startIndex, int indent) {
			this.name = name;
			this.time = Time.realtimeSinceStartup;
			this.startIndex = startIndex;
			this.indent = indent;
		}
	}

	/// <summary>
	/// Starts a new timer.
	/// </summary>
	public static void StartTimer() {
		StartTimer("Section " + timerNameCounter++);
	}

	/// <summary>
	/// Starts a new timer with the specified name
	/// </summary>
	/// <param name="name">Name of the timer</param>
	public static void StartTimer(string name) {
		timeEntries.Add(new TimeEntry(name, -1, timerIndex.Count));
		timerIndex.Push(timeEntries.Count-1);
	}
	
	/// <summary>
	/// Ends the last timer started
	/// </summary>
	public static void EndTimer() {
		int last = timerIndex.Pop();
		timeEntries.Add(new TimeEntry(timeEntries[last].name, last, timerIndex.Count));
	}

	/// <summary>
	/// Returns the last time result without removing anything
	/// </summary>
	/// <returns>The last time result.</returns>
	public static float PeekLastTime() {
		if(timeEntries.Count > 0) {
			TimeEntry last = timeEntries[timeEntries.Count - 1];
			if(last.startIndex >= 0) {
				return (float)Math.Round((double)(last.time - timeEntries[last.startIndex].time), 4);
			}
		}
		return -1;
	}

	/// <summary>
	/// Clears all results.
	/// </summary>
	public static void Clear() {
		timerIndex.Clear();
		timeEntries.Clear();
	}

	/// <summary>
	/// Outputs the results of the profiling to the debugger
	/// </summary>
	public static void WriteResults() {

		foreach(TimeEntry timeEntry in timeEntries) {
			StringBuilder line = new StringBuilder();
			line.Append('\t',timeEntry.indent);

			if(timeEntry.startIndex >= 0) { //This is an end time
				line.Append("/");
				line.Append(timeEntry.name);
				line.Append(" : ");
				line.Append(timeEntry.time - timeEntries[timeEntry.startIndex].time);
			} else {
				line.Append(timeEntry.name);
			}
			Debug.Log(line.ToString());
		}
	}
}
