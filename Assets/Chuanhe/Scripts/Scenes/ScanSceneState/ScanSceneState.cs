 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Vuforia;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

public class ScanSceneState
{
	public ScanSceneController scene;
	public string name;
	protected float trackTimer;
	//public Dictionary<string, ScanSceneState> states;

	public static ScanSceneState GetState(string name){
		if (name == "idle")
			return new ScanIdleState ();
		else if(name == "object")
			return new ScanObjectState ();
		else if(name == "video")
			return new ScanVideoState ();
		else if(name == "menuvideo")
			return new ScanMenuVideoState ();
		else if(name == "menuobject")
			return new ScanMenuObjectState ();
		else
			return new ScanMenuState ();
	}

	public virtual void OnTrackFound(){

	}

	public virtual void Update(){

	}

	public virtual void OnTrackLost(){

	}

	public virtual void OnBackClick(){
		scene.SetState ("idle");
	}

	public virtual void OnEnter(Hashtable args = null){
		StopVideoPanel ();
		if (ScanSceneController.currentTrackableObject == null)
			return;
		CustomTrackableEventHandler cteh = ScanSceneController.currentTrackableObject.GetComponent<CustomTrackableEventHandler> ();
		if (cteh) {
			ScanSceneController.instant.subtitle.Play (cteh.subtitlePath);
		}
	}

	protected void StopVideoPanel(){
		if (ScanSceneController.instant.videoPanel.shown) {
			ScanSceneController.instant.videoPanel.Hide();
			VideoController.instant.Stop ();
		}
	}

	public virtual void OnExit(){
		//ScanSceneController.instant.videoPanel.SetActive (false);
		ScanSceneController.instant.subtitle.Stop ();
	}

	public void RegisterClickHandler(Action<GameObject> action){
		if (Configuration.instant.enablePopupVideo) {
			GameObject curr = ScanSceneController.currentTrackableObject.transform.GetChild(0).gameObject;
			OnClick click = curr.GetComponent<OnClick> ();
			if (click != null) {
				click.OnClickHandler = action;
			}
		}
	}

	protected void TrackStart(string name, string type){
		Director.trackerManager.TrackEvent(TrackerEventName.TrackingStart, new Dictionary<string, object>(){{"Scene",scene.sceneName}, {"Name",name}, {"Type",type}});
		trackTimer = Time.fixedTime;
	}

	protected void TrackEnd(string name, string type){
		Director.trackerManager.TrackEvent(TrackerEventName.TrackingEnd, new Dictionary<string, object>(){{"Scene",scene.sceneName},{"Name",name}, {"Type",type}, {"Timer",Time.fixedTime-trackTimer}});
	}
}
