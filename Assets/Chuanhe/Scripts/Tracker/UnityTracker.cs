using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityTracker: ITracker {
	public Dictionary<string, string> eventBlacklist{ get; set; }

	public UnityTracker(){
		eventBlacklist = new Dictionary<string, string> ();
		eventBlacklist = new Dictionary<string, string> ();
	}

	public void Initialize(){

	}

	public void TrackEvent(string eventName, Dictionary<string, object> data){
		if (eventBlacklist.ContainsKey (eventName))
			return;
		Logger.Log ("UnityTracker " + eventName + " " + Print.Dictionary(data), "blue");
		Analytics.CustomEvent(eventName, data);
	}
}
