/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
//using UnityEngine.AI;

using System.Xml.Linq;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>

public class LvweiScanScene : ScanSceneController
{
	private AudioClip currentClip;
	public GameObject ARCamera;
	public bool isAudioPlayed = true;

	protected  override IEnumerator LoadDataSet ()
	{
		ClearAndLoadDataSet ();
		IEnumerable<TrackableBehaviour> tbs = Vuforia.TrackerManager.Instance.GetStateManager ().GetTrackableBehaviours ();
		GameObject buttonCanvasPrefab = null, buttonPrefab = null;// = Resources.Load("Prefab/ScanButtonCanvas") as GameObject;
		foreach (TrackableBehaviour tb in tbs) {

			Logger.Log (tb.TrackableName, "purple");
			tb.gameObject.name = tb.TrackableName;
			XElement info = Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName);
			if (info == null)
				continue;
			string objType = Xml.Attribute (info, "type");
			//tb.gameObject.AddComponent<DefaultTrackableEventHandler> ();
			FairyTrackableEventHandler fth = tb.gameObject.AddComponent<FairyTrackableEventHandler> ();
			tb.gameObject.AddComponent<TurnOffBehaviour> ();
			//tb.gameObject.GetComponent<FairyTrackableEventHandler> ();
			//fth.scene = this;
			fth.type = "object";
			fth.subtitlePath = GetAssetsPath (Xml.Attribute (info, "subtitle"), true);
			fth.mp3Src = Xml.Attribute (info, "src");
			UnityEngine.Object asset = null;

			string prefabName = Xml.Attribute (info, "prefab");
			asset = loadedAssets.ContainsKey (prefabName) ? loadedAssets [prefabName] : new GameObject ();
			//CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
			GameObject obj = InstantiateObject (tb, asset);
			obj.transform.localRotation = Quaternion.Euler (Vector3.zero);
			obj.transform.GetChild (0).localScale = Vector3.one * 0.6f;
			fth.Initialize (this);
		}
		ObjectTracker objectTracker = Vuforia.TrackerManager.Instance.GetTracker<ObjectTracker> ();
		if (!objectTracker.Start ()) {
			Debug.Log ("<color=yellow>Tracker Failed to Start.</color>");
		}
		yield return null;
	}


	void InstantiateFairyAudioObject (XElement info, TrackableBehaviour tb, out UnityEngine.Object asset)
	{
		string prefabName = Xml.Attribute (info, "prefab");
		asset = loadedAssets.ContainsKey (prefabName) ? loadedAssets [prefabName] : new GameObject ();
		var buttonNodes = info.Elements ();
		GameObject bPanel = null;
		CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
		GameObject obj = InstantiateObject (tb, asset);
//		obj.AddComponent<TouchRotate> ();
//		TouchRotate touchrotate = obj.GetComponent<TouchRotate> ();
//		touchrotate.scalable = Xml.Boolean (info, "scalable", true);
//		touchrotate.upEnabled = Xml.Boolean (info, "upEnabled", true); 
//		touchrotate.upDireciton = Xml.Attribute (info, "upDirection", "x"); 
//		touchrotate.rightEnabled = Xml.Boolean (info, "rightEnabled", true); 
//		touchrotate.rightDirection = Xml.Attribute (info, "rightDirection", "-z"); 
//		ITrackableController[] controllers = obj.GetComponents<ITrackableController> ();
//		cte.controllers.AddRange (controllers);
//		for (int i = 0; i < buttons.Count; i++) {
//			buttons [i].target = obj;
//			cte.controllers.Add (buttons [i]);
//		}
	}


	private AudioSource GetAudioSource ()
	{
		AudioSource audio = GetComponent<AudioSource> ();
		if (audio != null)
			return audio;
		gameObject.AddComponent<AudioSource> ();
		audio = GetComponent<AudioSource> ();
		return audio;
	}

	public void OnTrackChanged(FairyTrackableEventHandler handler){
		if (ScanSceneController.currentTrackableObject == null || ScanSceneController.currentTrackableObject.name != handler.gameObject.name || isAudioPlayed) {
			Logger.Log ("OnTrackChanged " +  (ScanSceneController.currentTrackableObject==null?"":ScanSceneController.currentTrackableObject.name) + " " + handler.gameObject.name, "green");
			StartCoroutine (PlayAudio (handler.mp3Src));
			animator = handler.content.GetChild (0).gameObject.GetComponent<Animator> ();
			RuntimeAnimatorController ac = animator.runtimeAnimatorController;
			AnimationClip[] clips = ac.animationClips;
			animatorStates = new List<String> ();
			for (int i = 0; i < clips.Length; i++) {
				Logger.Log ("OnTrackChanged " + clips [i].name, "green");
				if (clips [i].name != "Idle") {
					animatorStates.Add (clips [i].name);
				}
			}
			subtitle.Play (handler.subtitlePath);
			nextAnimatorChangedTime = Time.frameCount + 300;
		}
//		Transform cameraContainer = ARCamera.GetChildByName ("ObjectContainer").transform;
//		if (cameraContainer.childCount > 0 && ScanSceneController.currentTrackableObject != null) {
//			Transform prevObj = cameraContainer.GetChild (0);
//			prevObj.gameObject.SetActive (false);
//			prevObj.SetParent (ScanSceneController.currentTrackableObject.transform, false);
//			//prevObj.localRotation = Quaternion.Euler(Vector3.zero);
//		}
//		if(handler.transform.childCount > 0)
//			handler.transform.GetChild(0).gameObject.SetActive (true);
		if (ScanSceneController.currentTrackableObject != null) {
			FairyTrackableEventHandler prev = ScanSceneController.currentTrackableObject.GetComponent<FairyTrackableEventHandler> ();
			prev.ExitFullscreen ();
			prev.content.gameObject.SetActive (false);
		}
		handler.content.gameObject.SetActive(true);
		handler.content.gameObject.GetComponentInChildren<PlayerController> (true).isTalking = true;
	}


	public void OnTrackLost(FairyTrackableEventHandler handler){
		if (ScanSceneController.currentTrackableObject != null) {
			handler.Fullscreen ();
		}else
			handler.content.gameObject.SetActive (false);
	}

	protected IEnumerator PlayAudio (string path)
	{
		//Logger.Log (Request.ResolvePath (Application.persistentDataPath + "/" + path), "red");
		isAudioPlayed = false;
		WWW www = new WWW (Request.GetPersistentPath(path));
		yield return www;
		currentClip = www.GetAudioClip ();
		AudioSource audio = GetAudioSource ();
		audio.clip = currentClip;
		audio.Play ();
		CancelInvoke ();
		Invoke("AudioPlayed",  audio.clip.length);
	}

	private void AudioPlayed(){
		isAudioPlayed = true;
		PlayerController player = ScanSceneController.currentTrackableObject.GetComponentInChildren<PlayerController> (true);
		if (player != null)
			player.isTalking = false;
		else {
			player = ARCamera.GetChildByName ("ObjectContainer").GetComponentInChildren<PlayerController> (true);
			if (player != null)
				player.isTalking = false;
		}
	}

	public Animator animator;
	public List<string> animatorStates = new List<string>();
	public int nextAnimatorChangedTime;

	// Update is called once per frame
	void Update () {
		if (animator!=null && nextAnimatorChangedTime <= Time.frameCount && !isAudioPlayed) {
			animator.Play (animatorStates[UnityEngine.Random.Range(0, animatorStates.Count)]);
			nextAnimatorChangedTime = Time.frameCount + UnityEngine.Random.Range(200, 400);
		}
	}

	//	static string ResolvePath (string path, bool addFilePrefix = true)
	//	{
	//		if (!addFilePrefix)
	//			return path;
	//		string str = "file:///" + path;
	//		str = str.Replace ("file:////", "file:///");
	//		return str;
	//	}
}