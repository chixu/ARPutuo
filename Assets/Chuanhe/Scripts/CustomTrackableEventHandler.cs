/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Collections.Generic;

//using System;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;


public interface ITrackableController
{
	void OnTrackingFound ();

	void OnTrackingLost ();
}

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class CustomTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
{

	public string videoPath;
	public string subtitlePath;
	public List<ITrackableController> controllers = new List<ITrackableController> ();

	#region PRIVATE_MEMBER_VARIABLES

	private TrackableBehaviour mTrackableBehaviour;
	//private bool isForcedTrackingLost = false;
	//private ImageTargetBehaviour ITB;
	private TouchRotate touchRotate;
	//public MediaPlayer mediaPlayer;
	//public bool isMenu = false;
	public string type;

	#endregion // PRIVATE_MEMBER_VARIABLES


	public virtual void Start ()
	{	
		mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
		if (mTrackableBehaviour) {
			mTrackableBehaviour.RegisterTrackableEventHandler (this);
		}
//			if (myCanvas) {
//				myCanvas.SetActive (false);
//			}

		touchRotate = this.GetComponentInChildren<TouchRotate> ();
//			mediaPlayer = this.GetComponentInChildren<MediaPlayer> ();
//			if (mediaPlayer) {
//				mediaPlayer.m_VideoPath = mediaPlayer.m_VideoPath.Replace ("{%persistentPath%}", Application.persistentDataPath);
//				Debug.Log (mediaPlayer.m_VideoPath);
//				mediaPlayer.PostStart ();
//			}
	}



	#region PUBLIC_METHODS

	/// <summary>
	/// Implementation of the ITrackableEventHandler function called when the
	/// tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged (
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		     newStatus == TrackableBehaviour.Status.TRACKED ||
		     newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			OnTrackingFound ();
		} else {
			OnTrackingLost ();
		}
	}

	#endregion // PUBLIC_METHODS


	virtual protected void OnTrackingFound ()
	{
//			if (touchRotate) {
//				touchRotate.enabled = true;
//			}
		ScanSceneController.currentTrackableObject = this.gameObject;
		Canvas[] canvas = GetComponentsInChildren<Canvas> (true);
		Light[] LightComponents = GetComponentsInChildren<Light> (true);

		foreach (Canvas ca in canvas) {
			ca.gameObject.SetActive (true);
		}// Enable light:
		foreach (Light component in LightComponents) {
			component.enabled = true;
		}
//			foreach (AudioSource audio in audios)
//			{
//				audio.Play ();
//			}
//			if(!string.IsNullOrEmpty(videoPath))
//				PlayVideo (videoPath);
		for (int i = 0; i < controllers.Count; i++)
			controllers [i].OnTrackingFound ();
		ScanSceneController.instant.SetState (type);
	}

	//		public void PlayVideo(string path){
	//			if(mediaPlayer) {
	//				mediaPlayer.OpenVideoFromFile
	//				//VCR.instant
	//				VideoController.instant._videoSeekSlider.gameObject.SetActive(true);
	//				mediaPlayer.Rewind(false);
	//				mediaPlayer.Play ();
	//			}
	//		}


	virtual protected void OnTrackingLost ()
	{
//			if (touchRotate) {
//				touchRotate.enabled = false;
//			}
		//ScanSceneController.currentTrackableObject = null;
		Canvas[] canvas = GetComponentsInChildren<Canvas> (true);
		Light[] LightComponents = GetComponentsInChildren<Light> (true);
		//AudioSource[] audios = GetComponentsInChildren<AudioSource>(true);
		foreach (Canvas ca in canvas) {
			ca.gameObject.SetActive (false);
		}
		foreach (Light component in LightComponents) {
			component.enabled = false;
		}
		for (int i = 0; i < controllers.Count; i++)
			controllers [i].OnTrackingLost ();
//			foreach (AudioSource audio in audios)
//			{
//				audio.Stop ();
//			}
		if (!ScanSceneController.instant.exited && !ScanSceneController.instant.videoPanel.shown)
			ScanSceneController.instant.SetState ("idle");
	}

	//		public void StopVideo(){
	//			if (mediaPlayer) {
	//				VideoController.instant._videoSeekSlider.gameObject.SetActive (false);
	//				mediaPlayer.Stop ();
	//			}
	//		}

	//		#endregion // PRIVATE_METHODS
}

