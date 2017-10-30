using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager {

	private List<ITracker> trackers;
	//public static TrackerManager instant;

	public TrackerManager(){
		//instant = this;
		trackers = new List<ITracker> ();
	}

	public void AddTracker(ITracker tracker){
		trackers.Add (tracker);
		tracker.Initialize ();
	}

	public void TrackEvent(string eventName, Dictionary<string, object> data){
		for (int i = 0; i < trackers.Count; i++) {
			trackers [i].TrackEvent (eventName, data);
		}
	}
}

public class TrackerEventName{
	public static string TrackingStart = "TrackingStart";
	public static string TrackingEnd = "TrackingEnd";
	public static string SceneEnter = "SceneEnter";
}