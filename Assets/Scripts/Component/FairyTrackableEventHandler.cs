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


/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class FairyTrackableEventHandler: CustomTrackableEventHandler
{
	public LvweiScanScene scene;
	public string mp3Src;
	public Transform content;
	public Transform cameraContainer;
	public bool isFullscreen = false;

	public void Initialize(LvweiScanScene s){
		scene = s;
		content = transform.GetChild(0);
		cameraContainer = scene.ARCamera.GetChildByName ("ObjectContainer").transform;
	}

	protected override void OnTrackingFound ()
	{
		//if(ScanSceneController.currentTrackableObject == null || ScanSceneController.currentTrackableObject.name != gameObject.name)
		scene.OnTrackChanged(this);
		base.OnTrackingFound ();
	}

	protected override void OnTrackingLost ()
	{
		scene.OnTrackLost(this);
		if (!ScanSceneController.instant.exited && ScanSceneController.currentTrackableObject == null)
			ScanSceneController.instant.SetState ("idle");
	}

	public void ExitFullscreen(){
		if (isFullscreen) {
			content.SetParent (transform, false);
			isFullscreen = false;
		}
	}

	public void Fullscreen(){
		if (!isFullscreen) {
			content.SetParent (cameraContainer, false);
			isFullscreen = true;
		}
	}
}

