using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umeng;

public class UmengTracker: ITracker {

	public Dictionary<string, string> eventBlacklist { get; set; }

	public UmengTracker(string appid){
		GA.StartWithAppKeyAndChannelId (appid, "App Strore");
		eventBlacklist = new Dictionary<string, string> ();
		//GA.SetLogEnabled(
	}

	public void Initialize(){

	}

	public void TrackEvent(string eventName, Dictionary<string, object> data){
		if (eventBlacklist.ContainsKey (eventName))
			return;
		Logger.Log ("UnityTracker " + eventName + " " + Print.Dictionary(data), "blue");
		Dictionary<string, string> d = new Dictionary<string, string>();
		foreach (string key in data.Keys)
			d.Add (key, data [key].ToString ());
		GA.Event (eventName, d);
	}
}
