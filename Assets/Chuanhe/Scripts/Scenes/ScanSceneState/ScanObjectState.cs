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

public class ScanObjectState:ScanSceneState
{
	private AudioSource audio;
	private Vector3 localScale;
	private Quaternion localRotation;
	private Transform firstChild;
	//private Vector3 localScale;

	public ScanObjectState(){
		name = "object";
	}

	public override void OnEnter(Hashtable args = null){
		base.OnEnter ();
		audio = ScanSceneController.currentTrackableObject.GetComponent<AudioSource> ();
		TrackStart (ScanSceneController.currentTrackableObject.name, "Object");
		if (audio)
			audio.Play ();
		firstChild = ScanSceneController.currentTrackableObject.transform.GetChild (0);
		if (firstChild != null) {
			localScale = firstChild.localScale;
			firstChild.localRotation = Quaternion.identity;
		}
	}

	public override void OnExit(){
		TrackEnd (ScanSceneController.currentTrackableObject.name, "Object");
		if (audio)
			audio.Stop ();
		if (firstChild != null && localScale!=null) {
			firstChild.localScale = localScale;
			//firstChild.localRotation = localRotation;
		}
		base.OnExit ();
	}
}
