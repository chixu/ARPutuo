using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager {

	private List<ITracker> trackers;
	private float loadingSceneTime = -1;
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

	public void PrepareTrackLoadingScene(){
		loadingSceneTime = Time.time;
	}

	public float GetLoadingSceneTime(){
		if (loadingSceneTime != -1) {
			return Time.time - loadingSceneTime;
		} else
			return -1;
	}

//	public void TrackLoadingScene(){
//		if (loadingSceneTime != -1) {
//			TrackEvent (TrackerEventName.SceneLoad, new Dictionary<string, object>(){{"time", Time.time-loadingSceneTime}});
//		}
//	}
}

public class TrackerEventName{
	public static string TrackingStart = "TrackingStart";
	public static string TrackingEnd = "TrackingEnd";
	public static string SceneEnter = "SceneEnter";
}