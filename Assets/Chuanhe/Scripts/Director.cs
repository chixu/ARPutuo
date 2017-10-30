using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System;
using System.Xml.Linq;

public class Director {
	public static string SCAN_OBJECT_PREFIX = "AR-";
	public static Environment environment;
	public static UIStyle style;
	public static Version version;
	public static TrackerManager trackerManager;
	public static UserBehavior userBehavior;
	private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject> ();

	public static void Initialize(XElement config){
		trackerManager = new TrackerManager ();
		style = new UIStyle ();
		var trackers = config.Element ("trackers").Elements();
		foreach (XElement n in trackers) {
			string name = Xml.Attribute (n, "name");
			ITracker tracker = null;
			if (name == "unity")
				tracker = new UnityTracker ();
			else if (name == "umeng") {
				string appid = "";
				#if UNITY_IPHONE
				appid = Xml.Attribute (n, "iosid");
				#else
				appid = Xml.Attribute (n, "androidid");
				#endif
				if(!string.IsNullOrEmpty(appid))
					tracker = new UmengTracker (appid);
			}
			if (tracker != null) {
				var events = n.Element ("events");
				if (events != null) {
					var trackevents = events.Elements ();
					foreach (XElement nn in trackevents) {
						string eventname = Xml.Attribute (nn, "name");
						tracker.eventBlacklist.Add (eventname, eventname);
					}
				}
				trackerManager.AddTracker (tracker);
			}
		}
		userBehavior = new UserBehavior ();
	}

	public static GameObject LoadPrefab(string str, GameObject parent = null){
		GameObject prefab = null;
		if (prefabs.ContainsKey (str)) {
			prefab = prefabs [str];
		} else {
			prefab = Resources.Load<GameObject> ("Prefabs/" + str);
			prefabs.Add (str, prefab);
		}
		GameObject ins = GameObject.Instantiate (prefab);
		if (parent != null) {
			Quaternion rot = ins.transform.localRotation;
			Vector3 pos = ins.transform.localPosition;
			Vector3 rectPos = Vector3.zero;
			RectTransform rectTrans = ins.GetComponent<RectTransform> ();
			if(rectTrans != null)
				rectPos = rectTrans.localPosition;
			ins.transform.SetParent (parent.transform, false);
			ins.transform.localRotation = rot;
			ins.transform.position = pos;
			if(rectTrans != null)
				rectTrans.localPosition = rectPos;
		}
		return ins;
	}

	public static IEnumerator Load(){
		yield return userBehavior.Load ();
	}
}


public class UIStyle{

	public Color mainColor;
	public Color uiGrey;

	public UIStyle(){

	}

}